//using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;
using WeavyChat.Utils;

namespace WeavyChat.Entities
{
    public class Tokens
    {
        //public string WeavyServerEnvironmentURL { get; set; } = "https://f075cd1672b24d388bbec498ebe7e762.weavy.io";
        //public string WeavyServerAPIkey { get; set; } = "wys_f5X8hRoF7ZfSj7vfI7Yr5p2LIM8FRb3wgl05";
        //public List<UserAccessToken> UserAccessTokens { get; set; } = new List<UserAccessToken>();

        //public async Task<bool> TryGetValueAsync(string? uid, out UserAccessToken OutExistingToken)
        public async Task<Tuple<bool, UserAccessToken>> TryGetValueAsync(string? uid)
        {
            try
            {
                //open tokens json file
                //string jsonfile = File.ReadAllText(@".\ACTIVETOKENS.json");
                
                //Initialize existingToken. It will be set initially with ExpirationStatus.None
                var OutExistingToken = new UserAccessToken();

                if (File.Exists(@".\ACTIVETOKENS.json"))
                {
                    var userAccessTokenList = await Json.ReadAsync<List<UserAccessToken>>(@".\ACTIVETOKENS.json");                    

                    //var userAccessToken = new UserAccessToken();

                    //if (jsonfile != string.Empty)
                    if (userAccessTokenList != null && userAccessTokenList.Any())
                    {
                        //List<UserAccessToken>? userAccessTokenLists = JsonSerializer.Deserialize<List<UserAccessToken>>(jsonfile);                    

                        //un usuario solo puede tener un token activo a la vez
                        OutExistingToken = userAccessTokenList.FirstOrDefault(token => token.Uid == uid);

                        //if (userAccessTokenList != null && userAccessTokenList.Any())
                        //{
                        //    userAccessToken = userAccessTokenList.FirstOrDefault(token => token.Uid == uid);
                        //}
                        //else
                        //{
                        //    userAccessToken = null;

                        //}                

                        //if (userAccessToken is not null)
                        //{
                        //    //Devuelvo el token existente, sea vigente o expirado
                        //    OutExistingToken = userAccessToken;

                        //    return true;                    
                        //}
                        //else
                        //{
                        //    //No existe Token para el usuario
                        //    Console.WriteLine($"Uid: {uid}");
                        //    Console.WriteLine($"No existe Token para el usuario");
                        //    return false;
                        //}
                    }
                    else
                    {
                        OutExistingToken = null;

                    }
                }
                else
                {
                    //Se crea archivo por primera vez
                    StreamWriter file = File.CreateText(@".\ACTIVETOKENS.json");
                    file.Close();
                    OutExistingToken = null;
                }

                if (OutExistingToken != null)
                {
                    //Devuelvo el token existente, sea vigente o expirado
                    //OutExistingToken = userAccessToken;

                    //return true;
                    return new Tuple<bool, UserAccessToken>(true, OutExistingToken);
                }
                else
                {
                    //No existe Token para el usuario
                    Console.WriteLine($"Uid: {uid}");
                    Console.WriteLine($"No existe Token para el usuario");
                    //return false;
                    return new Tuple<bool, UserAccessToken>(false, new UserAccessToken());
                }
            }
            catch (Exception e)
            {

                throw new Exception("Error al intentar obtener el token de usuario desde el archivo ACTIVETOKENS.json", e);
            }                                    
        }

