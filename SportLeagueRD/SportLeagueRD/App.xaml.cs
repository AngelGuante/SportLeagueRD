using SportLeagueRD.Model;
using SportLeagueRD.Services;
using SportLeagueRD.View;
using SportLeagueRD.ViewModel;
using System;
using System.IO;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SportLeagueRD {
    public partial class App : Application {
        public static ContentPage page = null;

        #region LOCAL DATABASE VARIABLES
        private static Database database = null;
        public static Entity_usuario usuario = null; 
        #endregion

        #region SERVER CONNECTION VARIABLES
        public static ServerConnection ServerC = null;
        public static string ServerUrl = "10.0.0.8:8080/WebSocket";
        public static string ServerImagesPath = "/Multimedia/Imagenes/";
        #endregion

        #region GOOGLE LOGIN VARIABLES
        public static OAuth2Authenticator Authenticator = null;

        // OAuth
        public static string iOSClientId = "831340715062-ctso0qq9icmeepiam2gg6i7sf0q20nd9.apps.googleusercontent.com";
        public static string AndroidClientId = "831340715062-jb6i8t016tdt742ci1toulv091fj4lj4.apps.googleusercontent.com";

        public static string iOSRedirectUrl = "com.googleusercontent.apps.831340715062-ctso0qq9icmeepiam2gg6i7sf0q20nd9:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.831340715062-jb6i8t016tdt742ci1toulv091fj4lj4:/oauth2redirect"; 
        #endregion

        //  PROPIEDAD PARA CREAR/USAR LA BASE DE DATOS
        public static Database DB {
            get {
                if (database == null)
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SportLeagueSQLite.db3"));
                return database;
            }
        }

        public App(){
            InitializeComponent();

            ServerC = new ServerConnection();

            database = DB;
            InicializarDatosUsuarioLogeadoAsync();
        }

        //  INIIALIZA LA REFERENCIA A LOS DATOS DEL USUARIO, LO HAGO EN UN METODO PORQUE ES AWAIT   
        private async void InicializarDatosUsuarioLogeadoAsync() {
            usuario = await database.GetItemAsync();

            //  VERIFICO SI EL USUARIO SE A LOGEADO, SI ESTA LOGEADO PASA DIRECTAMENTE A LA PANTALLA
            //  PRINCIPAL DE LA APP, EN CASO CONTRARIO SE MUESTRA LA VENTANA DE LOGEO Y REGISTRO.
            if (usuario.Correo.Equals("")) page = new view_acceso();
            else page = new view_splashApp();
        }

        protected override void OnStart(){}

        protected override void OnSleep(){}

        protected override void OnResume(){}
    }
}
