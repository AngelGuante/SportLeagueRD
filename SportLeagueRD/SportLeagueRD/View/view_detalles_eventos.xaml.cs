using SportLeagueRD.Model;
using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_detalles_eventos : ContentPage{
        #region VARIABLES
        private viewmodel_detalles_eventos viewmodel = null;
        #endregion

        #region CONSTRUCTOR
        public view_detalles_eventos(model_eventos evento){
            InitializeComponent();

            viewmodel = new viewmodel_detalles_eventos(evento);
            BindingContext = viewmodel;
        }
        #endregion
    }
}