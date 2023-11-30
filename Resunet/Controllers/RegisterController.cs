using System;
using Resunet.BL.Auth;
using Microsoft.AspNetCore.Mvc;
using Resunet.ViewModels;
using Resunet.ViewMapper;
using Resunet.BL;
namespace Resunet.Controllers
{
  public class RegisterController : Controller
  {
    private readonly IAuth authBL;
    public RegisterController(IAuth authBL)
    {
      this.authBL = authBL;
    }

    [HttpGet]
    [Route("/register")]
    public IActionResult Index()
    {
      return View("Index", new RegisterViewModel());
    }
    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> IndexSave(RegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          await authBL.CreateUser(AuthMapper.MapRegistrationViewModelToUserModel(model));
          return Redirect("/");
        }
        catch (DublicateEmailException)
        {
          ModelState.TryAddModelError("Email", "Email уже существует");
        }
      }
      return View("Index", model);
    }
  }
}

