using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class ViewModel_Ingresar {
        #region ICOMMANDS
        public ICommand _btnGoogle { get; set; }
        #endregion

        #region CONSTRUCTOR
        public  ViewModel_Ingresar() {
            _btnGoogle = new Command(IngresarConGoogle);
        }
        #endregion

        #region METODOS
        private void IngresarConGoogle() {
            DependencyService.Get<IToast>().Show("FUNCIONA!");
        }
        #endregion
    }
}
