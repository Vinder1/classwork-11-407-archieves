namespace EmeraldFramework.Controller.Tree;

public class ControllerEndPoint
{
    public async Task InvokeAsync()
    {
        var task = Action?.Invoke();
        if (task == null)
            return;
        await task;
    }

    public string? RoleRequired { get; init; }
    public string? Redirect { get; init; }
    public Type? RequiredModelType { get; init; }

    internal Func<Task> Action { get; init; } = null!;
}
