using Microsoft.AspNetCore.Mvc;

namespace Resunet.Controllers
{
  public class ProfileController : Controller
  {
    [HttpGet]
    [Route("/profile")]
    public IActionResult Index()
    {
      return View();
    }
    [HttpPost]
    [Route("/profile")]
    public IActionResult IndexSave()
    {
      return View();
    }
  }
}