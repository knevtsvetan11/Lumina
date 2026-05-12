namespace ApiLumina.Area.Identity.DTO_s;

public class UserPermissionsDto
{
    public string PermissionValue { get; set; }

    public string DisplayName { get; set; }

    public bool IsGranted { get; set; }

    public bool IsInheritedFromRole { get; set; }
}
