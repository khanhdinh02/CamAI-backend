namespace Core.Domain.Events;

public interface ISubject<in T, in TEventArgs>
    where TEventArgs : EventArgs
{
    void OnChange(TEventArgs e);
    void Attach(T observer);
    void Detach(T observer);
}
