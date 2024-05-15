using MySqlConnector;

namespace MangaApp.Model.Domain;

public class User
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public string? Avatar { get; set; }
    public string? faceAuthenticationImage { get; set; }
    public  long Point { get; set; }
    public byte[]? HashPassword { get; set; }
    public byte[]? SaltPassword { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<UserManga> UserMangas { get; set; }
}
