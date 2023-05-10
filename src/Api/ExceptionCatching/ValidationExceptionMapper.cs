using System.ComponentModel.DataAnnotations;
using ExceptionCatcherMiddleware.Mappers.CreatingCustomMappers;

namespace Api.ExceptionCatching;

public class ValidationExceptionMapper : IExceptionMapper<ValidationException>
{
    public BadResponse Map(ValidationException exception)
    {
        return new BadResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ResponseDto = new
            {
                Title = "Validation Exception",
                Detail = exception.Message
            }
        };
    }
}