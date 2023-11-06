using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Resunet.ViewModels;

namespace Resunet.Controllers
{
  public class ProfileController : Controller
  {
    [HttpGet]
    [Route("/profile")]
    public IActionResult Index()
    {
      return View(new ProfileViewModel());
    }
    [HttpPost]
    [Route("/profile")]
    public async Task<IActionResult> IndexSave()
    {
      var imageData = Request.Form.Files[0];
      if (imageData != null)
      {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(imageData.FileName);
        byte[] hashBytes = MD5.HashData(inputBytes);

        string hash = Convert.ToHexString(hashBytes);

        var dir = "./wwwroot/images/" + hash[..2] + "/" + hash[..4];

        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);

        string filename = dir + "/" + imageData.FileName;
        using var stream = System.IO.File.Create(filename);
        await imageData.CopyToAsync(stream);

      }
      return View("Index", new ProfileViewModel());
    }
  }
}