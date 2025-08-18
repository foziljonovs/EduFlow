using Blazored.SessionStorage;
using EduFlow.Management.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace EduFlow.Management.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private readonly IJSRuntime _jsRuntime;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(
            ISessionStorageService sessionStorageService,
            IJSRuntime jsRuntime)
        {
            _sessionStorageService = sessionStorageService;
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_jsRuntime is IJSInProcessRuntime)
            {
                var token = await _sessionStorageService.GetItemAsync<string>("token");
                if (string.IsNullOrEmpty(token))
                    return new AuthenticationState(_anonymous);

                var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            else
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public void AuthenticateUser(string token)
        {
            var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void Logout()
        {
            var authState = Task.FromResult(new AuthenticationState(_anonymous));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
