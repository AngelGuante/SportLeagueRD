using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class view_loginSignup : ContentPage {
        public view_loginSignup() {
            InitializeComponent();
            BindingContext = new viewModel_loginSignup();
        }
    }
}