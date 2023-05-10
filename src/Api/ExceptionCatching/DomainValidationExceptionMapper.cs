using Domain.Exceptions;
using ExceptionCatcherMiddleware.Mappers.CreatingCustomMappers;

namespace Api.ExceptionCatching;

public class DomainValidationExceptionMapper : IExceptionMapper<DomainValidationException>
{
    public BadResponse Map(DomainValidationException exception)
    {
        return new BadResponse()
        {
            StatusCode = 400,
            ResponseDto = new
            {
                Title = "Domain validation exception",
                Detail = exception.Message
            }
        };
    }
}