using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using SportLeagueRD.Messages;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class mdp : MasterDetailPage{
		public mdp (){
			InitializeComponent ();

            //ENVIAR LA REFERENCIA DE ESTA CLASE AL VIEWMODEL PARA PODER AGREGAR OTROS DETAILS DESDE AHI.
            BindingContext = new viewmodel_mdp(this);

            //INICIALIZA LA REFERENCIA DE LA PAGINA 1 EN VIWEMODEL_MDP PARA QUE SE REUTILIZE ESTA MISMA REFERENCIA.
            MessagingCenter.Send(new Message { Variable = new object[] {1} }, "cambiarDetail");
        }

        //PARA ELIMINAR LA PAGINA DE REGISTRAR AL INICIAR LA APP SIN USAR UNA CUENTA.
        protected override void OnAppearing() {
            base.OnAppearing();
            if (App.page != null) {
                Application.Current.MainPage.Navigation.RemovePage(App.page);
                App.page = null;
            }
        }
    }
}