using Resunet.DAL.Models;
namespace Resunet.BL.Auth
{
  public interface IAuth
  {

    Task<int> CreateUser(UserModel user);
    Task<int> Authentificate(string email, string password, bool rememberMe);

    Task ValidateEmail(string email);

    Task Register(UserModel user);
  }
}



