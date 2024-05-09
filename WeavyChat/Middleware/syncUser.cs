using System.Net.Http.Headers;
using WeavyChat.Entities;
using System.Text.Json;

namespace WeavyChat.Middleware
{
    public class syncUser
    {
        private readonly string? _weavyUrl;
        private readonly string? _apiKey;

        public syncUser()
        {
        }
        public syncUser(IConfiguration _config, HttpContext ctx)
        {
            _weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            _apiKey = _config["MySettings:WeavyServerAPIkey"];
        }
        
        public async Task<TokenResponse?> GetTokenAsync(string uid)
        {
            HttpClient _httpClient = new HttpClient();

            // request access_token from Weavy
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await _httpClient.PostAsync($"{_weavyUrl}/api/users/{uid}/tokens", null);

            if (response.IsSuccessStatusCode)
            {
                // read access_token
                var newTokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                if (newTokenResponse == null)
                {
                    throw new NullReferenceException("HttpResponseMessage should not be null: newTokenResponse is null");
                }
                else
                {
                    //token recuperado correctamente
                    return newTokenResponse;
                }
            }
            //se retorna null porque el acceso a la API de tokens weavy fue Bad Request
            return null;
        }

        public async Task<UserSync.Output> GetUserAsync(string uid, bool trashed, IConfiguration _config, HttpContext _ctx)
        {
            //var weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            //var apiKey = _config["MySettings:WeavyServerAPIkey"];
            HttpClient _httpClient = new HttpClient();

            // get weavy user
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.GetAsync($"{_weavyUrl}/api/users/{uid}");

            if (response.IsSuccessStatusCode)
            {
                // read User in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<UserSync.Output>();
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

        public async Task<UserList.OutputUserList> ListUsersAsync(UserList.InputUserList userListInput, IConfiguration _config, HttpContext _ctx)
        {
            //var weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            //var apiKey = _config["MySettings:WeavyServerAPIkey"];
            HttpClient _httpClient = new HttpClient();

            // get weavy user
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string? queryParameters = nameof(userListInput.top) + "=" + userListInput.top.ToString();

            var response = await _httpClient.GetAsync($"{_weavyUrl}/api/users?{queryParameters}");

            if (response.IsSuccessStatusCode)
            {
                // Get Users in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<UserList.OutputUserList>();
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

        public async Task<UserSync.Output> UpsertUserAsync(User currentUser, IConfiguration _config, HttpContext _ctx)
        {
            //var weavyUrl = _config["MySettings:WeavyServerEnvironmentURL"];
            //var apiKey = _config["MySettings:WeavyServerAPIkey"];
            HttpClient _httpClient = new HttpClient();

            // update user in Weavy
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //INPUT REQUEST CLASS
            UserSync.Input newUserSync = new UserSync.Input();
            newUserSync.display_name = currentUser.DisplayName;
            newUserSync.email = currentUser.EmailAddress;
            newUserSync.name = currentUser.UserName;
            newUserSync.uid = currentUser.UserName;
            newUserSync.phone_number = "123456789";
            //newUserSync.directory_id = int.Parse(currentUser.OrgId);
            newUserSync.directory = currentUser.OrgRoleName;
            //newUserSync.directory.id = int.Parse(currentUser.OrgId);
            //newUserSync.directory.name = currentUser.OrgId;
            newUserSync.comment = $"OrgId: {currentUser.OrgId} OrgRoleName: {currentUser.OrgRoleName}";

            var dataAsString = JsonSerializer.Serialize(newUserSync);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PutAsync($"{_weavyUrl}/api/users/{currentUser.UserName}", content);

            if (response.IsSuccessStatusCode)
            {
                // read User in OUTPUT REQUEST CLASS
                var newUserResponse = await response.Content.ReadFromJsonAsync<UserSync.Output>();
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
