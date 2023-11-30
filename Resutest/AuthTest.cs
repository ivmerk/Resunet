using System;
using Resutest.Helpers;
using System.Transactions;
using Resunet.BL;

namespace Resutest
{
  public class AuthTest : Helpers.BaseTest
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task AuthRegistrationTest()
    {
      using (TransactionScope scope = Helper.CreateTransactionScope())
      {
        string email = Guid.NewGuid().ToString() + "@test.com";

        // create user
        int userId = await auth.CreateUser(
            new Resunet.DAL.Models.UserModel()
            {
              Email = email,
              Password = "qwer1234"
            });

        Assert.Throws<AuthorizationException>(() => auth.Authentificate("sefrew", "111", false).GetAwaiter().GetResult());

        Assert.Throws<AuthorizationException>(delegate { auth.Authentificate(email, "111", false).GetAwaiter().GetResult(); });

        Assert.Throws<AuthorizationException>(delegate { auth.Authentificate("werewr", "qwer1234", false).GetAwaiter().GetResult(); });

        await auth.Authentificate(email, "qwer1234", false);
      }
    }
  }
}