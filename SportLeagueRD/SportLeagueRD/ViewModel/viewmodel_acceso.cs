using SportLeagueRD.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class viewmodel_acceso{
        #region ICOMMANDS
        public ICommand _btn_acceder { get; set; }
        public ICommand _btn_sinCuenta { get; set; }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_acceso(){
            _btn_acceder = new Command(MC_btn_acceder);
            _btn_sinCuenta = new Command(MC_btn_sinCuentaAsync);
        }
        #endregion

        #region METODOS
        //ABRIR LA PAGINA DE ACCESO DE USUARIO.
        private void MC_btn_acceder() => Application.Current.MainPage.Navigation.PushAsync(new view_loginSignup());

        //PASAR DIRECTAMENTE A LA PAGINA PRINCIPAL DE LA APLICACION.
        private void MC_btn_sinCuentaAsync() => Application.Current.MainPage.Navigation.PushAsync(new mdp());
        #endregion
    }
}
