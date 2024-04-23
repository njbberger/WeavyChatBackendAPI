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
using static WeavyChat.Entities.Conversation;
using WeavyChat.Middleware;
//using IdentityModel.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeavyChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly HttpContext _ctx;
        private readonly IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        private readonly GetWeavyUserToken _getWeavyUserToken = new GetWeavyUserToken();
        private readonly Tokens _tokens = new Tokens();
        private static HttpClient _httpClient = new HttpClient();

        public AuthenticationController(IConfiguration configuration, IHttpContextAccessor ctxa, ILogger<UsersController> logger)
        {
            _config = configuration;
            _ctx = ctxa.HttpContext ?? throw new NullReferenceException("HTTPContext should not be null");
            _logger = logger;
            _getWeavyUserToken = new GetWeavyUserToken(_config, _ctx);
        }

        /// <summary>
        /// Se recibe un parametro de entrada "user" con los datos del usuario y un parametro "refresh" desde el servicio angular, que indica si es necesario generar un nuevo token o se puede usar el existente
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        [HttpGet("~/api/token")]
        public async Task<IActionResult> GetTokenAsync([FromQuery] string currentUserUid, bool refresh = false)
        {
            var jsonAccessToken = await _getWeavyUserToken.GetTokenAsync(currentUserUid, refresh);

            if (jsonAccessToken is not null)
            {
                return Content(jsonAccessToken);
            }
            else 
            {
                return BadRequest();
            } 
        }

        //private readonly HttpContext _ctx;
        //private readonly IConfiguration _config;
        //public AuthenticationController(IConfiguration configuration, IHttpContextAccessor ctxa)
        //{
        //    _config = configuration;
        //    _ctx = ctxa.HttpContext ?? throw new NullReferenceException("HTTPContext should not be null");
        //}
        
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

        //private static HttpClient _httpClient = new HttpClient();

        //private DateTime _time = DateTime.UtcNow.AddHours(4);
        //private Tokens _tokens = new Tokens();

        ///// <summary>
        ///// Se recibe un parametro de entrada "user" con los datos del usuario y un parametro "refresh" desde el servicio angular, que indica si es necesario generar un nuevo token o se puede usar el existente
        ///// </summary>
        ///// <param name="currentUser"></param>
        ///// <param name="refresh"></param>
        ///// <returns></returns>
        ///// <exception cref="NullReferenceException"></exception>
        //[HttpGet("~/api/token")]
        ////[HttpGet(Name = "GetTokenForUser")]
        
        //public async Task<IActionResult> GetToken([FromQuery] User currentUser, bool refresh = false)
        //{
            #region remove
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
            #endregion remove

            #region GetLocalToken
            //string uid = currentUser.UserName;
            //UserAccessToken existingToken = new UserAccessToken();
            //existingToken.ExpirationStatus = ExpirationStatus.NotExists;

            ////if (!refresh && _tokens.TryGetValueAsync(uid, out UserAccessToken existingToken))
            //if (!refresh) //NO REFRESH TOKEN
            //{
            //    // check local token store for existing access_token
            //    var task = await _tokens.TryGetValueAsync(uid);

            //    //evaluo el item1 que indica TRUE=encontrado
            //    if (task != null && task.Item1)
            //    {
            //        //se encontro token
            //        existingToken = task.Item2;

            //        //verifico vigencia del token recuperado
            //        if (existingToken.TimestampFinValidezToken > DateTime.Now)
            //        {
            //            //token ACTIVO                        
            //            existingToken.ExpirationStatus = ExpirationStatus.Active;

            //            Console.WriteLine($"Uid: {existingToken.Uid}");
            //            Console.WriteLine($"Token: {existingToken.Access_token}");
            //            Console.WriteLine($"Token Activo - TimestampFinValidezToken: {existingToken.TimestampFinValidezToken}");

            //            //Return current token that is active
            //            //return Content(existingToken.Token);
            //            AccessToken accessToken = new AccessToken();
            //            accessToken.access_token = existingToken.Access_token;
            //            string jsonAccessToken = JsonSerializer.Serialize<AccessToken>(accessToken);
            //            return Content(jsonAccessToken);
            //        }
            //        else
            //        {
            //            //token EXPIRADO, se debe solicitar nuevo token por lo tanto CONTNUA sin hacer return
            //            existingToken.ExpirationStatus = ExpirationStatus.Inactive;

            //            Console.WriteLine($"Uid: {existingToken.Uid}");
            //            Console.WriteLine($"Token: {existingToken.Access_token}");
            //            Console.WriteLine($"Token Expirado - TimestampFinValidezToken: {existingToken.TimestampFinValidezToken}");
            //        }
            //    }
            //    else
            //    {
            //        //token NO EXISTE, se debe solicitar nuevo token por lo tanto CONTNUA sin hacer return
            //        existingToken.ExpirationStatus = ExpirationStatus.NotExists;

            //        Console.WriteLine($"UserName: {currentUser.UserName}");
            //        Console.WriteLine($"EmailAddress: {currentUser.EmailAddress}");
            //        Console.WriteLine($"REFRESH NO y NO Existe Token para el usuario");
            //    }
            //}
            //else //REFRESH TOKEN
            //{
            //    // check local token store for existing access_token
            //    var task = await _tokens.TryGetValueAsync(uid);

            //    //evaluo el item1 que indica TRUE=encontrado
            //    if (task != null && task.Item1)
            //    {
            //        //se encontro token
            //        existingToken = task.Item2;
            //        //lo trato como expirado
            //        existingToken.ExpirationStatus = ExpirationStatus.Inactive;
            //    }
            //    else
            //    {
            //        //token NO EXISTE, lo trato como inexistente
            //        existingToken.ExpirationStatus = ExpirationStatus.NotExists;

            //        Console.WriteLine($"UserName: {currentUser.UserName}");
            //        Console.WriteLine($"EmailAddress: {currentUser.EmailAddress}");
            //        Console.WriteLine($"REFRESH YES y NO Existe Token para el usuario");
            //    }
            //}
            #endregion GetLocalToken

            #region call API 
            //    // get weavy url and api key from config 
            //    //var weavyUrl = _config["WEAVY-SERVER"];
            //    //var apiKey = _config["WEAVY-API-KEY"];
            //    var weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            //    var apiKey = _config["MySettings:WeavyServerAPIkey"];

            //    // request access_token from Weavy
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            //    var response = await _httpClient.PostAsync($"{weavyUrl}/api/users/{uid}/tokens", null);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        // read access_token
            //        var newTokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            //        if (newTokenResponse == null)
            //        {
            //            throw new NullReferenceException("HttpResponseMessage should not be null");
            //        }
            //        else
            //        {
            //            UserAccessToken newUserAccessToken = new UserAccessToken();

            //            newUserAccessToken.Uid = uid;

            //            //Asigno el NUEVO TOKEN devuelto por la API a la respuesta
            //            newUserAccessToken.Access_token = newTokenResponse.access_token;

            //            //****SEGUN EL ESTADO DEL TOKEN GUARDADO LOCALMENTE, SE DEFINE COMO ACTUALIZAR EL TOKEN EN ALMACENAMIENTO LOCAL
            //            //se setea el estado del token recuperado (NotExists o INACTIVO) para evaluar si hay que insertar o actualizar el token
            //            newUserAccessToken.ExpirationStatus = existingToken.ExpirationStatus;

            //            //lapso de tiempo (inicio-fin) que el token es valido
            //            newUserAccessToken.TimestampInicioValidezToken = DateTime.Now;
            //            newUserAccessToken.TimestampFinValidezToken = newUserAccessToken.TimestampInicioValidezToken.AddSeconds(newTokenResponse.expires_in);                    

            //            // store and return access_token
            //            //_tokens[uid] = accessToken;
            //            await _tokens.TrySetValueAsync(newUserAccessToken);

            //            //Return current token that is active
            //            //return Content(newUserAccessToken.Access_token);
            //            AccessToken accessToken = new AccessToken();
            //            accessToken.access_token = newUserAccessToken.Access_token;
            //            string jsonAccessToken = JsonSerializer.Serialize<AccessToken>(accessToken);
            //            return Content(jsonAccessToken);                    
            //        }
            //    }                                      
            //    return BadRequest();
            #endregion call API
        //}
    }
}
