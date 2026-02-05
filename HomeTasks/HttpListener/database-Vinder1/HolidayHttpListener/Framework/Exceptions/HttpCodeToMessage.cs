using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolidayHttpListener.Framework.Exceptions;

public static class HttpCodeToMessage
{

    // I m too lazy to make it expandable, I probably wont touch it anyway
    public static string GetFrom(StatusCode code)
    {
        return code switch
        {
            StatusCode.NotFound404 => "Not found",
            StatusCode.Forbidden403 => "Forbidden access",
            StatusCode.BadRequest400 => "Bad request",
            StatusCode.IAmATeapot418 => "I am a teapot, la-la-la",

            StatusCode.InternalServerError500 => "Internal server error",
    
            _ => "Unknown"
        };
    }

    public static int ToInt(this StatusCode code)
        => (int)code;
}
