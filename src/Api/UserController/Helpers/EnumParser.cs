namespace Api.UserController;

public class EnumParser
{
    public T Parse<T>(string rawEnum) where T : struct
    {
        return Enum.Parse<T>(rawEnum, true);
    }
}