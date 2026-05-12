namespace ApiLumina.Area.Identity.DTO_sl;

public  class RegisterRequest
{
    public string FirstName { get; set; } = null!;

    public string LastName { get;set; } = null!;

    public int PhoneNumber { get; set; } 

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;


}
