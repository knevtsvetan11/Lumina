namespace ApiLumina.Area.Identity.DTO_s;

public record class LoginRequest
{
    public string Email { get; init; }

    public string Password { get; init; }
}
