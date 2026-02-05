using HolidayApp.Dto;
using Emerald;
using Emerald.Controller;
using HolidayApp.Repositories;
using Emerald.Exceptions;
using HolidayApp.Services;

namespace HolidayApp.Controllers;

[Controller]
public class UserHolidayController(HolidayRepository repository, UserRepository userRepository, JwtAuth jwtAuth)
{
    private HolidayRepository HolidayRepository => repository;
    private UserRepository UserRepository => userRepository;
    private JwtAuth JwtAuth => jwtAuth;

    [Controller.Get("holidays")]
    public async Task GetHolidaysPage(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("holidays.html");
    }

    [Controller.Get("api/userHolidays/byCreator/{int}")]
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
    public async Task GetNewHolidayPage(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("createHoliday.html");
    }

    [Controller.Post("api/userHolidays/new")]
    public async Task CreateNewHoliday(Emerald.AppContext context)
    {
        var holidayDto = await context.ReadJson<HolidayDto>(context.CancellationToken);

        if (holidayDto == null)
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

        var id2 = await HolidayRepository.CreateHolidayAsync(
            creator: id.Value,
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
    public async Task GetSpecificHolidayPage(Emerald.AppContext context, int id)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("editHoliday.html");
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
    public async Task UpdateHoliday(Emerald.AppContext context)
    {
        // TODO: проверка что его можно редактировать 

        var holidayDto = await context.ReadJson<UpdateHolidayDto>(context.CancellationToken);

        if (holidayDto == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

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
    public async Task DeleteHoliday(Emerald.AppContext context, int id)
    {
        // TODO: проверка что его можно удалить 

        await HolidayRepository.DeleteHolidayAsync(
            id: id,
        context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { redirect = "/holidays" });
    }
}
