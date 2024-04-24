using Microsoft.JSInterop;

namespace UI.Components.Layout
{
    public partial class NavMenu
    {
        private string
            _userName = string.Empty,
            _base64 = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await GetClaimsUserAsync();

            await GetBase64Async();
        }
        public async Task GetClaimsUserAsync()
        {
            var auth = await _authServices.GetAuthenticationStateAsync();

            var user = auth.User;

            if (user.Identity?.IsAuthenticated is true)
                _userName = user.Claims.ElementAt(0).Value.ToString();
        }

        public async Task LogoutAsync()
        {
            await _authServices.Logout();
            _navigationManager.NavigateTo("/login", true);
        }

        private async Task NavigateAndCloseOffcanvas(string uri)
        {
            _navigationManager.NavigateTo(uri);
            await _js.InvokeVoidAsync("closeOffcanvas");
        }

        private async Task GetBase64Async()
        {
            string userImage = string.Empty;

            var auth = await _authServices.GetAuthenticationStateAsync();

            var user = auth.User;

            if (user.Identity?.IsAuthenticated is true)
                userImage = user.Claims.ElementAt(3).Value.ToString();

            if (!string.IsNullOrEmpty(userImage))
                _base64 = await _pictureStorageServices.GetBase64Async(userImage);
        }
    }
}