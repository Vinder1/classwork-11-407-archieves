using System.Data.Common;
using HolidayHttpListener.Dto;
using HolidayHttpListener.Framework;
using HolidayHttpListener.Framework.Controller;
using HolidayHttpListener.Repositories;

namespace HolidayHttpListener.Controllers;

[Controller]
public class UserHolidayController
{
    private HolidayRepository? repository;
    private UserRepository? userRepository;

    [Controller.Get("holidays")]
    public async Task GetHolidaysPage(Framework.AppContext context)
    {
        await context.SendFile("holidays.html");
    }

    [Controller.Get("api/userHolidays/byCreator/{str}")]
    public async Task GetHolidaysList(Framework.AppContext context, string creatorLogin)
    {
        repository ??= new HolidayRepository(context.DBConnectionProvider.GetConnectionString());
        userRepository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var user = await userRepository.GetByLoginAsync(
            login: creatorLogin,
        context.CancellationToken);

        if (user == null)
        {
            context.Response.Redirect("/signin");
            return;
        }

        var holidayRows = await repository.GetByCreatorId(
            creatorId: user.Id,
        context.CancellationToken);

        await context.WriteJson(holidayRows);
    }

    [Controller.Get("holidays/new")]
    public async Task GetNewHolidayPage(Framework.AppContext context)
    {
        await context.SendFile("createHoliday.html");
    }

    [Controller.Post("api/userHolidays/new")]
    public async Task CreateNewHoliday(Framework.AppContext context)
    {
        var holidayDto = await context.ReadJson<HolidayDto>(context.CancellationToken);

        if (holidayDto == null)
        {
            return;
        }

        repository ??= new HolidayRepository(context.DBConnectionProvider.GetConnectionString());
        userRepository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var user = await userRepository.GetByLoginAsync(
            login: holidayDto.CreatorLogin,
        context.CancellationToken);

        if (user == null)
        {
            await context.WriteJson(new { ok = false, redirect = "/signin" });
            return;
        }

        var id = await repository.CreateHolidayAsync(
            creator: user.Id,
            name: holidayDto.Name,
            celebDay: holidayDto.Date,
        context.CancellationToken);

        if (id == -1)
        {
            await context.WriteJson(new { ok = false });
            return;
        }

        await context.WriteJson(new { ok = true, redirect = "/holidays" });
    }

    [Controller.Get("holidays/{int}")]
    public async Task GetSpecificHolidayPage(Framework.AppContext context, int id)
    {
        await context.SendFile("editHoliday.html");
    }

    [Controller.Get("api/userHolidays/byId/{int}")]
    public async Task GetSpecificHoliday(Framework.AppContext context, int id)
    {
        repository ??= new HolidayRepository(context.DBConnectionProvider.GetConnectionString());

        var holidayRow = await repository.GetById(
            id: id,
        context.CancellationToken);

        if (holidayRow == null)
        {
            context.Response.Redirect("/holidays");
            return;
        }

        await context.WriteJson(holidayRow);
    }

    [Controller.Put("api/userHolidays/edit")]
    public async Task UpdateHoliday(Framework.AppContext context)
    {
        var holidayDto = await context.ReadJson<UpdateHolidayDto>(context.CancellationToken);

        if (holidayDto == null)
        {
            return;
        }

        repository ??= new HolidayRepository(context.DBConnectionProvider.GetConnectionString());
        userRepository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var id = await repository.ForceUpdateHolidayAsync(
            id: holidayDto.Id,
            name: holidayDto.Name,
            celebDay: holidayDto.Date,
        context.CancellationToken);

        if (id == -1)
        {
            await context.WriteJson(new { ok = false });
            return;
        }

        await context.WriteJson(new { ok = true, redirect = "/holidays" });
    }

    [Controller.Delete("api/userHolidays/delete/{int}")]
    public async Task DeleteHoliday(Framework.AppContext context, int id)
    {
        repository ??= new HolidayRepository(context.DBConnectionProvider.GetConnectionString());

        await repository.DeleteHolidayAsync(
            id: id,
        context.CancellationToken);

        await context.WriteJson(new { ok = true, redirect = "/holidays" });
    }
}
