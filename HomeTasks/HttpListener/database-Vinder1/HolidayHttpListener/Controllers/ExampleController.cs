using System.Text;
using HolidayHttpListener.Framework;
using HolidayHttpListener.Framework.Controller;

namespace HolidayHttpListener.Controllers;

[Controller]
public class ExampleController
{
    [Http(Path: "example",
        Method: Framework.HttpMethod.GET)]
    public async Task Const(Framework.AppContext context)
    {
        await context.WritePlainText("Пример константного");
    }

    [Controller.Get("example/new/")]
    public async Task NewAttributeFormat(Framework.AppContext context)
    {
        await context.WritePlainText("Пример использования более читабельного аттрибута вместо HttpAttribute");
    }

    [Http(Path: "example/const",
        Method: Framework.HttpMethod.GET)]
    public async Task ConstDouble(Framework.AppContext context)
    {
        await context.WritePlainText("Пример константного двууровневого");
    }

    [Http(Path: "example/{int}/{str}",
        Method: Framework.HttpMethod.GET)]
    public async Task IntAnsString(Framework.AppContext context, int i, string s)
    {
        await context.WritePlainText($"Число: {i} Строка: {s}");
    }

    [Http(Path: "example",
        Method: Framework.HttpMethod.POST)]
    public async Task Post(Framework.AppContext context)
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