using AdsApp.DTO;
using AdsApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdsApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService ?? throw new System.ArgumentNullException(nameof(userService));
        }

        public IActionResult Index()
        {
            return View("~/Views/RegisterView.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View("~/Views/RegisterView.cshtml");
            }

            await _userService.Register(request);

            return RedirectToAction("Home");
        }
    }
}