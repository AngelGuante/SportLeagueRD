using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SportLeagueRD.Droid {
    [Activity(Label = "SportLeagueRD", Icon = "@mipmap/app_icono", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        protected override void OnCreate(Bundle savedInstanceState) {
            //FFIMAGE
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //OAuth
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            //don't show warning message when closing account selection page
            Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;

            //POPUP
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}