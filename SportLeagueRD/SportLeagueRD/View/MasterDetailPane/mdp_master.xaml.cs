using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View.MasterDetailPane{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class mdp_master : ContentPage{
		public mdp_master (){
			InitializeComponent ();

            BindingContext = new viewmodel_mdp_master();
		}
	}
}