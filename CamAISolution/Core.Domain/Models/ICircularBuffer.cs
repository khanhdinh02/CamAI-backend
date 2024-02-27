namespace Core.Domain.Models;

public interface ICircularBuffer<T>
{
    void Write(T item);
    T Read();
    T Peek();
    bool IsEmpty { get; }
    bool IsFull { get; }
    int Count { get; }
}
