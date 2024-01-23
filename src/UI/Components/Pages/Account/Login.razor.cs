using Domain.Entities.Request;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace UI.Components.Pages.Account
{
    public partial class Login
    {
        private UserRequestModel _userViewModel = new();

        private EditContext? _editContext;

        private bool
            _isLoading = false,
            _showValidationLogin = false,
            _showValidationPassword = false;
        protected override void OnInitialized()
        {
            var serviceCollection = new ServiceCollection();

            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            _editContext = new(_userViewModel);

            _editContext.EnableDataAnnotationsValidation(provider);
        }

        private async Task HandleSubmit()
        {
            if (_editContext is not null && _editContext.Validate())
                await SigninAsync();

            ValidationLoginMessage();
            ValidationPasswordMessage();
        }

        private async Task SigninAsync()
        {
            try
            {
                _isLoading = true;

                var userToken = await _accountServices.SignInAsync(_userViewModel);

                if (userToken.StatusCode == HttpStatusCode.BadRequest)
                    throw new ArgumentException(userToken.Message);

                if (userToken.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception(userToken.Message);

                if (userToken.IsSuccess && userToken.Model is not null)
                {
                    await _authServices.LoginAsync(userToken.Model.Token);

                    _navigationManager.NavigateTo("/", true);
                }
            }
            catch (ArgumentException args)
            {
                await ShowErrorAsync(args.Message, "Atenção");
            }
            catch (Exception ex) when (ex.Message.Contains("fetch"))
            {
                await ShowErrorAsync("Não foi possível conectar ao servidor, tente novamente mais tarde.", "Error");
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
            {
                await ShowErrorAsync("Tempo de conexão esgotado, tente novamente mais tarde.", "Error");
            }
            catch (Exception)
            {
                await ShowErrorAsync("Ocorreu um erro inesperado. Caso o error persista, entrar em contato com administrador do sistema", "Error");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task ShowErrorAsync(string message, string? title = null) => await DialogService.ShowErrorAsync(message, title);

        private void ValidationLoginMessage() => _showValidationLogin = string.IsNullOrEmpty(_userViewModel.Login);

        private void ValidationPasswordMessage() => _showValidationPassword = string.IsNullOrEmpty(_userViewModel.Password);

    }
}