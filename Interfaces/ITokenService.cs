using MangaApp.Model.Domain;

namespace MangaApp.Interfaces;

public interface ITokenService
{
  
    string CreateToken(User user);
}