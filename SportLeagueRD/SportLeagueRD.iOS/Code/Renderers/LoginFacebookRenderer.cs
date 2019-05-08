using SportLeagueRD.Services;
using SportLeagueRD.View.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LoginFacebookView),
                          typeof(SportLeagueRD.iOS.Code.Renderers.LoginFacebookRenderer))]
namespace SportLeagueRD.iOS.Code.Renderers {
    class LoginFacebookRenderer : PageRenderer {
        bool done = false;

        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);

            if (done)
                return;
            new FecebookLoginService();
            done = true;
            PresentViewController(App.Authenticator.GetUI(), true, null);
        }
    }
}