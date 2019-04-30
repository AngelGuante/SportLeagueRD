using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class view_splashApp : ContentPage {
        public view_splashApp() {
            InitializeComponent();

            Application.Current.MainPage = new NavigationPage(this);

            LlamarVentanaCalendario();
        }

        //  LUEGO DE 2 SEGUNDOS LLAMA A LA VENTANA DE CALENDARIO
        private void LlamarVentanaCalendario() => Device.StartTimer(TimeSpan.FromSeconds(2), () => {
            Application.Current.MainPage.Navigation.PushAsync(new mdp());
            return false;
        });
    }
}