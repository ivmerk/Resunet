using System;
namespace Resunet.BL.Auth
{
  public interface ICurrentUser
  {
    Task<bool> IsLoggedIn();
  }
}