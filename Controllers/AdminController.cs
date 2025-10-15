using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPRMAspNetCoreMVC.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public IActionResult ManageUsers()
        {
            return RedirectToAction("Index", "Users");
        }
    }
    
}
