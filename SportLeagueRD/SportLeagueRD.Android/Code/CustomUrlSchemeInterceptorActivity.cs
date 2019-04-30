using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;

namespace SportLeagueRD.Droid.Code {
    // ESTA CLASE INTERCEPTA LOS DATOS VERIFICADOS POR EL USUARIO PARA PODER DEVOLVERLO A LA APLICACION
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "com.googleusercontent.apps.831340715062-jb6i8t016tdt742ci1toulv091fj4lj4" },
        DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity : Activity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            App.Authenticator.OnPageLoading(uri);

            // PARA CERRAR EL NAVEGADOR LUEGO DE QUE SE AUTENTICA EL USUARIO
            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            Finish();
        }
    }
}