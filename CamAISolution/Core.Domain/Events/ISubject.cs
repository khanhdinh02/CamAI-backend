namespace Core.Domain.Events;

public interface ISubject<in T, in TEventArgs>
    where TEventArgs : EventArgs
{
    void Notify(TEventArgs e);
    void Attach(T observer);
    void Detach(T observer);
}
