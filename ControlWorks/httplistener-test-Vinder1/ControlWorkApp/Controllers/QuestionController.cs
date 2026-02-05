using ControlWorkApp.Dto;
using ControlWorkApp.Repositories;
using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;

namespace ControlWorkApp.Controllers;

[Controller]
public class QuestionController(QuestionRepository repository, UserRepository userRepository)
{
    private QuestionRepository Repository => repository;
    private UserRepository UserRepository => userRepository;

    [Controller.Post("tests/questions")]
    public async Task TryCreate(Emerald.AppContext context)
    {
        var signupInfo = await context.ReadJsonAsync<NewQuestionDto>(context.CancellationToken);

        if (signupInfo == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var sessionCookie = context.Request.Cookies["session"];
        if (sessionCookie == null || sessionCookie.Value == null)
        {
            context.SetResponseStatusCode(StatusCode.Unauthorized401);
            return;
        }

        var sessionId = sessionCookie.Value;

        var user = await UserRepository.GetBySessionAsync(
            sessionId,
        context.CancellationToken);

        if (user == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        if (!user.Role.Equals("teacher", StringComparison.CurrentCultureIgnoreCase))
        {
            context.SetResponseStatusCode(StatusCode.Forbidden403);
            return;
        }

        var session = await Repository.CreateQuestionAsync(
            signupInfo,
        context.CancellationToken);

        if (session == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.SetResponseStatusCode(StatusCode.Created201);
    }

    [Controller.Get("tests/{int}/questions")]
    public async Task TryGet(Emerald.AppContext context, int authorId)
    {
        var rows = await Repository.GetByTestAsync(authorId, context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJsonAsync(rows);
    }
}
