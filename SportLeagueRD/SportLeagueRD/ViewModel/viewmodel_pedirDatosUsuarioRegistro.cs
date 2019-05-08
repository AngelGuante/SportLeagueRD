using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class viewmodel_pedirDatosUsuarioRegistro : Base_viewModel{
        private string userEmail = "";

        public string _entryUserName { get; set; } = "";

        public ICommand _btnSiguiente { get; set; }

        public viewmodel_pedirDatosUsuarioRegistro(string userEmail) {
            this.userEmail = userEmail;
            _btnSiguiente = new Command(Prueba);
        }

        //SI EL USUARIO A INGRESADO UN NOMBRE CON MAS DE 2 CARACTERES, ESTE GUARDARA SUS DATOS.
        private async void Prueba() {
            int CaracteresMinimosNombreUsuario = 2;
            if (_entryUserName.Length > CaracteresMinimosNombreUsuario) {
                await App.DB.UpdateItemAsync(new Entity_usuario {
                    ID = 1,
                    Nombre = _entryUserName,
                    Correo = userEmail
                });
                await Application.Current.MainPage.Navigation.PushAsync(new mdp()); 
            } else
                DependencyService.Get<IToast>().Show($"Debe ingresar un nombre de mas de {CaracteresMinimosNombreUsuario} caracteres.");
        }
    }
}
