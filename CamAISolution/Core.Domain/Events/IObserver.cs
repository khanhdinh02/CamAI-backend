namespace Core.Domain.Events;

public interface IObserver<in TEventArgs>
    where TEventArgs : EventArgs
{
    void Update(object? sender, TEventArgs args);
}
