using System;
using Resunet.DAL.Models;
using Resunet.DAL;
using System.ComponentModel.DataAnnotations;
using Resunet.BL.General;
namespace Resunet.BL.Auth
{
  public class Auth : IAuth
  {
    private readonly IAuthDAL authDAL;
    private readonly IEncrypt encrypt;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IDbSession dbSession;

    public Auth(IAuthDAL authDAL, IEncrypt encrypt, IHttpContextAccessor httpContextAccessor, IDbSession dbSession)
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
    public async Task ValidateEmail(string email)
    {
      var user = await authDAL.GetUser(email);
      if (user.UserId != null)
        throw new DublicateEmailException();
    }

    public async Task Register(UserModel user)
    {
      using (var scope = Helpers.CreateTransactionScope())
      {
        await dbSession.Lock();
        await ValidateEmail(user.Email);
        await CreateUser(user);
        scope.Complete();
      }
    }
  }
}

