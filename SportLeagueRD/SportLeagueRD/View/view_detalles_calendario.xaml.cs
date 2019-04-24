using SportLeagueRD.Model;
using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_detalles_calendario : ContentPage{
        #region VARIABLES
        private viewmodel_detalles_calendario viewmodel = null;
        #endregion

        #region CONSTRUCTOR
        public view_detalles_calendario (model_marcador calendario){
			InitializeComponent ();

            viewmodel = new viewmodel_detalles_calendario(calendario, listaComentarios);
            BindingContext = viewmodel;
        }
        #endregion

        #region METODOS
        protected override void OnDisappearing() {
            base.OnDisappearing();
            //DETIENE EL MESDAGINCENTER CUANDO LA VENTANA ESTA INVISIBLE PARA AHORRAR MEMORIA
            viewmodel.StopMessaginCenter(1);
        }
        #endregion
    }
}