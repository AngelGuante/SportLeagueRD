using Android.App;
using Android.Content;
using SportLeagueRD.Services;
using SportLeagueRD.View.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginFacebookView),
                          typeof(SportLeagueRD.Droid.Code.Renderers.LoginFacebookRenderer))]
namespace SportLeagueRD.Droid.Code.Renderers {
    class LoginFacebookRenderer : PageRenderer {

        public LoginFacebookRenderer(Context context) : base(context) {
            Activity activity = Context as Activity;
            new FecebookLoginService();
            activity.StartActivity(App.Authenticator.GetUI(activity));
        }
    }
}