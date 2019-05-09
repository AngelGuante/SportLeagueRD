using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class viewmodel_pedirDatosUsuarioRegistro : Base_viewModel{
        #region VARIABLES
        private string userEmail = "";
        private readonly string Comprobante = "US01";
        #endregion

        public string _entryUserName { get; set; } = "";

        public ICommand _btnSiguiente { get; set; }

        //  EL CONSTRUCTOR PIDE COMO PARAMETRO 2 DATOS, userEmail ES O EL EMAIL DEL USUARIO EN CASO DE QUE SE REGISTRE CON GOOGLE
        //  O DIRECTAMENTE INGRESANDO SU CORREO, Y EN CASO DE SER CON FB GUARDARA EL ID DE USUARIO.
        //  EL SEGUNDO PARAMETRO ALMACENA EL TIPO DE REGISTRO QUE HIZO, SI FUE POR:
        //  1 - EMAIL
        //  2 - GOOGLE
        //  3 - FACEBOOK
        public viewmodel_pedirDatosUsuarioRegistro(string userEmail, int fuente) {
            this.userEmail = userEmail;
            _btnSiguiente = new Command(Prueba);
        }

        //SI EL USUARIO A INGRESADO UN NOMBRE CON MAS DE 2 CARACTERES, ESTE GUARDARA SUS DATOS.
        private async void Prueba() {
            int CaracteresMinimosNombreUsuario = 2;
            if (_entryUserName.Length > CaracteresMinimosNombreUsuario) {
                //  GUARDARLO EN LA BASE DE DATOS
                await App.DB.UpdateItemAsync(new Entity_usuario {
                    ID = 1,
                    Nombre = _entryUserName,
                    Correo = userEmail
                });
                //  GUARDARLO EN EL SERVIDOR
                App.ServerC.SendMessageAsync($"{Comprobante}-{userEmail}-{_entryUserName}");
                //  Mandar al usuario al servidor
                await Application.Current.MainPage.Navigation.PushAsync(new mdp()); 
            } else
                DependencyService.Get<IToast>().Show($"Debe ingresar un nombre de mas de {CaracteresMinimosNombreUsuario} caracteres.");
        }
    }
}
