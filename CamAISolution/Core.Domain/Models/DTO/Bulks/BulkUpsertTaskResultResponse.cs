using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

public record BulkUpsertTaskResultResponse(BulkUpsertStatus Status, int Inserted, int Updated, int Failed, string Description = "" , params object[] Metadata);

[Lookup]
public enum BulkUpsertStatus
{
    Success,
    Fail,
}