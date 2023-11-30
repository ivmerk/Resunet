using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Resunet.BL.Auth;
using Resunet.ViewModels;

namespace Resunet.Controllers
{
  public class LoginController : Controller
  {

    private readonly IAuth authBL;
    public LoginController(IAuth authBL)
    {
      this.authBL = authBL;
    }
    [HttpGet]
    [Route("/login")]
    public IActionResult Index()
    {
      return View("Index", new LoginViewModel());
    }
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> IndexSave(LoginViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          await authBL.Authentificate(model.Email!, model.Password!, model.RememberMe == true);
          return Redirect("/");
        }
        catch (Resunet.BL.AuthorizationException)
        {
          ModelState.AddModelError("Email", "Имя или Email неверные");
        }
      }
      return View("Index", model);

    }
  }
}