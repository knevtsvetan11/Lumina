namespace ApiLumina.Area.Identity.DTO_s;

public class UserSearchDto
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string? SearchData { get; set; }
    public string? FilterColumn { get; set; }
}