        public async Task<bool> TrySetValueAsync(UserAccessToken newUserAccessToken)
        {
            try
            {
                //Si el token devuelto tiene estado Inactive, implica que hay que actualizarlo con el nuevo
                //Si el token devuelto tiene estado None, implica que NO existe token para el usuario
                if (newUserAccessToken.ExpirationStatus == ExpirationStatus.Inactive)
                {
                    //***Read entire list of tokens
                    //Read file to string
                    //var accessTokensJsonFile = File.OpenRead(@".\ACTIVETOKENS.json");
                    //var userAccessTokenList = await Json.ReadAsync<List<UserAccessToken>>(@".\ACTIVETOKENS.json");
                    var userAccessTokenList = await Json.ReadAsync<List<UserAccessToken>>(@".\ACTIVETOKENS.json");

                    //Deserialize from file to object
                    //var userAccessTokenList = new List<UserAccessToken>();
                    //if (userAccessTokenList != null && userAccessTokenList.Count > 0)
                    //{                        
                    //    userAccessTokenList = await JsonSerializer.DeserializeAsync<List<UserAccessToken>>(accessTokensJsonFile);
                    //}

                    //Set new token expiration status
                    newUserAccessToken.ExpirationStatus = ExpirationStatus.Active;

                    //***Find specific token to UPDATE                    
                    if (userAccessTokenList != null && userAccessTokenList.Any())
                    {
                        int pos = userAccessTokenList.FindIndex(n => n.Uid == newUserAccessToken.Uid);
                        if (pos >= 0)
                        {
                            userAccessTokenList[pos] = newUserAccessToken;
                            Console.WriteLine($"Se actualiza el token del usuario {newUserAccessToken.Uid} en el archivo ACTIVETOKENS.json");
                        }
                        else
                        {
                            userAccessTokenList.Add(newUserAccessToken);
                            Console.WriteLine($"Se agrega nuevo token ya que No existe el token de usuario en el archivo ACTIVETOKENS.json: {newUserAccessToken.Uid}");
                        }
                    }
                    else 
                    {
                        userAccessTokenList = new List<UserAccessToken>();
                        userAccessTokenList.Add(newUserAccessToken);
                        Console.WriteLine($"Se agrega nuevo token para el usuario {newUserAccessToken.Uid} ya que el archivo ACTIVETOKENS.json se encuentra vacio");
                    }

                    //***UPDATE token writing the entire list again
                    using (StreamWriter file = File.CreateText(@".\ACTIVETOKENS.json"))
                    {
                        //System.Txt.Json - a LIST of tokens
                        var stream = new MemoryStream();
                        await JsonSerializer.SerializeAsync<List<UserAccessToken>>(stream, userAccessTokenList, new JsonSerializerOptions() { WriteIndented = true });                        
                        stream.Position = 0;
                        using var reader = new StreamReader(stream);
                        string newUserAccessTokenList = await reader.ReadToEndAsync();

                        //write string to file
                        await file.WriteAsync(newUserAccessTokenList);                        

                        return true;
                    }                    
                }
                else if(newUserAccessToken.ExpirationStatus == ExpirationStatus.NotExists) //INSERT token (userAccessToken.ExpirationStatus = NotExists)
                {
                    //***Read entire list of tokens
                    //Read file to string
                    //string accessTokensJsonFile = File.ReadAllText(@".\ACTIVETOKENS.json");
                    var userAccessTokenList = await Json.ReadAsync<List<UserAccessToken>>(@".\ACTIVETOKENS.json");

                    //Deserialize from file to object
                    //var userAccessTokenLists = new List<UserAccessToken>();
                    //if (!string.IsNullOrEmpty(accessTokensJsonFile))
                    //{
                    //    userAccessTokenLists = await JsonSerializer.DeserializeAsync<List<UserAccessToken>>(accessTokensJsonFile);
                    //}

                    //Set token expiration status
                    newUserAccessToken.ExpirationStatus = ExpirationStatus.Active;

                    //ADD NEW TOKEN                 
                    if (userAccessTokenList != null && userAccessTokenList.Any())
                    {                        
                        userAccessTokenList.Add(newUserAccessToken);
                    }
                    else
                    {
                        userAccessTokenList = new List<UserAccessToken>();  
                        userAccessTokenList.Add(newUserAccessToken);
                    }

                    //open file stream
                    using (StreamWriter file = File.CreateText(@".\ACTIVETOKENS.json"))
                    {                        
                        //serialize object directly into file stream
                        //Newtonsoft
                        //string json = JsonConvert.SerializeObject(userAccessToken);                        

                        //System.Txt.Json - 1 token
                        var stream = new MemoryStream();
                        await JsonSerializer.SerializeAsync<List<UserAccessToken>>(stream, userAccessTokenList, new JsonSerializerOptions() { WriteIndented = true });
                        stream.Position = 0;
                        using var reader = new StreamReader(stream);
                        string newUserAccessTokenList = await reader.ReadToEndAsync();

                        //write string to file
                        await file.WriteAsync(newUserAccessTokenList);

                        return true;
                    }
                }
                else { throw new Exception("Error en el estado del token NO valido"); }                
            }
            catch (Exception e)
            {
                throw new Exception("Error al intentar grabar el token de usuario en el archivo ACTIVETOKENS.json", e);
            }            
        }
    }

    // Define a class to store token values to be converted to JSON  
    public class UserAccessToken
    {
        // Make sure all class attributes have relevant getter setter.        
        public string Uid { get; set; } = string.Empty;
        public string Access_token { get; set; } = string.Empty;
        public ExpirationStatus ExpirationStatus { get; set; } = ExpirationStatus.NotExists;
        public DateTime TimestampInicioValidezToken { get; set; } = DateTime.MinValue;
        public DateTime TimestampFinValidezToken { get; set; } = DateTime.MinValue;
    }

    public class TokenResponse 
    {
        public string access_token { get; set; } = string.Empty;
        public int expires_in { get; set; } = 0;
    }

    public class AccessToken
    {
        public string access_token { get; set; } = string.Empty;
    }

    public enum ExpirationStatus
    {
        NotExists = 0,
        Active = 1,
        Inactive = 2,
    }
}
