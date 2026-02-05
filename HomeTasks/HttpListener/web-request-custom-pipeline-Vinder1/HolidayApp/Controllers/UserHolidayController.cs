using HolidayApp.Dto;
using Emerald;
using Emerald.Controller;
using HolidayApp.Repositories;
using Emerald.Exceptions;
using HolidayApp.Services;
using EmeraldFramework.Controller;

namespace HolidayApp.Controllers;

[Controller]
public class UserHolidayController(HolidayRepository repository, UserRepository userRepository)
{
    private HolidayRepository HolidayRepository => repository;
    private UserRepository UserRepository => userRepository;

    [Controller.Get("holidays")]
    [Authorized]
    public async Task GetHolidaysPage(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "holidays");
    }

    [Controller.Get("api/userHolidays/byCreator/{int}")]
    [Authorized]
    public async Task GetHolidaysList(Emerald.AppContext context, int creatorId)
    {
        var user = await UserRepository.GetByIdAsync(
            id: creatorId,
        context.CancellationToken);

        if (user == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var holidayRows = await HolidayRepository.GetByCreatorId(
            creatorId: user.Id,
        context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(holidayRows);
    }

    [Controller.Get("holidays/new")]
    [Authorized]
    public async Task GetNewHolidayPage(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "createHoliday");
    }

    [Controller.Post("api/userHolidays/new")]
    [RequireModel<HolidayDto>]
    [Authorized]
    public async Task CreateNewHoliday(Emerald.AppContext context)
    {
        var holidayDto = (context.Model as HolidayDto)!;

        var id2 = await HolidayRepository.CreateHolidayAsync(
            creator: context.AuthInfo!.Id,
            name: holidayDto.Name,
            celebDay: holidayDto.Date,
        context.CancellationToken);

        if (id2 == -1)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { redirect = "/holidays" });
    }

    [Controller.Get("holidays/{int}")]
    [Authorized]
    public async Task GetSpecificHolidayPage(Emerald.AppContext context, int id)
    {
        await View.RenderHtmlFile(context, "editHoliday");
    }

    [Controller.Get("api/userHolidays/byId/{int}")]
    public async Task GetSpecificHoliday(Emerald.AppContext context, int id)
    {
        var holidayRow = await HolidayRepository.GetById(
            id: id,
        context.CancellationToken);

        if (holidayRow == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            context.Response.Redirect("/holidays");
            return;
        }

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(holidayRow);
    }

    [Controller.Put("api/userHolidays/edit")]
    [Authorized]
    [RequireModel<UpdateHolidayDto>]
    public async Task UpdateHoliday(Emerald.AppContext context)
    {
        var holidayDto = (context.Model as UpdateHolidayDto)!;

        var id = await HolidayRepository.ForceUpdateHolidayAsync(
            id: holidayDto.Id,
            name: holidayDto.Name,
            celebDay: holidayDto.Date,
        context.CancellationToken);

        if (id == -1)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { redirect = "/holidays" });
    }

    [Controller.Delete("api/userHolidays/delete/{int}")]
    [Authorized]
    public async Task DeleteHoliday(Emerald.AppContext context, int id)
    {
        await HolidayRepository.DeleteHolidayAsync(
            id: id,
        context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { redirect = "/holidays" });
    }
}
