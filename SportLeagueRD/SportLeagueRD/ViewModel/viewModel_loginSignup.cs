using SportLeagueRD.Utilitys;
using SportLeagueRD.View;
using SportLeagueRD.View.Renderer;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class viewModel_loginSignup : Base_viewModel {
        private bool _busy = false;

        #region ICOMMANDS
        public ICommand _btn_siguiente { get; set; }
        public ICommand _btnGoogle { get; set; }
        public ICommand _btnFacebook { get; set; }
        #endregion

        #region PROPIEDADES
        public string _email { get; set; } = "";

        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CONSTRUCTOR
        public viewModel_loginSignup() {
            _btn_siguiente = new Command(IngresarConCorreo);
            _btnGoogle = new Command(IngresarConGoogle);
            _btnFacebook = new Command(IngresarConFacebook);
        }
        #endregion

        #region METODOS
        // ABRE LA VENTANA DE LOGEO CON GOOGLE
        private void IngresarConGoogle() {
            IsBusy = true;
            new Services.GoogleLoginService();
        }
        
        // ABRE LA VENTANA DE LOGEO CON FACEBOOK
        private async void IngresarConFacebook() {
            await Application.Current.MainPage.Navigation.PopAsync();
            App.page = new LoginFacebookView();
            Application.Current.MainPage = new NavigationPage(App.page);
        }

         //  INGRESAR DIRECTAMENTE CON EL CORREO
        private void IngresarConCorreo() {
            if(new IsValidEmail().IsValid(_email)) {
                IsBusy = true;
                LlamarVentana(_email);
            } else 
                DependencyService.Get<IToast>().Show("Debe Ingresar un correo valido");
        }

        //  MANDA AL USUARIO A LA VENTANA DE PEDIR DATOS RESTANTES PARA COMPLETAR EL LOGEO
        private async void LlamarVentana(string usertEmail) {
            await Application.Current.MainPage.Navigation.PopAsync();
            App.page = new view_pedirDatosUsuarioRegistro(usertEmail, 1);
            Application.Current.MainPage = new NavigationPage(App.page);
        }
        #endregion
    }
}
