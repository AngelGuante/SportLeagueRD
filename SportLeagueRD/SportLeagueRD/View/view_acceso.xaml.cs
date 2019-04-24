using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_acceso : ContentPage{
		public view_acceso (){
			InitializeComponent ();

            BindingContext = new viewmodel_acceso();
		}
	}
}