using HolidayApp.Dto;
using Emerald;
using Emerald.Controller;
using HolidayApp.Repositories;
using Emerald.Exceptions;
using EmeraldFramework.Controller;

namespace HolidayApp.Controllers;

[Controller]
public class PresentsController(PresentRepository presentRepository, UserRepository userRepository)
{
    private PresentRepository PresentRepository => presentRepository;
    private UserRepository UserRepository => userRepository;

    [Controller.Get("presents")]
    [Authorized]
    public async Task GetPresentsPage(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "presents");
    }

    [Controller.Get("api/presents/canCreate")]
    [Authorized]
    public async Task CanCreatePresent(Emerald.AppContext context)
    {
        var can = await PresentRepository.CanMakePresent(context.AuthInfo!.Id, context.CancellationToken);
        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { value = can });
    }

    [Controller.Get("api/presents")]
    [Authorized]
    public async Task GetListOfReceivedPresents(Emerald.AppContext context)
    {
        var holidayRows = await PresentRepository.GetByReceiverId(
            toId: context.AuthInfo!.Id,
        context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(holidayRows);
    }

    [Controller.Get("presents/new")]
    [Authorized]
    public async Task GetNewPresentPage(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "createPresent");
    }

    [Controller.Post("api/presents/new")]
    [Authorized]
    [RequireModel<PresentDto>]
    public async Task CreateNewPresent(Emerald.AppContext context)
    {
        var presentDto = (context.Model as PresentDto)!;

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
            id: context.AuthInfo!.Id,
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
