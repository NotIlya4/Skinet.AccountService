namespace Infrastructure.RefreshTokenService.Models;

public record RefreshToken
{
    public Guid Value { get; }

    public RefreshToken(string value) : this(new Guid(value))
    {
        
    }
    
    public RefreshToken(Guid value)
    {
        Value = value;
    }
    
    public override string ToString()
    {
        return Value.ToString();
    }
}