using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Security.Principal;
using WeavyChat.Entities;
using WeavyChat.Middleware;
using static WeavyChat.Entities.Conversation;

namespace WeavyChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly HttpContext _ctx;
        private readonly IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        private readonly syncConversations _syncConversation = new syncConversations();

        private static HttpClient _httpClient = new HttpClient();
        public ConversationsController(IConfiguration configuration, IHttpContextAccessor ctxa, ILogger<UsersController> logger)
        {
            _config = configuration;
            _ctx = ctxa.HttpContext ?? throw new NullReferenceException("HTTPContext should not be null");            
            _logger = logger;
            _syncConversation = new syncConversations(_config, _ctx);
        }

        //POST /api/conversations
        [HttpPost("~/api/CreateConversation")]
        public async Task<OutputCreateConversation> CreateConversationAsync(string currentUserUid, InputCreateConversation inputCreateConversation)
        {
            return await _syncConversation.CreateConversationAsync(currentUserUid, inputCreateConversation);
        }
    }
}
