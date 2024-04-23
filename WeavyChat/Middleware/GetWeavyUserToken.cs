using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WeavyChat.Entities;

namespace WeavyChat.Middleware
{
    public class GetWeavyUserToken
    {
        private readonly string? _weavyUrl;
        private readonly string? _apiKey;
        private readonly HttpContext? _ctx;
        private readonly syncUser _syncUser = new syncUser();
        private readonly Tokens _tokens = new Tokens();

        public GetWeavyUserToken()
        {
        }
        public GetWeavyUserToken(IConfiguration _config, HttpContext ctx)
        {
            _weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            _apiKey = _config["MySettings:WeavyServerAPIkey"];
            _ctx = ctx ?? throw new NullReferenceException("HTTPContext should not be null");
            _syncUser = new syncUser(_config, _ctx);
        }

        public async Task<string?> GetTokenAsync(string currentUserUid, bool refresh = false)
        {            
            //***TOMAR EL USUARIO DEL CONTEXT
            string uid = currentUserUid;
            UserAccessToken existingToken = new UserAccessToken();
            existingToken.ExpirationStatus = ExpirationStatus.NotExists;

            //if (!refresh && _tokens.TryGetValueAsync(uid, out UserAccessToken existingToken))
            if (!refresh) //NO REFRESH TOKEN
            {
                // check local token store for existing access_token
                var task = await _tokens.TryGetValueAsync(uid);

                //evaluo el item1 que indica TRUE=encontrado
                if (task != null && task.Item1)
                {
                    //se encontro token y se lo asigna al UserAccessToken a devolver
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
                        AccessToken accessToken = new AccessToken();
                        accessToken.access_token = existingToken.Access_token;
                        string existingJsonAccessToken = JsonSerializer.Serialize<AccessToken>(accessToken);
                        return existingJsonAccessToken;
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
                else
                {
                    //token NO EXISTE, se debe solicitar nuevo token por lo tanto CONTNUA sin hacer return
                    existingToken.ExpirationStatus = ExpirationStatus.NotExists;

                    Console.WriteLine($"UserName: {currentUserUid}");
                    Console.WriteLine($"REFRESH NO y NO Existe Token para el usuario");
                }
            }
            else //REFRESH TOKEN
            {
                // check local token store for existing access_token
                var task = await _tokens.TryGetValueAsync(uid);

                //evaluo el item1 que indica TRUE=encontrado
                if (task != null && task.Item1)
                {
                    //se encontro token
                    existingToken = task.Item2;
                    //lo trato como expirado porque se piude un refresh
                    existingToken.ExpirationStatus = ExpirationStatus.Inactive;
                }
                else
                {
                    //token NO EXISTE, lo trato como inexistente. se debe solicitar nuevo token
                    existingToken.ExpirationStatus = ExpirationStatus.NotExists;

                    Console.WriteLine($"UserName: {currentUserUid}");
                    Console.WriteLine($"REFRESH YES y NO Existe Token para el usuario");
                }
            }
            
            //LLamada a la API de generación de TOKENS WEAVY
            var newTokenResponse = await _syncUser.GetTokenAsync(uid);

            if (newTokenResponse == null)
            {
                Console.WriteLine("HttpResponseMessage should not be null");
                //se retorna null porque es un Bad Request
                return null;
            }
            else
            {
                #region Update local tokens storage
                UserAccessToken newUserAccessToken = new UserAccessToken();

                newUserAccessToken.Uid = uid;
                
                //Asigno el NUEVO TOKEN devuelto por la API a la respuesta
                newUserAccessToken.Access_token = newTokenResponse.access_token;

                //****SEGUN EL ESTADO DEL TOKEN GUARDADO LOCALMENTE, SE DEFINE COMO ACTUALIZAR EL TOKEN EN ALMACENAMIENTO LOCAL
                //se setea el estado del token recuperado (NotExists o INACTIVO) para evaluar si hay que insertar o actualizar el token
                newUserAccessToken.ExpirationStatus = existingToken.ExpirationStatus;

                //lapso de tiempo (inicio-fin) que el token es valido
                newUserAccessToken.TimestampInicioValidezToken = DateTime.Now;
                newUserAccessToken.TimestampFinValidezToken = newUserAccessToken.TimestampInicioValidezToken.AddSeconds(newTokenResponse.expires_in);

                // store and return access_token
                //_tokens[uid] = accessToken;
                await _tokens.TrySetValueAsync(newUserAccessToken);
                #endregion Update local tokens storage

                //Return current token that is active
                AccessToken accessToken = new AccessToken();
                accessToken.access_token = newUserAccessToken.Access_token;
                string newJsonAccessToken = JsonSerializer.Serialize<AccessToken>(accessToken);
                return newJsonAccessToken;
            }                        
        }
    }
}
