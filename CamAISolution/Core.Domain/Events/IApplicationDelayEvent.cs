namespace Core.Domain.Events;

/// <summary>
///     Every properties of derived class must be public and have { get; set; } syntax in order to set the value.
///     Ensure that all Properties have assign into DI container.
///     Declaring properties in constructor will be ignored.
/// </summary>
public interface IApplicationDelayEvent
{
    Task UseDelay();
    Task InvokeAsync();
}