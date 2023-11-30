namespace Resutest;

using System.Transactions;
using Helpers;
using Resunet.BL;

public class RegisterTest : Helpers.BaseTest
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public async Task BaseRegistrationTest()
  {
    using (TransactionScope scope = Helper.CreateTransactionScope())
    {
      string email = Guid.NewGuid().ToString() + "@test.com";

      // validate: should not be in the DB
      auth.ValidateEmail(email).GetAwaiter().GetResult();

      // create user
      int userId = await auth.CreateUser(
        new Resunet.DAL.Models.UserModel()
        {
          Email = email,
          Password = "qwer12345"
        });
      Assert.That(userId, Is.GreaterThan(0));

      var userDalResult = await authDal.GetUser(userId);
      Assert.Multiple(() =>
      {
        Assert.That(userDalResult.Email, Is.EqualTo(email));
        Assert.That(userDalResult.Salt, Is.Not.Null);
      });
      var userByEmailDalResult = await authDal.GetUser(email);
      Assert.That(userByEmailDalResult.Email, Is.EqualTo(email));


      // validate: should be in the DB
      Assert.Throws<DublicateEmailException>(delegate { auth.ValidateEmail(email).GetAwaiter().GetResult(); });

      string encPassword = encrypt.HashPassword("qwer12345", userByEmailDalResult.Salt);
      Assert.That(encPassword, Is.EqualTo(userByEmailDalResult.Password));
    }
  }
}



