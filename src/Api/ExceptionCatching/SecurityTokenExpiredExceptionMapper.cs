using ExceptionCatcherMiddleware.Mappers.CreatingCustomMappers;
using Microsoft.IdentityModel.Tokens;

namespace Api.ExceptionCatching;

public class SecurityTokenExpiredExceptionMapper : IExceptionMapper<SecurityTokenExpiredException>
{
    public BadResponse Map(SecurityTokenExpiredException exception)
    {
        return new BadResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ResponseDto = new
            {
                Title = "Jwt token expired",
                Detail = exception.Message
            }
        };
    }
}