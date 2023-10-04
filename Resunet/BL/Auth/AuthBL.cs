using System;
using Resunet.DAL.Models;
using Resunet.DAL;
namespace Resunet.BL.Auth
{
  public class AuthBL : IAuthBL
  {
    private readonly IAuthDAL authDAL;
    private readonly IEncrypt encrypt;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuthBL(IAuthDAL authDAL, IEncrypt encrypt, IHttpContextAccessor httpContextAccessor)
    {
      this.authDAL = authDAL;
      this.encrypt = encrypt;
      this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<int> CreateUser(UserModel user)
    {
      user.Salt = Guid.NewGuid().ToString();
      user.Password = encrypt.HashPassword(user.Password, user.Salt);
      int id = await authDAL.CreateUser(user);
      Login(id);
      return id;
    }

    public void Login(int id)
    {
      httpContextAccessor.HttpContext?.Session.SetInt32(AuthConstants.AUTH_SESSION_PARAM_NAME, id);
    }
    public async Task<int> Authentificate(string email, string password, bool rememberMe)
    {
      var user = await authDAL.GetUser(email);
      if (user.Password == encrypt.HashPassword(password, user.Salt))
      {
        Login(user.UserId ?? 0);
        return user.UserId ?? 0;
      }
      return 0;
    }
  }
}

