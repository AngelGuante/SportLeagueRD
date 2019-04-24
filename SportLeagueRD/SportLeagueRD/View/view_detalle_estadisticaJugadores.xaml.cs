using SportLeagueRD.Model;
using SportLeagueRD.ViewModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_detalle_estadisticaJugadores : ContentPage{
		public view_detalle_estadisticaJugadores (model_equipos equipo, ObservableCollection<model_jugadores> _jugadores, string estadisticaAVer, string variacion){
			InitializeComponent ();

            BindingContext = new viewmodel_detalle_estadisticas(equipo, _jugadores, estadisticaAVer, variacion);
		}

		public view_detalle_estadisticaJugadores (model_equipos equipo, model_jugadores _jugadores, string estadisticaAVer, string variacion, bool continuar, string tituloPagina){
			InitializeComponent ();

            Title = tituloPagina;

            BindingContext = new viewmodel_detalle_estadisticas(equipo, _jugadores, estadisticaAVer, variacion, continuar);
		}

        public view_detalle_estadisticaJugadores(model_equipos equipo, string estadisticaAVer, string variacion, string tituloPagina){
            InitializeComponent();

            Title = tituloPagina;

            BindingContext = new viewmodel_detalle_estadisticas(equipo, estadisticaAVer, variacion);
        }
    }
}