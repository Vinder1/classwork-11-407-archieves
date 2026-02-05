using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;

namespace HolidayApp.Controllers;

[Controller]
public class ExampleController
{
    [Http(Path: "example",
        Method: Emerald.HttpMethod.GET)]
    public async Task Const(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.WritePlainText("Пример константного");
    }

    [Controller.Get("example/new/")]
    public async Task NewAttributeFormat(Emerald.AppContext context)
    {
        await context.WritePlainText("Пример использования более читабельного аттрибута вместо HttpAttribute");
    }

    [Http(Path: "example/const",
        Method: Emerald.HttpMethod.GET)]
    public async Task ConstDouble(Emerald.AppContext context)
    {
        await context.WritePlainText("Пример константного двууровневого");
    }

    [Http(Path: "example/{int}/{str}",
        Method: Emerald.HttpMethod.GET)]
    public async Task IntAnsString(Emerald.AppContext context, int i, string s)
    {
        await context.WritePlainText($"Число: {i} Строка: {s}");
    }

    [Http(Path: "example",
        Method: Emerald.HttpMethod.POST)]
    public async Task Post(Emerald.AppContext context)
    {
        await context.WritePlainText("Еще и другие методы работают");
    }

    // [Http(Path: "example",
    //     Method: Framework.HttpMethod.POST)]
    // public async Task SamePost(Framework.AppContext context)
    // {
    //     await context.WritePlainText("А вот это выкинет ошибку");
    // }
}