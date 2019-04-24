using Rg.Plugins.Popup.Pages;
using SportLeagueRD.ViewModel;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View.Popups {
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_popup_listadoRankingJugadores : PopupPage{
		public view_popup_listadoRankingJugadores (string comprobante, string parametroBusqueda){
            BindingContext = new viewmodel_popup_listadoRankingJugador(comprobante, parametroBusqueda);
            InitializeComponent ();
		}

        public view_popup_listadoRankingJugadores(string posicion) {
            BindingContext = new viewmodel_popup_listadoRankingJugador(posicion);
            InitializeComponent();
        }
    }
}