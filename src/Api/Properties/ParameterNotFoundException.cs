namespace Api.Properties;

public class ParameterNotFoundException : Exception
{
    public ParameterNotFoundException(string parameterName) : base($"{parameterName} not found in configuration")
    {
        
    }
}