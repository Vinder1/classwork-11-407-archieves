using ControlWorkApp.Dto;
using ControlWorkApp.Repositories;
using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;

namespace ControlWorkApp.Controllers;

[Controller]
public class TestController(TestRepository testRepository)
{
    private TestRepository Repository => testRepository;

    [Controller.Post("tests")]
    public async Task TryCreate(Emerald.AppContext context)
    {
        var signupInfo = await context.ReadJsonAsync<NewTestDto>(context.CancellationToken);

        if (signupInfo == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var session = await Repository.CreateTestAsync(
            signupInfo,
        context.CancellationToken);

        if (session == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.SetResponseStatusCode(StatusCode.Created201);
    }

    [Controller.Get("tests")]
    public async Task TryGet(Emerald.AppContext context)
    {
        var rows = await Repository.GetAllAsync(context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJsonAsync(rows);
    }
}
