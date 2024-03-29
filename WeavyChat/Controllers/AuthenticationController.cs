using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using WeavyChat.Entities;
using System.Numerics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
//using Newtonsoft.Json;
using System.Xml.Linq;
//using IdentityModel.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeavyChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthenticationController(IConfiguration configuration, IHttpContextAccessor ctxa)
        {
            _config = configuration;
            _ctx = ctxa.HttpContext ?? throw new NullReferenceException("HTTPContext should not be null");
        }
        
        //// GET: api/<AuthenticationController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<AuthenticationController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<AuthenticationController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<AuthenticationController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AuthenticationController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        private static HttpClient _httpClient = new HttpClient();

        private readonly HttpContext _ctx;
        private DateTime _time = DateTime.UtcNow.AddHours(4);
        private Tokens _tokens = new Tokens();

        [HttpGet("~/api/token")]
        //[HttpGet(Name = "GetTokenForUser")]
        public async Task<IActionResult> GetToken([FromQuery] User currentUser, bool refresh = false)
        {
            //string? uid = HttpContext.User.Identity != null ? HttpContext.User.Identity.Name : string.Empty;
            //ExpirationStatus aux_ExpirationStatus = ExpirationStatus.None;

            //_ctx.User = new GenericPrincipal(new ClaimsIdentity(currentUser.EmailAddress), new[] { "user" });
            ////_ctx.Session.Set("key", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("user"))); //SetKSuid(SessionId, currentUser.Id!);
            ////_ctx.Session.SetString("expiresAt", _time.ToString("s") + _time.ToString("zzz"));

            //// get uid for the authenticated user
            //if (User.Identity != null && User.Identity.IsAuthenticated)
            //{
            //    uid = User.Identity.Name;
            //}

            string uid = currentUser.UserName;
            UserAccessToken existingToken = new UserAccessToken();
            existingToken.ExpirationStatus = ExpirationStatus.None;

            //if (!refresh && _tokens.TryGetValueAsync(uid, out UserAccessToken existingToken))
            if (!refresh )
            {                
                // check local token store for existing access_token
                var task = await _tokens.TryGetValueAsync(uid);

                if (task != null && task.Item1)
                {
                    existingToken = task.Item2;

                    //verifico vigencia del token recuperado
                    if (existingToken.TimestampFinValidezToken > DateTime.Now)
                    {
                        //token ACTIVO                        
                        existingToken.ExpirationStatus = ExpirationStatus.Active;

                        Console.WriteLine($"Uid: {existingToken.Uid}");
                        Console.WriteLine($"Token: {existingToken.Access_token}");
                        Console.WriteLine($"Token Activo - TimestampFinValidezToken: {existingToken.TimestampFinValidezToken}");

                        //Return current token that is active
                        //return Content(existingToken.Token);
                        AccessToken accessToken = new AccessToken();
                        accessToken.access_token = existingToken.Access_token;
                        string jsonAccessToken = JsonSerializer.Serialize<AccessToken>(accessToken);
                        return Content(jsonAccessToken);                        
                    }
                    else
                    {
                        //token EXPIRADO, se debe solicitar nuevo token por lo tanto CONTNUA sin hacer return
                        existingToken.ExpirationStatus = ExpirationStatus.Inactive;

                        Console.WriteLine($"Uid: {existingToken.Uid}");
                        Console.WriteLine($"Token: {existingToken.Access_token}");
                        Console.WriteLine($"Token Expirado - TimestampFinValidezToken: {existingToken.TimestampFinValidezToken}");
                    }
                }                                               
            }            

            // get weavy url and api key from config 
            //var weavyUrl = _config["WEAVY-SERVER"];
            //var apiKey = _config["WEAVY-API-KEY"];
            var weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            var apiKey = _config["MySettings:WeavyServerAPIkey"];

            // request access_token from Weavy
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var response = await _httpClient.PostAsync($"{weavyUrl}/api/users/{uid}/tokens", null);

            if (response.IsSuccessStatusCode)
            {
                // read access_token
                var newTokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                if (newTokenResponse == null)
                {
                    throw new NullReferenceException("HttpResponseMessage should not be null");
                }
                else
                {
                    UserAccessToken newUserAccessToken = new UserAccessToken();

                    newUserAccessToken.Uid = uid;
                    newUserAccessToken.Access_token = newTokenResponse.access_token;                            
                    //se setea el estado del token recuperado (NONE o INACTIVO) para evaluar si hay que insertar o actualizar el token
                    newUserAccessToken.ExpirationStatus = existingToken.ExpirationStatus;                                                    

                    // store and return access_token
                    //_tokens[uid] = accessToken;
                    await _tokens.TrySetValueAsync(newUserAccessToken);

                    //Return current token that is active
                    //return Content(newUserAccessToken.Access_token);
                    AccessToken accessToken = new AccessToken();
                    accessToken.access_token = newUserAccessToken.Access_token;
                    string jsonAccessToken = JsonSerializer.Serialize<AccessToken>(accessToken);
                    return Content(jsonAccessToken);                    
                }
            }                                      
            return BadRequest();
        }


    }
}
