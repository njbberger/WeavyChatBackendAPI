using EnumsNET;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using WeavyChat.Entities;
using static WeavyChat.Entities.WeavyApp;

namespace WeavyChat.Middleware
{
    public class syncApp
    {
        private readonly string? _weavyUrl;
        private readonly string? _apiKey;

        public syncApp()
        {
        }
        public syncApp(IConfiguration _config, HttpContext ctx)
        {
            _weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            _apiKey = _config["MySettings:WeavyServerAPIkey"];
        }

        public async Task<WeavyApp.OutputGetApp> GetAppAsync(string uid, bool trashed)
        {
            HttpClient _httpClient = new HttpClient();

            // get weavy user
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.GetAsync($"{_weavyUrl}/api/apps/{uid}");

            if (response.IsSuccessStatusCode)
            {
                // read User in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<WeavyApp.OutputGetApp>();
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

        //GET /api/apps
        public async Task<OutputListApp> ListAppsAsync(InputListApp inputListApp)
        {
            HttpClient _httpClient = new HttpClient();

            // get members of the app
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string? queryParameters = nameof(inputListApp.contextual) + "=" + inputListApp.contextual.ToString();

            if (inputListApp.type != null && inputListApp.type.Count() > 0)
            {
                queryParameters = queryParameters + "&" + nameof(inputListApp.type) + "=" + inputListApp.type[0].ToString();
            }             

            var response = await _httpClient.GetAsync($"{_weavyUrl}/api/apps?{queryParameters}");

            if (response.IsSuccessStatusCode)
            {
                // Get Apps in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<OutputListApp>();
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

        //PUT /api/apps/{app}/members/{user}
        public async Task<string> AddMemberAsync(string AppUid, string UserNameUid, WeavyApp.Access MemberAccess)
        {
            HttpClient _httpClient = new HttpClient();

            // get weavy user
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Optional access level for member.Defaults to write when not specified.
            //none = cannot access the app
            //read = can see the app and it's content but can cannot contribute
            //write = access to the app and is allowed to create content

            string access = MemberAccess.AsString(EnumFormat.Description);           

            var bodyParameters = JsonSerializer.Serialize(access);
            var content = new StringContent(bodyParameters);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PutAsync($"{_weavyUrl}/api/apps/{AppUid}/members/{UserNameUid}", content);

            if (response.IsSuccessStatusCode)
            {
                // read User in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<string>();
                if (newUserResponse == null)
                {
                    throw new NullReferenceException("HttpResponseMessage should not be null");
                }
                else
                {
                    return newUserResponse;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return $"User {UserNameUid} added to the Application {AppUid} with the permission {access}";
            }
            else
            {
                throw new NullReferenceException("HttpResponseMessage not expected");
            }
        }

        //GET /api/apps/{app}/members
        public async Task<OutputListMembers> ListMembersAsync(InputListMembers inputListMembers)
        {
            HttpClient _httpClient = new HttpClient();

            // get members of the app
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string? queryParameters = nameof(inputListMembers.top) + "=" + inputListMembers.top.ToString();

            var response = await _httpClient.GetAsync($"{_weavyUrl}/api/apps/{inputListMembers.app}/members?{queryParameters}");

            if (response.IsSuccessStatusCode)
            {
                // Get Users in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<OutputListMembers>();
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
    }
}
