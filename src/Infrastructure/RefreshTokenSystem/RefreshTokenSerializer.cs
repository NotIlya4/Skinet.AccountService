using Newtonsoft.Json.Linq;

namespace Infrastructure.RefreshTokenSystem;

public class RefreshTokenSerializer
{
    private JObject SerializeTimestampRefreshToken(TimestampRefreshToken refreshToken)
    {
        JObject obj = new();
        obj[nameof(TimestampRefreshToken.Issued)] = refreshToken.Issued;
        obj[nameof(TimestampRefreshToken.RefreshToken)] = refreshToken.RefreshToken.ToString();
        return obj;
    }

    private TimestampRefreshToken DeserializeTimestampRefreshToken(JToken rawRefreshToken)
    {
        return new TimestampRefreshToken()
        {
            Issued = rawRefreshToken[nameof(TimestampRefreshToken.Issued)]!.ToObject<DateTime>(),
            RefreshToken = new Guid(rawRefreshToken[nameof(TimestampRefreshToken.RefreshToken)]!.Value<string>()!)
        };
    }

    public string SerializeCollection(List<TimestampRefreshToken> refreshTokens)
    {
        List<JObject> jobjects = refreshTokens.Select(SerializeTimestampRefreshToken).ToList();
        JArray array = new JArray(jobjects);

        return array.ToString();
    }

    public string SerializeCollection(ValidRefreshTokenCollection refreshTokens)
    {
        return SerializeCollection(refreshTokens.ToList());
    }

    public ValidRefreshTokenCollection DeserializeCollection(string rawTokensCollection, TimeSpan expireIn)
    {
        JArray array = JArray.Parse(rawTokensCollection);
        List<TimestampRefreshToken> refreshTokensList =
            array.Select(DeserializeTimestampRefreshToken).ToList();
        return new ValidRefreshTokenCollection(refreshTokensList, expireIn);
    }
}