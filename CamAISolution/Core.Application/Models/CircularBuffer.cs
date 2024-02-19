using Core.Domain.Models;

namespace Core.Application.Models;

public class CircularBuffer<T>(int capacity) : ICircularBuffer<T>
{
    private readonly T[] buffer = new T[capacity];
    private int head;
    private int tail;
    private int count;

    public void Write(T item)
    {
        buffer[head] = item;
        head = (head + 1) % buffer.Length;
        count = Math.Min(count + 1, buffer.Length);
    }

    public T Read()
    {
        if (count == 0)
            throw new InvalidOperationException("Buffer is empty");

        var item = buffer[tail];
        tail = (tail + 1) % buffer.Length;
        count--;
        return item;
    }

    public T Peek()
    {
        if (count == 0)
            throw new InvalidOperationException("Buffer is empty");

        return buffer[tail];
    }

    public bool IsEmpty => count == 0;

    public bool IsFull => count == buffer.Length;

    public int Count => count;
}
