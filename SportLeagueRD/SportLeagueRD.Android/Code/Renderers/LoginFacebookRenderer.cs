using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Newtonsoft.Json;
using SportLeagueRD.Model;
using SportLeagueRD.View.Renderer;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginFacebookView),
                          typeof(SportLeagueRD.Droid.Code.Renderers.LoginFacebookRenderer))]
namespace SportLeagueRD.Droid.Code.Renderers {
    class LoginFacebookRenderer : PageRenderer {
        [Obsolete]
        public LoginFacebookRenderer() {
            var activity = Context as Activity;

            var auth = new OAuth2Authenticator(
                clientId: "732544403828209",
                scope: "",
                authorizeUrl: new Uri(
                    "https://www.facebook.com/v3.3/dialog/oauth"),
                redirectUrl: new Uri(
                    "http://www.facebook.com/connect/login_success.html"));

            auth.Completed += async (sender, eventArgs) => {
                if (eventArgs.IsAuthenticated) {
                    var accessToken =
                        eventArgs.Account.Properties["access_token"].ToString();
                    var profile = await GetFacebookProfileAsync(accessToken);
                    App.LoginFacebookSuccess(profile);
                }
                else {
                    App.LoginFacebookFail();
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }

        async Task<FacebookResponse> GetFacebookProfileAsync(string accessToken) {
            var requestUrl = "https://graph.facebook.com/v3.3/me/?fields=" +
                "name,picture.width(999),cover,age_range,devices,email," +
                "gender,is_verified,birthday,languages,work,website," +
                "religion,location,locale,link,first_name,last_name," +
                "hometown&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            var facebookResponse =
                JsonConvert.DeserializeObject<FacebookResponse>(userJson);
            return facebookResponse;
        }
    }
}