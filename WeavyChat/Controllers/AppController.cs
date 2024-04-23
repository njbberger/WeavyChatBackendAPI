using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using WeavyChat.Entities;
using WeavyChat.Utils;
using System.Text.Json;
using WeavyChat.Middleware;
using static WeavyChat.Entities.WeavyApp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeavyChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly HttpContext _ctx;
        private readonly ILogger<AppController> _logger;
        private readonly IConfiguration _config;
        private static HttpClient _httpClient = new HttpClient();
        private readonly syncApp _syncApp = new syncApp();

        public AppController(IConfiguration configuration, IHttpContextAccessor ctxa, ILogger<AppController> logger)
        {
            _config = configuration;
            _ctx = ctxa.HttpContext ?? throw new NullReferenceException("HTTPContext should not be null");
            _logger = logger;
            _syncApp = new syncApp(_config, _ctx);
        }

        // GET: UsersController/apps/{app}
        [HttpGet("~/api/GetApp")]
        public async Task<WeavyApp.OutputGetApp> GetAppAsync([FromQuery] string UserName, bool trashed = true)
        {
            string uid = UserName;

            //return await _syncApp.GetApp(uid, trashed, _config, _ctx);
            return await _syncApp.GetAppAsync(uid, trashed);
        }

        // GET /api/apps
        [HttpGet("~/api/ListApps")]
        public async Task<OutputListApp> ListAppsAsync([FromQuery] InputListApp inputListApp)
        {
            return await _syncApp.ListAppsAsync(inputListApp);
        }

        // PUT /api/apps/{app}/members/{user}
        [HttpPut("~/api/AddMember")]
        public async Task<string> AddMemberAsync([FromQuery] string AppUid, string UserNameUid, WeavyApp.Access MemberAccess)
        {
            //return await _syncApp.GetApp(uid, trashed, _config, _ctx);
            return await _syncApp.AddMemberAsync(AppUid, UserNameUid, MemberAccess);
        }

        // GET UsersController/apps/{app}/members
        [HttpGet("~/api/ListMembers")]
        public async Task<OutputListMembers> ListMembersAsync([FromQuery] InputListMembers inputListMembers)
        {
            return await _syncApp.ListMembersAsync(inputListMembers);
        }

        // POST api/<AppController>
        [HttpPost("~/api/CreateApp")]
        public async Task<IActionResult> CreateApp([FromBody] string uid)
        {
            //curl https://{WEAVY-SERVER}/api/apps
            //-H "Authorization: Bearer {ACCESS-TOKEN | API-KEY}"
            //--json "{ 'type': 'chat', 'uid': 'acme-chat', 'access': 'write' }"
            
            return BadRequest();

        }


    }
}
