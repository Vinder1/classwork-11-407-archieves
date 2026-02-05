using ControlWorkApp.Dto;
using ControlWorkApp.Repositories;
using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;

namespace ControlWorkApp.Controllers;

[Controller]
public class AnswerController(AnswerRepository repository, QuestionRepository qRepository, UserRepository userRepository)
{
    private AnswerRepository Repository => repository;
    private QuestionRepository QuestionRepository => qRepository;
    private UserRepository UserRepository => userRepository;

    [Controller.Post("tests/{int}/answers")]
    public async Task TryCreate(Emerald.AppContext context, int testId)
    {
        var createInfo = await context.ReadJsonAsync<NewAnswerDto>(context.CancellationToken);

        if (createInfo == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var question = await QuestionRepository.GetByIdAsync(createInfo.QuestionId, context.CancellationToken);

        if (question == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        createInfo.IsCorrect = question.CorrectAnswer.ToLowerInvariant()
            .Equals(createInfo.GivenAnswer.ToLowerInvariant());

        var session = await Repository.CreateAnswerAsync(
            createInfo,
        context.CancellationToken);

        if (session == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.SetResponseStatusCode(StatusCode.Created201);
    }

    [Controller.Get("tests/{int}/answers")]
    public async Task TryGet(Emerald.AppContext context, int userId)
    {
        var rows = await Repository.GetByUserAsync(userId, context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJsonAsync(rows);
    }

    [Controller.Get("results/teacher")]
    public async Task TryGetStatistics(Emerald.AppContext context)
    {
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

        var rows = await Repository.TryGetStatistics(user.Id, context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJsonAsync(rows);
    }
}
