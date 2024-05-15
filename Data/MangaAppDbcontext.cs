using MangaApp.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Data;

public class MangaAppDbcontext:DbContext
{
    public MangaAppDbcontext(DbContextOptions dbContextOptions): base (dbContextOptions)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<UserManga> UserMangas { get; set; }
    public DbSet<Gacha> GachaItems { get; set; }
}