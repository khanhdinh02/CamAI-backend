namespace Core.Domain.Events;

public interface IObserver<TEventArgs>
    where TEventArgs : EventArgs
{
    void Update(object? sender, TEventArgs args);
}
