using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using WeavyChat.Entities;
using WeavyChat.Middleware;

namespace WeavyChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpContext _ctx;
        private readonly ILogger<UsersController> _logger;
        private readonly syncUser _syncUser = new syncUser();
        private readonly DateTime _time = DateTime.UtcNow.AddHours(4);

        public UsersController(IConfiguration configuration, IHttpContextAccessor ctxa, ILogger<UsersController> logger)
        {
            _config = configuration;
            _ctx = ctxa.HttpContext ?? throw new NullReferenceException("HTTPContext should not be null");
            _logger = logger;
            _syncUser = new syncUser(_config, _ctx);
        }      

        // PUT: UsersController
        [HttpPut("~/api/UpsertUserAsync")]
        //[ValidateAntiForgeryToken]
        public async Task<UserSync.Output> UpsertUserAsync([FromQuery] User currentUser)
        {
            string? uid = HttpContext.User.Identity != null ? HttpContext.User.Identity.Name : string.Empty;

            _ctx.User = new GenericPrincipal(new ClaimsIdentity(currentUser.EmailAddress), new[] { "user" });
            //_ctx.Session.Set("key", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("user"))); //SetKSuid(SessionId, currentUser.Id!);
            //_ctx.Session.SetString("expiresAt", _time.ToString("s") + _time.ToString("zzz"));

            // get uid for the authenticated user
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                uid = User.Identity.Name;
            }

            uid = currentUser.UserName;    

            return await _syncUser.UpsertUserAsync(currentUser, _config, _ctx); ;
        }

        // GET: UsersController/users/{user}
        [HttpGet("~/api/GetUserAsync")]
        public async Task<UserSync.Output> GetUserAsync([FromQuery] string UserName, bool trashed = true)
        {
            string uid = UserName;

            return await _syncUser.GetUserAsync(uid, trashed, _config, _ctx); ;
        }

        // GET: UsersController/users
        [HttpGet("~/api/ListUsersAsync")]
        public async Task<UserList.OutputUserList> ListUsersAsync([FromQuery] UserList.InputUserList userListInput)
        {
            return await _syncUser.ListUsersAsync(userListInput, _config, _ctx); ;
        }



        //// GET: UsersController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UsersController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsersController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UsersController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsersController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UsersController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
