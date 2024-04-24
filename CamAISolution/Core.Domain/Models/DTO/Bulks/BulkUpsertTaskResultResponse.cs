namespace Core.Domain.DTO;

public record BulkUpsertTaskResultResponse(int Added, int Updated, params object[] Metadata);