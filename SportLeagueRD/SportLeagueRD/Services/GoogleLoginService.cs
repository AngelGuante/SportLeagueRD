using Newtonsoft.Json;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SportLeagueRD.Services {
    class GoogleLoginService {
        #region VARIABLES
        private readonly string AppName = AppInfo.Name;

        private readonly string Scope = "https://www.googleapis.com/auth/userinfo.email";
        private readonly string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        private readonly string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private readonly string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        private Account account = null;
        private AccountStore store = null;

        private readonly string clientId = null;
        private readonly string redirectUri = null; 
        #endregion

        public GoogleLoginService() {
            switch (Xamarin.Forms.Device.RuntimePlatform) {
                case Xamarin.Forms.Device.iOS:
                    clientId = App.iOSClientId;
                    redirectUri = App.iOSRedirectUrl;
                    break;

                case Xamarin.Forms.Device.Android:
                    clientId = App.AndroidClientId;
                    redirectUri = App.AndroidRedirectUrl;
                    break;
            }
            store = AccountStore.Create();

            account = store.FindAccountsForService(AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Scope,
                new Uri(AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            App.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        } 

        private async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e) {
            if (sender is OAuth2Authenticator authenticator) {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            UserGoogleProfile user = null;
			if (e.IsAuthenticated) {
				// If the user is authenticated, request their basic user data from Google
				// UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
				var request = new OAuth2Request("GET", new Uri(UserInfoUrl), null, e.Account);
				var response = await request.GetResponseAsync();
				if (response != null) {
					// Deserialize the data and store it in the account store
					// The users email address will be used to identify data in SimpleDB
					string userJson = await response.GetResponseTextAsync();
					user = JsonConvert.DeserializeObject<UserGoogleProfile>(userJson);
				}

				if (account != null)
					store.Delete(account, AppName);

                await store.SaveAsync(account = e.Account, AppName);

                LlamarVentana(user.Email);
            }
            App.Authenticator = null;

        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e) {
            if (sender is OAuth2Authenticator authenticator) {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Application.Current.MainPage.DisplayAlert("Authentication error", e.Message, "OK");

            App.Authenticator = null;
		}

        //  MANDA AL USUARIO A LA VENTANA DE PEDIR DATOS RESTANTES PARA COMPLETAR EL LOGEO
        private async void LlamarVentana(string usertEmail) {
            await Application.Current.MainPage.Navigation.PopAsync();
            App.page = new view_pedirDatosUsuarioRegistro(usertEmail);
            Application.Current.MainPage = new NavigationPage(App.page);
        }
    }
}
