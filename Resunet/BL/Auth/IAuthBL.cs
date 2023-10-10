using System;
using System.ComponentModel.DataAnnotations;
using Resunet.DAL.Models;
namespace Resunet.BL.Auth
{
  public interface IAuthBL
  {

    Task<int> CreateUser(UserModel user);
    Task<int> Authentificate(string email, string password, bool rememberMe);

    Task<ValidationResult?> ValidateEmail(string email);
  }
}



