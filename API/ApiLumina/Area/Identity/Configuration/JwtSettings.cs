namespace ApiLumina.Area.Identity.Configuration;

public class JwtSettings
{
    public string Secret { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int Expiryminutes { get; set; }


}
