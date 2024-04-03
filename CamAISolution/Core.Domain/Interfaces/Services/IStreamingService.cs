namespace Core.Domain.Interfaces.Services;

public interface IStreamingService
{
    Task<Uri> StreamCamera(Guid id);
}
