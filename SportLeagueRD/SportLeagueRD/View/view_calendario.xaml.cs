using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_calendario : ContentPage{
        #region VARIABLES
        private viewmodel_calendario viewmodel = null;
        #endregion

        #region CONSTRUCTOR
        public view_calendario(){
            InitializeComponent();

            viewmodel = new viewmodel_calendario(picker);
            BindingContext = viewmodel;
        }
        #endregion

        #region METODOS
        protected override void OnAppearing(){
            base.OnAppearing();
            //INICIA EL MESDAGINCENTER CUANDO LA VENTANA ESTA VISIBLE
            viewmodel.StarMessaginCenter();
        }

        protected override void OnDisappearing(){
            base.OnDisappearing();
            //DETIENE EL MESDAGINCENTER CUANDO LA VENTANA ESTA INVISIBLE PARA AHORRAR MEMORIA
            viewmodel.StopMessaginCenter();
        }
        #endregion
    }
}