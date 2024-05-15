namespace MangaApp.DTO;

public class UserMangaDto
{
    public Guid UserId { get; set; }
    public Guid MangaId { get; set; }
    public string Slug { get; set; }
    public string MangaName { get; set; }
    public string MangaImage { get; set; }
}
