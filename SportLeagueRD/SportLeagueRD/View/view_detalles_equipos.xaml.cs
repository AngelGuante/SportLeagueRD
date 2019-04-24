using SportLeagueRD.Model;
using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_detalles_equipos : ContentPage{
		public view_detalles_equipos (model_equipos equipo, int variacion){
			InitializeComponent ();
            
            BindingContext = new viewmodel_detalles_equipos(equipo, variacion);
        }
	}
}