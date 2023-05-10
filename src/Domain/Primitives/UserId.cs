namespace Domain.Primitives;

public record UserId
{
    public Guid Value { get; }

    public UserId(string value) : this(new Guid(value))
    {
        
    }
    
    public UserId(Guid value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}