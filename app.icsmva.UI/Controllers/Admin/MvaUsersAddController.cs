using Microsoft.AspNetCore.Mvc;

namespace app.icsmva.UI.Controllers.Admin
{
    public class MvaUsersAddController : Controller
    {
        public IActionResult User_Add()
        {
            return View();
        }
    }
}
