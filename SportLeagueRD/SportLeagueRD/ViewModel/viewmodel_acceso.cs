using SportLeagueRD.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel
{
    class viewmodel_acceso{
        #region ICOMMANDS
        public ICommand _btn_registrar { get; set; }
        public ICommand _btn_ingresar { get; set; }
        public ICommand _btn_sinCuenta { get; set; }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_acceso(){
            _btn_registrar = new Command(MC_btn_registrar);
            _btn_ingresar = new Command(MC_btn_ingresar);
            _btn_sinCuenta = new Command(MC_btn_sinCuentaAsync);
        }
        #endregion

        #region METODOS

        //ABRIR LA PAGINA DE REGISTRAR USUARIO.
        private void MC_btn_registrar() => Application.Current.MainPage.Navigation.PushAsync(new view_registrar());

        //ABRIR LA PAGINA DE LOGEAR USUARIO.
        private void MC_btn_ingresar() => Application.Current.MainPage.Navigation.PushAsync(new view_ingresar());

        //PASAR DIRECTAMENTE A LA PAGINA PRINCIPAL DE LA APLICACION.
        private void MC_btn_sinCuentaAsync() => Application.Current.MainPage.Navigation.PushAsync(new mdp());
        #endregion
    }
}
