using Domain.Entities;

namespace UI.Components.Pages.Breakdown
{
    public partial class Index
    {
        private bool
            _isLoading = false;

        private List<Domain.Entities.Breakdown> _breakdownsList = new();

        private User _user = new();

        protected override async Task OnInitializedAsync()
        {
            await GetClaimsUserAsync();

            _isLoading = true;

            await GetBreakdownsAsync();

            _isLoading = false;
        }

        private async Task GetBreakdownsAsync()
        {
            try
            {
                var breakdowns = await _breakdownServices.GetBreakdownsIEnumerableAsync();

                var enumerable = breakdowns.ToList();

                var breakdownUserList =
                    enumerable.Where(bu => bu.BreakdownUsers!.Any(b => b.UserId == _user.UserId)).ToList();

                if (breakdownUserList.Count > 0 && _user.Role is { Name: "Colaborador" })
                {
                    _breakdownsList = breakdownUserList.ToList();
                    return;
                }

                _breakdownsList = enumerable.ToList();
            }
            catch (Exception)
            {
                await _dialogService.ShowErrorAsync(
                    "Ocorreu um erro inesperado ao tentar obter a lista de OS. Caso o erro persista, favor entrar em contato com o administrador do sistema.");
            }
        }

        private async Task GetClaimsUserAsync()
        {
            var auth = await _authServices.GetAuthenticationStateAsync();

            var user = auth.User;

            if (user.Identity?.IsAuthenticated is true)
                _user = new User
                {
                    Name = user.Claims.ElementAt(0).Value,
                    Login = user.Claims.ElementAt(1).Value,
                    UserId = int.Parse(user.Claims.ElementAt(2).Value),
                    Role = new Roles()
                    {
                        Name = user.Claims.ElementAt(3).Value
                    }
                };
        }

        private void BreakdownDetails(int breakdownId) => _navigationManager.NavigateTo($"/breakdownviewer/{breakdownId}");
    }
}