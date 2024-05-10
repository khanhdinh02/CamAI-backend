namespace Core.Application.Events.Args;

public class BulkUpsertCurrentProgressArgs(int currentFinishedRecord, string taskId) : EventArgs
{
    public int CurrentFinishedRecord => currentFinishedRecord;
    public string TaskId => taskId;
}