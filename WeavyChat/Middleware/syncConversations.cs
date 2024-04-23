using EnumsNET;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeavyChat.Entities;
using static WeavyChat.Entities.Conversation;

namespace WeavyChat.Middleware
{
    public class syncConversations
    {
        private readonly string? _weavyUrl;
        private readonly string? _apiKey;
        private readonly HttpContext? _ctx;
        private readonly GetWeavyUserToken _getWeavyUserToken = new GetWeavyUserToken();

        public syncConversations()
        {
        }
        public syncConversations(IConfiguration _config, HttpContext ctx)
        {
            _weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            _apiKey = _config["MySettings:WeavyServerAPIkey"];
            _ctx = ctx;
            _getWeavyUserToken = new GetWeavyUserToken(_config, _ctx);
        }

        /// <summary>
        /// POST /api/conversations
        /// Creates a new conversation with the specified members. When no type is specified the following logic is applied to decide the type of conversation to create:
        /// -If a name is specified, or member count is more than one, a chat room is created.
        /// -If only one member is specified, a private chat is created.If there is an existing chat between the creator and member, that instance is returned instead of creating a new chat.
        /// </summary>
        /// <param name="inputCreateConversation"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<OutputCreateConversation> CreateConversationAsync(string currentUserName, InputCreateConversation inputCreateConversation)
        {
            HttpClient _httpClient = new HttpClient();

            //Get UER TOKEN FOR THE REQUEST
            var jsonAccessToken = await _getWeavyUserToken.GetTokenAsync(currentUserName, false) ?? throw new NullReferenceException("User TOKEN should not be null: AccessToken is null");
            var userAccesToken = JsonSerializer.Deserialize<AccessToken>(jsonAccessToken) ?? throw new NullReferenceException("User Token Error: NULL");

            // HTTP Client config
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccesToken.access_token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Optional access level for member.Defaults to write when not specified.
            //none = cannot access the app
            //read = can see the app and it's content but can cannot contribute
            //write = access to the app and is allowed to create content

            //Currently is only allowed to create private conversations therefore name and type should not be included in body porameters
            inputCreateConversation.name = null;
            inputCreateConversation.type = null;

            JsonSerializerOptions options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            //Body parameters with only one member to create a private conversation            
            var JsonBodyParameters = JsonSerializer.Serialize(inputCreateConversation, options);

            var content = new StringContent(JsonBodyParameters);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync($"{_weavyUrl}/api/conversations", content);

            if (response.IsSuccessStatusCode)
            {
                // read Conversation in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<OutputCreateConversation>();
                if (newUserResponse == null)
                {
                    throw new NullReferenceException("HttpResponseMessage should not be null");
                }
                else
                {
                    Console.WriteLine($"Chat Conversation for User {currentUserName} with User {inputCreateConversation.members[0]} was created");
                    return newUserResponse;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine($"Chat Conversation for User {currentUserName} with User {inputCreateConversation.members[0]} not created due to: {response.StatusCode.ToString()}");
                throw new NullReferenceException($"HttpResponseMessage Forbidden or Unauthorized {response.StatusCode.ToString()}");
            }
            else 
            {
                throw new NullReferenceException($"HttpResponseMessage not expected {response.StatusCode.ToString()}");
            }
        }
    }
}
