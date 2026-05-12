namespace ApiLumina.Area.Identity.DTO_s;

public class CreateUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
