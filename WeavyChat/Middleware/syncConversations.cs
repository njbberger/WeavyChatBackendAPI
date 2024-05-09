using EnumsNET;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeavyChat.Entities;
using static WeavyChat.Entities.Conversation;
using static WeavyChat.Entities.WeavyApp;

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

            //Get USER TOKEN FOR THE REQUEST
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

        //GET /api/conversations
        public async Task<OutputListConversations> ListConversationsAsync(string currentUserName, InputListConversations inputListConversations)
        {
            HttpClient _httpClient = new HttpClient();

            //Get USER TOKEN FOR THE REQUEST
            var jsonAccessToken = await _getWeavyUserToken.GetTokenAsync(currentUserName, false) ?? throw new NullReferenceException("User TOKEN should not be null: AccessToken is null");
            var userAccesToken = JsonSerializer.Deserialize<AccessToken>(jsonAccessToken) ?? throw new NullReferenceException("User Token Error: NULL");

            // get members of the app
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccesToken.access_token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string? queryParameters = nameof(inputListConversations.member) + "=" + inputListConversations.member;

            if (inputListConversations.type != null && inputListConversations.type.Count > 0)
            {
                queryParameters = queryParameters + "&" + nameof(inputListConversations.type) + "=" + inputListConversations.type[0].ToString();
            }

            var response = await _httpClient.GetAsync($"{_weavyUrl}/api/conversations?{queryParameters}");

            if (response.IsSuccessStatusCode)
            {
                // Get Apps in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<OutputListConversations>();
                if (newUserResponse == null)
                {
                    throw new NullReferenceException("HttpResponseMessage should not be null");
                }
                else
                {
                    return newUserResponse;
                }
            }

            throw new NullReferenceException("HttpResponseMessage should not be null");
        }

        /// <summary>
        /// Sync process to Create or Remove Conversations for the authenticated user based on updated Permited Contact List (Called from Login process or Messenger Icon Click)
        /// </summary>
        /// <param name="currentUserName"></param>
        /// <returns>bool</returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<bool> SyncConversationAsync(string authenticatedUserName, string conversationWithUserName)
        {
            HttpClient _httpClient = new HttpClient();

            #region Permited Contact List
            //Get Permited Contact List to chat for current user
            List<string> UsersAvailableToChat = new List<string>();
            //UsersAvailableToChat = GetListUsersAvailableToChat(); //internal process based on business rules
            #endregion Permited Contact List

            #region Current Conversation List
            //Get Current Conversation List (chats) already created for the current user        
            InputListConversations inputListOfChats = new InputListConversations();
            inputListOfChats.member = conversationWithUserName;
            inputListOfChats.type.Add("7e14f418-8f15-46f4-b182-f619b671e470");

            OutputListConversations CurrentListOfChats = await ListConversationsAsync(authenticatedUserName, inputListOfChats); //List Conversation Weavy API

            List<string> CurrentListOfChatsIDs = new List<string>();
            foreach (var currentChat in CurrentListOfChats.data)
            {
                CurrentListOfChatsIDs.Add(currentChat.uid);
            }

            #region Call List Conversations API
            //Get USER TOKEN FOR THE REQUEST
            var jsonAccessToken = await _getWeavyUserToken.GetTokenAsync(authenticatedUserName, false) ?? throw new NullReferenceException("User TOKEN should not be null: AccessToken is null");
            var userAccesToken = JsonSerializer.Deserialize<AccessToken>(jsonAccessToken) ?? throw new NullReferenceException("User Token Error: NULL");

            // HTTP Client config            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccesToken.access_token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //
            #endregion Call List Conversations API

            #endregion Current Conversation List


            #region New Conversation API
            //Obtain List of new Conversations to create
            var newUserConversationsToCreate = UsersAvailableToChat.Except(CurrentListOfChatsIDs).ToList();
            
            InputCreateConversation inputCreateConversation = new InputCreateConversation();

            foreach (var UserConversationToCreate in newUserConversationsToCreate) 
            {                
                inputCreateConversation.members.Add(UserConversationToCreate);

                var newConversationCreated = await CreateConversationAsync(authenticatedUserName, inputCreateConversation);
                
                //clean the list so next time just one person will be used to create a new conversation (private conversation) 
                inputCreateConversation.members.Clear();
            }
            #endregion New Conversation API

            #region Remove Member or Trash/Delete App API
            //Obtain List of conversation to be removed or set to Read only
            var UserConversationsToUpdate = CurrentListOfChatsIDs.Except(UsersAvailableToChat).ToList();

            foreach (var userConversationToUpdate in UserConversationsToUpdate) 
            {
                
            }
            #endregion Remove Member or Trash/Delete App API

            return true;
        }
    }
}