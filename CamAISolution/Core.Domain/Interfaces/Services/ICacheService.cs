namespace Core.Domain.Services;

public interface ICacheService
{
    /// <summary>
    /// Get or create cached object.
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? Get<T>(string key);
    T Set<T>(string key, T value, TimeSpan expired);
    public Task<Guid> GetAdminAccount();
}
