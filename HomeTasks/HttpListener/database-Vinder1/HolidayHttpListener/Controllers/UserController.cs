using HolidayHttpListener.Dto;
using HolidayHttpListener.Framework;
using HolidayHttpListener.Framework.Controller;
using HolidayHttpListener.Repositories;

namespace HolidayHttpListener.Controllers;

[Controller]
public class UserController
{
    private UserRepository? repository;

    // [Controller.Get("register/{str}/{str}")]
    // public async Task Register(Framework.AppContext context, string login, string password)
    // {
    //     repository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

    //     var id = await repository.CreateUserAsync(login, password, username, context.CancellationToken);
    //     await context.WritePlainText($"Успешно зарегистрирован новый юзер с id={id}");
    // }

    [Controller.Get("signup")]
    public async Task SignUp(Framework.AppContext context)
    {
        await context.SendFile("signup.html");
    }

    [Controller.Post("signup")]
    public async Task SignUpPost(Framework.AppContext context)
    {
        var signupInfo = await context.ReadJson<SignUpDto>(context.CancellationToken);

        if (signupInfo == null)
        {
            return;
        }

        repository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());
        var id = await repository.CreateUserAsync(
            login: signupInfo.Login,
            password: signupInfo.Password,
            username: signupInfo.Username,
        context.CancellationToken);

        if (id == -1)
        {
            await context.WriteJson(new { ok = false });
            return;
        }

        await context.WriteJson(new { ok = true, redirect = "/home" });

        //context.Response.Redirect("home");
    }

    [Controller.Get("signin")]
    public async Task SignIn(Framework.AppContext context)
    {
        await context.SendFile("signin.html");
    }

    [Controller.Post("signin")]
    public async Task SignInPost(Framework.AppContext context)
    {
        var loginDto = await context.ReadJson<LoginDto>(context.CancellationToken);

        if (loginDto == null)
        {
            return;
        }

        repository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());
        var userRow = await repository.GetByLoginAsync(
            login: loginDto.Login,
        context.CancellationToken);

        if (userRow == null || !PasswordHasher.Validate($"{userRow.Hash};{userRow.Salt}", loginDto.Password))
        {
            await context.WriteJson(new UserRepository.UserRow
            {
                Id = -1
            });
            return;
        }

        await context.WriteJson(userRow);
    }

    [Controller.Get("profile")]
    public async Task GetProfile(Framework.AppContext context)
    {
        await context.SendFile("profile.html");
    }

    [Controller.Put("profile")]
    public async Task UpdateInfo(Framework.AppContext context)
    {
        var signupInfo = await context.ReadJson<SignUpDto>(context.CancellationToken);

        if (signupInfo == null)
        {
            return;
        }

        repository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var id = await repository.ForceUpdateUserAsync(
            login: signupInfo.Login,
            password: signupInfo.Password,
            username: signupInfo.Username,
        context.CancellationToken);

        await context.WriteJson(new { redirect = "/profile" });

        //context.Response.Redirect("home");
    }

    [Controller.Delete("profile/{str}")]
    public async Task DeleteUser(Framework.AppContext context, string login)
    {
        repository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        await repository.DeleteUserAsync(
            login: login,
        context.CancellationToken);

        await context.WriteJson(new { redirect = "/home" });

        //context.Response.Redirect("home");
    }

    [Controller.Get("api/user/{str}")]
    public async Task GetUserByLogin(Framework.AppContext context, string login)
    {
        repository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var user = await repository.GetByLoginAsync(login, context.CancellationToken);
        if (user == null)
        {
            await context.WriteJson(new { ok = false });
            return;
        }
        else
        {
            await context.WriteJson(new { ok = true, user });
        }
    }
    
    [Controller.Get("api/user/byId/{int}")]
    public async Task GetUserById(Framework.AppContext context, int id)
    {
        repository ??= new UserRepository(context.DBConnectionProvider.GetConnectionString());

        var user = await repository.GetByIdAsync(id, context.CancellationToken);
        if (user == null)
        {
            await context.WriteJson(new { ok = false });
            return;
        }
        else
        {
            await context.WriteJson(new { ok = true, user });
        }
    }
}
