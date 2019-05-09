using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class view_pedirDatosUsuarioRegistro : ContentPage {
        public view_pedirDatosUsuarioRegistro(string email, int fuente) {
            InitializeComponent();

            BindingContext = new viewmodel_pedirDatosUsuarioRegistro(email, fuente);
        }
    }
}