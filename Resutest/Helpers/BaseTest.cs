using System;
using Resunet.BL.Auth;
using Resunet.DAL;
using Microsoft.AspNetCore.Http;
namespace Resutest.Helpers
{
  public class BaseTest
  {
    protected IAuthDAL authDal = new AuthDAL();
    protected IEncrypt encrypt = new Encrypt();
    protected IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
    protected IAuth auth;
    protected IDbSessionDAL dbSessionDAL = new DbSessionDAL();
    protected IDbSession dbSession;
    public BaseTest()
    {
      dbSession = new DbSession(dbSessionDAL, httpContextAccessor);
      auth = new Auth(authDal, encrypt, httpContextAccessor, dbSession);

    }
  }
}