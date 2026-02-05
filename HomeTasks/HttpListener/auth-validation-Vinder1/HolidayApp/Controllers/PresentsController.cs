using HolidayApp.Dto;
using Emerald;
using Emerald.Controller;
using HolidayApp.Repositories;
using Emerald.Exceptions;
using HolidayApp.Services;

namespace HolidayApp.Controllers;

[Controller]
public class PresentsController(PresentRepository presentRepository, UserRepository userRepository, JwtAuth jwtAuth)
{
    private PresentRepository PresentRepository => presentRepository;
    private UserRepository UserRepository => userRepository;
    private JwtAuth JwtAuth => jwtAuth;

    [Controller.Get("presents")]
    public async Task GetPresentsPage(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("presents.html");
    }

    [Controller.Get("api/presents/canCreate")]
    public async Task CanCreatePresent(Emerald.AppContext context)
    {
        var (isSuccess, id) = JwtAuth.ValidateJwtToken(context.GetTokenFromHeader());
        if (!isSuccess || id == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var can = await PresentRepository.CanMakePresent(id.Value, context.CancellationToken);
        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { value = can });
    }

    [Controller.Get("api/presents/{str}")]
    public async Task GetListOfReceivedPresents(Emerald.AppContext context, string login)
    {
        var user = await UserRepository.GetByLoginAsync(
            login: login,
        context.CancellationToken);

        if (user == null)
        {
            context.Response.Redirect("/signin");
            return;
        }

        var holidayRows = await PresentRepository.GetByReceiverId(
            toId: user.Id,
        context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(holidayRows);
    }

    [Controller.Get("presents/new")]
    public async Task GetNewPresentPage(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("createPresent.html");
    }

    [Controller.Post("api/presents/new")]
    public async Task CreateNewPresent(Emerald.AppContext context)
    {
        var presentDto = await context.ReadJson<PresentDto>(context.CancellationToken);

        if (presentDto == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var (isSuccess, id) = JwtAuth.ValidateJwtToken(context.GetTokenFromHeader());
        if (!isSuccess || id == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        if (id == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            await context.WriteJson(new { redirect = "/signin" });
            return;
        }

        var receiver = await UserRepository.GetByLoginAsync(
            login: presentDto.ReceiverLogin,
        context.CancellationToken);

        if (receiver == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        // Just check that it exists
        var sender = await UserRepository.GetByIdAsync(
            id: id.Value,
        context.CancellationToken);

        if (sender == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var id2 = await PresentRepository.CreatePresentAndTransferMoneyAsync(
            fromId: sender.Id,
            toId: receiver.Id,
            hid: presentDto.HolidayId,
            context.CancellationToken,
            inside: presentDto.Inside ?? "-"
        );

        if (id2 == -1)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { redirect = "/presents" });
    }
}
