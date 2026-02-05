using HolidayHttpListener.Dto;
using HolidayHttpListener.Framework;
using HolidayHttpListener.Framework.Controller;
using HolidayHttpListener.Repositories;

namespace HolidayHttpListener.Controllers;

[Controller]
public class PresentsController
{
    private PresentRepository? repository;
    private UserRepository? userRepository;

    [Controller.Get("presents")]
    public async Task GetPresentsPage(Framework.AppContext context)
    {
        await context.SendFile("presents.html");
    }

    [Controller.Get("api/presents/canCreate")]
    public async Task CanCreatePresent(Framework.AppContext context)
    {
        repository ??= new PresentRepository(context.DBConnectionProvider.GetConnectionString());

        var login = context.Request.QueryString["login"];
        if (login == null)
        {
            await context.WriteJson(new { ok = false });
            return;
        }
        var can = await repository.CanMakePresent(login, context.CancellationToken);
        await context.WriteJson(new { ok = true, value = can });
    }

    [Controller.Get("api/presents/{str}")]
    public async Task GetListOfReceivedPresents(Framework.AppContext context, string login)
    {
        repository ??= new PresentRepository(context.DBConnectionProvider.GetConnectionString());
        userRepository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var user = await userRepository.GetByLoginAsync(
            login: login,
        context.CancellationToken);

        if (user == null)
        {
            context.Response.Redirect("/signin");
            return;
        }

        var holidayRows = await repository.GetByReceiverId(
            toId: user.Id,
        context.CancellationToken);

        await context.WriteJson(holidayRows);
    }

    [Controller.Get("presents/new")]
    public async Task GetNewPresentPage(Framework.AppContext context)
    {
        await context.SendFile("createPresent.html");
    }

    [Controller.Post("api/presents/new")]
    public async Task CreateNewPresent(Framework.AppContext context)
    {
        var presentDto = await context.ReadJson<PresentDto>(context.CancellationToken);

        if (presentDto == null)
        {
            return;
        }

        repository ??= new PresentRepository(context.DBConnectionProvider.GetConnectionString());
        userRepository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var receiver = await userRepository.GetByLoginAsync(
            login: presentDto.ReceiverLogin,
        context.CancellationToken);

        if (receiver == null)
        {
            await context.WriteJson(new { ok = false });
            return;
        }

        var sender = await userRepository.GetByLoginAsync(
            login: presentDto.SenderLogin,
        context.CancellationToken);

        if (sender == null)
        {
            await context.WriteJson(new { ok = false });
            return;
        }

        var id = await repository.CreatePresentAndTransferMoneyAsync(
            fromId: sender.Id,
            toId: receiver.Id,
            hid: presentDto.HolidayId,
            context.CancellationToken,
            inside: presentDto.Inside ?? "-"
        );

        if (id == -1)
        {
            await context.WriteJson(new { ok = false });
            return;
        }

        await context.WriteJson(new { ok = true, redirect = "/presents" });
    }
}
