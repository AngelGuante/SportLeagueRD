using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_ingresar : ContentPage {
		public view_ingresar () {
			InitializeComponent ();
            BindingContext = new ViewModel_Ingresar();
        }
    }
}