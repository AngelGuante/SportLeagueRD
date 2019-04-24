using SportLeagueRD.Model;
using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_detalles_jugadores : ContentPage{
		public view_detalles_jugadores (model_jugadores jugador, string variacion){
			InitializeComponent ();

            BindingContext = new viewmodel_detalles_jugadores(jugador, variacion);
		}
	}
}