namespace ApiLumina.Area.Identity.DTO_s;

public class UserPermissionsUpdateDto
{
    public  Guid UserId { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
}
