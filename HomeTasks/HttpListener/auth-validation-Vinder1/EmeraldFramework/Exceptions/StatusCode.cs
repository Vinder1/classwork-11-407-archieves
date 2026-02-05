namespace Emerald.Exceptions;

public enum StatusCode
{
    Unknown,
    OK = 200,
    Created201 = 201,
    Accepted202 = 202,

    BadRequest400 = 400,
    Unauthorized401 = 401,
    Forbidden403 = 403,
    NotFound404 = 404,
    IAmATeapot418 = 418,

    InternalServerError500 = 500,
}
