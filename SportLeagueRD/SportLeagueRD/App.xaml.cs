using SportLeagueRD.Model;
using SportLeagueRD.Services;
using SportLeagueRD.View;
using SportLeagueRD.ViewModel;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SportLeagueRD {
    public partial class App : Application {
        public static view_acceso page = null;

        //  VARIABLES DE LA BASE DE DATOS LOCAL
        private static Database database = null; 
        public static Entity_usuario usuario = null;

        //  VARIABLES DE EL SERVER
        public static ServerConnection ServerC = null;
        public static string ServerUrl = "10.0.0.7:8080/WebSocket";
        public static string ServerImagesPath = "/Multimedia/Imagenes/";

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

            page = new view_acceso();
            ServerC = new ServerConnection();
            MainPage = new NavigationPage(page);

            database = DB;
            InicializarDatosUsuarioLogeadoAsync();
        }

        //  INIIALIZA LA REFERENCIA A LOS DATOS DEL USUARIO, LO HAGO EN UN METODO PORQUE ES AWAIT   
        private static async void InicializarDatosUsuarioLogeadoAsync() => usuario = await database.GetItemAsync();

        protected override void OnStart(){}

        protected override void OnSleep(){}

        protected override void OnResume(){}
    }
}
