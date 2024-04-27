using System.Collections.Concurrent;
using Core.Application.Exceptions;
using Core.Domain.DTO;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Host.CamAI.API.Utils;

public static class ManageTaskHelper
{
    private static readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, Task<BulkUpsertTaskResultResponse>>>
        upsertTasks = new();

    public static void AddUpsertTask(Guid actorId, Task<BulkUpsertTaskResultResponse> task, string taskId)
    {
        if (upsertTasks.TryGetValue(actorId, out var actorCurrentTasks))
        {
            actorCurrentTasks.TryAdd(taskId, task);
        }
    }

    public static void GetTaskByActorId(Guid actorId, out List<string> taskIds)
    {
        if (!upsertTasks.TryGetValue(actorId, out var tasks))
            throw new NotFoundException("Not found any task");
        taskIds = tasks.Keys.ToList();
    }

    public static void GetTaskById(Guid actorId, string taskId, out Task<BulkUpsertTaskResultResponse> result)
    {
        if (!upsertTasks.TryGetValue(actorId, out var tasks))
            throw new NotFoundException("Not found any task");
        if(!tasks.TryGetValue(taskId, out var task))
            throw new NotFoundException("Not found any task");
        result = task;
    }

    public static void RemoveTaskById(Guid actorId, string taskId)
    {
        if (upsertTasks.TryGetValue(actorId, out var tasks) && tasks.TryRemove(taskId, out _) && !tasks.Any())
            upsertTasks.TryRemove(actorId, out _);
    }
}