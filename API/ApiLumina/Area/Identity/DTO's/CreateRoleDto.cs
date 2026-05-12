using System.ComponentModel.DataAnnotations;

namespace ApiLumina.Area.Identity.DTO_s;

public class CreateRoleDto
{
    [MaxLength(length:20,ErrorMessageResourceName ="Role's name can't be with more than 20 letters.")]
    [MinLength(length:4, ErrorMessageResourceName = "Role's name must be with min 4 letters.")]
    [Required(ErrorMessageResourceName ="Role name is required.")]
    public string RoleName { get; set; }
}
