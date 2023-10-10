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
    protected IAuthBL authBL;
    public BaseTest()
    {
      authBL = new AuthBL(authDal, encrypt, httpContextAccessor);

    }
  }
}