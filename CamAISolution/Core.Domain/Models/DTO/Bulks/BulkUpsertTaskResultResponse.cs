namespace Core.Domain.DTO;

public record BulkUpsertTaskResultResponse(int Inserted, int Updated, int Failed, params object[] Metadata);