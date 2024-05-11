using System.Collections.Concurrent;
using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class BulkTaskService : IBulkTaskService, Domain.Events.IObserver<BulkUpsertCurrentProgressArgs>
{
    private readonly ICacheService cacheService;
    private readonly BulkUpsertProgressSubject bulkUpsertProgressSubject;
    private readonly IAppLogging<BulkTaskService> logger;
    public BulkTaskService(ICacheService cacheService, BulkUpsertProgressSubject bulkUpsertProgressSubject, IAppLogging<BulkTaskService> logger)
    {
        this.cacheService = cacheService;
        this.bulkUpsertProgressSubject = bulkUpsertProgressSubject;
        this.logger = logger;
        bulkUpsertProgressSubject.Attach(this);
    }
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, Task<BulkUpsertTaskResultResponse>>> upsertTasks = new();

    private readonly ConcurrentDictionary<string, int> bulkTaskProgresses = new();

    public void AddUpsertTask(Guid actorId, Task<BulkUpsertTaskResultResponse> task, string taskId)
    {
        if (upsertTasks.TryGetValue(actorId, out var actorCurrentTasks))
            actorCurrentTasks.TryAdd(taskId, task);
        else
        {
            var ownTasks = new ConcurrentDictionary<string, Task<BulkUpsertTaskResultResponse>>();
            ownTasks.TryAdd(taskId, task);
            upsertTasks.TryAdd(actorId, ownTasks);
        }
    }

    public void GetTaskByActorId(Guid actorId, out List<string> taskIds)
    {
        taskIds = new();

        if (upsertTasks.TryGetValue(actorId, out var tasks))
            taskIds = tasks.Keys.ToList();

        var cacheTaskIds = cacheService.Get<HashSet<string>>(actorId.ToString("N"));
        if (cacheTaskIds != null && cacheTaskIds.Count > 0)
            taskIds.AddRange(cacheTaskIds);
    }

    public void GetTaskById(Guid actorId, string taskId, out Task<BulkUpsertTaskResultResponse> result)
    {
        if (!upsertTasks.TryGetValue(actorId, out var tasks))
            throw new NotFoundException("Not found any task");
        if (!tasks.TryGetValue(taskId, out var task))
            throw new NotFoundException("Not found any task");
        result = task;
    }

    public void RemoveTaskById(Guid actorId, string taskId)
    {
        if (!upsertTasks.TryGetValue(actorId, out var tasks) || !tasks.TryRemove(taskId, out var task))
        {
            return;
        }

        if (tasks.IsEmpty)
            upsertTasks.TryRemove(actorId, out _);

        var removedTaskIds = cacheService.Get<HashSet<string>>(actorId.ToString("N"));
        if (removedTaskIds == null)
            removedTaskIds = new HashSet<string> { taskId };
        else
            removedTaskIds.Add(taskId);
        bulkTaskProgresses.TryRemove(taskId, out _);
        cacheService.Set(actorId.ToString("N"), removedTaskIds, TimeSpan.FromDays(1));
        cacheService.Set(taskId, task, TimeSpan.FromDays(1), (key, _) =>
        {
            // Clean up when data is evicted.
            var tasks = cacheService.Get<HashSet<string>>(actorId.ToString("N"));
            if (tasks == null)
                return;
            tasks.Remove(taskId);
            if (tasks.Count == 0)
                cacheService.Remove(actorId.ToString("N"));
            else
                cacheService.Set(actorId.ToString("N"), removedTaskIds, TimeSpan.FromDays(1));
        });
    }

    public async Task<BulkUpsertTaskResultResponse?> GetBulkUpsertTaskResultResponse(
        Guid actorId,
        string taskId,
        CancellationToken cancellationToken,
        TimeSpan timeout
    )
    {
        // Check cache for finished task
        var finishedTaskResult = cacheService.Get<Task<BulkUpsertTaskResultResponse>>(taskId);
        if (finishedTaskResult != null)
            return await finishedTaskResult;

        // check and wait for current running task
        GetTaskById(actorId, taskId, out var bulkTask);
        using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        tokenSource.CancelAfter(timeout);
        var timeoutTask = Task.Delay(-1, tokenSource.Token);
        var completedTask = await Task.WhenAny(timeoutTask, bulkTask);
        if (completedTask == bulkTask)
            return await bulkTask;
        return null;
    }

    public (int, int) GetTaskProgress(string taskId)
    {
        var total = cacheService.Get<int>($"total-records-{taskId}");
        if (total == 0)
            return new(1, 1);
        if (!bulkTaskProgresses.TryGetValue(taskId, out var currentFinishedRecords))
            return new(1, 1);
        return new(currentFinishedRecords, total);
    }

    public void Update(object? sender, BulkUpsertCurrentProgressArgs args)
    {
        logger.Info($"task {args.TaskId}, current finished records: {args.CurrentFinishedRecord}");
        bulkTaskProgresses[args.TaskId] = args.CurrentFinishedRecord;
    }

    ~BulkTaskService()
    {
        bulkUpsertProgressSubject.Detach(this);
    }
}
