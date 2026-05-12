namespace ApiLumina.Area.Identity.DTO_s;

public class UserOutputDto
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public string Addres { get; set; }

    public int  CountUsers { get; set; }
}
