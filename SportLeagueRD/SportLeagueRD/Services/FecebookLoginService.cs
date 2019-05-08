using Newtonsoft.Json;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace SportLeagueRD.Services {
    public class FecebookLoginService {
        #region VARIABLES
        private readonly string ClientId = "732544403828209";
        private readonly string AuthorizeUrl = "https://www.facebook.com/v3.3/dialog/oauth";
        private readonly string RedirectUrl = "http://www.facebook.com/connect/login_success.html";
        private readonly string FBApiVersion = "v3.3";
        #endregion

        #region METODOS
        public FecebookLoginService() {
            OAuth2Authenticator authenticator = new OAuth2Authenticator(
                clientId: ClientId,
                scope: "",
                authorizeUrl: new Uri(AuthorizeUrl),
                redirectUrl: new Uri(RedirectUrl));

            authenticator.Completed += async (sender, eventArgs) => {
                if (eventArgs.IsAuthenticated) {
                    string accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    UserFacebookProfile profile = await GetFacebookProfileAsync(accessToken);
                    LoginFacebookSuccess(profile);
                }
                else
                    LoginFacebookFail();
            };
            App.Authenticator = authenticator;
        }
        #endregion

        #region METODOS
        private async Task<UserFacebookProfile> GetFacebookProfileAsync(string accessToken) {
            string requestUrl = $"https://graph.facebook.com/{FBApiVersion}/me/?fields=" +
                "name,picture.width(999),cover,age_range,devices,email," +
                "gender,is_verified,birthday,languages,work,website," +
                "religion,location,locale,link,first_name,last_name," +
                "hometown&access_token=" + accessToken;
            HttpClient httpClient = new HttpClient();
            string userJson = await httpClient.GetStringAsync(requestUrl);
            return JsonConvert.DeserializeObject<UserFacebookProfile>(userJson);
        }

        private async void LoginFacebookSuccess(UserFacebookProfile profile) {
            await Application.Current.MainPage.Navigation.PopAsync();
            App.page = new view_pedirDatosUsuarioRegistro(profile.Id);
            Application.Current.MainPage = new NavigationPage(App.page);
        }

        private void LoginFacebookFail() => DependencyService.Get<IToast>().Show("Error, Intentelo de nuevo");
        #endregion
    }
}
