using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class ViewModel_Ingresar : Base_viewModel{
        private bool _busy = false;

        #region ICOMMANDS
        public ICommand _btnGoogle { get; set; }
        #endregion

        #region PROPIEDADES
        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CONSTRUCTOR
        public ViewModel_Ingresar() {
            _btnGoogle = new Command(IngresarConGoogle);
        }
        #endregion

        #region METODOS
        // ABRE LA VENTANA DE LOGEO CON GOOGLE
        private void IngresarConGoogle() {
            new Services.GoogleLoginService();
            IsBusy = true;
        }
        #endregion
    }
}
