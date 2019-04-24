using SportLeagueRD.Model;
using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_detalles_marcador : ContentPage{
        #region VARIABLES
        private viewmodel_detalles_marcador viewmodel = null;
        #endregion

        #region CONSTRUCTOR
        public view_detalles_marcador(model_marcador marcador) {
            InitializeComponent();

            viewmodel = new viewmodel_detalles_marcador(marcador);
            BindingContext = viewmodel;
        }
        #endregion

        #region METODOS
        protected override void OnAppearing() {
            base.OnAppearing();

            viewmodel.LimpiarListasSiTienenDatosDeMas();
        }
        #endregion
    }
}