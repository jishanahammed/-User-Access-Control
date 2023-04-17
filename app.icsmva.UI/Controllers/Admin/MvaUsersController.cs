using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class MvaUsersController : Controller
    {
        private readonly ILogger<MvaUsersController> _logger;
        private readonly IUsers usersservice;
        public MvaUsersController(ILogger<MvaUsersController> logger, IUsers usersservice)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.usersservice = usersservice;
            _logger = logger;
        }
        [Authorize("Authorization")]
        public IActionResult User_View(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var result = usersservice.GetUserPagedListAsync(page, pagesize);
            return View(result);
        }
        [HttpGet]
        public IActionResult Getpage(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var result = usersservice.GetUserPagedListAsync(page, pagesize);
            return PartialView("_userpaginatedpartial", result);
        }
    }
}
