using System.Collections.Concurrent;
using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class BulkTaskService(ICacheService cacheService) : IBulkTaskService
{
    private readonly ConcurrentDictionary<
        Guid,
        ConcurrentDictionary<string, Task<BulkUpsertTaskResultResponse>>
    > upsertTasks = new();

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
        if (!upsertTasks.TryGetValue(actorId, out var tasks))
            throw new NotFoundException("Not found any task");
        taskIds = tasks.Keys.ToList();
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

        if(tasks.IsEmpty)
            upsertTasks.TryRemove(actorId, out _);

        cacheService.Set(taskId, task, TimeSpan.FromDays(1));
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

        // check and wait for current task
        GetTaskById(actorId, taskId, out var bulkTask);
        using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        tokenSource.CancelAfter(timeout);
        var timeoutTask = Task.Delay(-1, tokenSource.Token);
        var completedTask = await Task.WhenAny(timeoutTask, bulkTask);
        if (completedTask == bulkTask)
            return await bulkTask;
        return null;
    }
}
