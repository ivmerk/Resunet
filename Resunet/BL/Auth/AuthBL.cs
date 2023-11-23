using System;
using Resunet.DAL.Models;
using Resunet.DAL;
using System.ComponentModel.DataAnnotations;
namespace Resunet.BL.Auth
{
  public class AuthBL : IAuthBL
  {
    private readonly IAuthDAL authDAL;
    private readonly IEncrypt encrypt;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IDbSession dbSession;

    public AuthBL(IAuthDAL authDAL, IEncrypt encrypt, IHttpContextAccessor httpContextAccessor, IDbSession dbSession)
    {
      this.authDAL = authDAL;
      this.encrypt = encrypt;
      this.httpContextAccessor = httpContextAccessor;
      this.dbSession = dbSession;
    }
    public async Task<int> CreateUser(UserModel user)
    {
      user.Salt = Guid.NewGuid().ToString();
      user.Password = encrypt.HashPassword(user.Password, user.Salt);
      int id = await authDAL.CreateUser(user);
      await Login(id);
      return id;
    }

    public async Task Login(int id)
    {
      await dbSession.SetUserId(id);
    }
    public async Task<int> Authentificate(string email, string password, bool rememberMe)
    {
      var user = await authDAL.GetUser(email);

      if (user.UserId != null && user.Password == encrypt.HashPassword(password, user.Salt))
      {
        await Login(user.UserId.Value);
        return user.UserId.Value;
      }
      throw new AuthorizationException();
    }
    public async Task<ValidationResult?> ValidateEmail(string email)
    {
      var user = await authDAL.GetUser(email);
      if (user.UserId != null)
        return new ValidationResult("Email уже существует");
      return null;
    }
  }
}

