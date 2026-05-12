using Elasticsearch.Net;

namespace ApiLumina.Area.Identity.DTO_s.Login;

public class LoginResponse
{
    public string  Token { get; set; }

    public DateTime  Exspiration { get; set; }

    public Guid UserId { get; set; }

    public string UserEmail { get; set; }

    public string Username { get; set; }

    public string? UserRole { get; set; }
}
