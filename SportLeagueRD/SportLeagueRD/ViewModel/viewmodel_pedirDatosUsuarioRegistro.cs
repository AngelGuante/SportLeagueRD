using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class viewmodel_pedirDatosUsuarioRegistro : Base_viewModel{
        private string userEmail = "";

        public string _entryUserName { get; set; } = "";

        public ICommand _btnSiguiente {
            get;
            set;
        }

        public viewmodel_pedirDatosUsuarioRegistro(string userEmail) {
            this.userEmail = userEmail;
            _btnSiguiente = new Command(Prueba);
        }

        private async void Prueba() {
            if (_entryUserName.Length > 2) {
                await App.DB.UpdateItemAsync(new Entity_usuario {
                    ID = 1,
                    Nombre = _entryUserName,
                    Correo = userEmail
                });

                await Application.Current.MainPage.Navigation.PushAsync(new mdp()); 
            }
        }
    }
}
