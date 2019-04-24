using SportLeagueRD.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_ingresar : ContentPage {
		public view_ingresar () {
			InitializeComponent ();
            //BindingContext = new ViewModel_Ingresar();
            googlebtn.Clicked += prueba;
		}

        private void prueba(object sender, EventArgs e) {
        }
    }
}