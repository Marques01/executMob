namespace UI.Components.Pages
{
    public partial class Home
    {
        private string
            _userName = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await GetClaimsUserAsync();
        }
        public async Task GetClaimsUserAsync()
        {
            var auth = await _authServices.GetAuthenticationStateAsync();

            var user = auth.User;

            if (user.Identity?.IsAuthenticated is true)
                _userName = user.Claims.ElementAt(0).Value.ToString();
        }
    }
}
