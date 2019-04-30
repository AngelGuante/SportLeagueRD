using Newtonsoft.Json;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

namespace SportLeagueRD.Services {
    class GoogleLoginService {
        private Account account;
        private AccountStore store;

        private readonly string clientId = null;
        private readonly string redirectUri = null;

        public GoogleLoginService() {
            switch (Device.RuntimePlatform) {
                case Device.iOS:
                    clientId = App.iOSClientId;
                    redirectUri = App.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = App.AndroidClientId;
                    redirectUri = App.AndroidRedirectUrl;
                    break;
            }
            store = AccountStore.Create();

            account = store.FindAccountsForService(App.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                App.Scope,
                new Uri(App.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(App.AccessTokenUrl),
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
				var request = new OAuth2Request("GET", new Uri(App.UserInfoUrl), null, e.Account);
				var response = await request.GetResponseAsync();
				if (response != null) {
					// Deserialize the data and store it in the account store
					// The users email address will be used to identify data in SimpleDB
					string userJson = await response.GetResponseTextAsync();
					user = JsonConvert.DeserializeObject<UserGoogleProfile>(userJson);
				}

				if (account != null)
					store.Delete(account, App.AppName);

                await store.SaveAsync(account = e.Account, App.AppName);

                //--
                LlamarVentana(user.Email);
            }
        }

        private async void LlamarVentana(string usertEmail) {
            await Application.Current.MainPage.Navigation.PopAsync();
            App.page = new view_pedirDatosUsuarioRegistro(usertEmail);
            Application.Current.MainPage = new NavigationPage(App.page);
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e) {
            if (sender is OAuth2Authenticator authenticator) {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Application.Current.MainPage.DisplayAlert("Authentication error", e.Message, "OK");
		}
    }
}
