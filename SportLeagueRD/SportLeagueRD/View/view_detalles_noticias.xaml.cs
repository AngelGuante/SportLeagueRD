using SportLeagueRD.Model;
using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_detalles_noticias : ContentPage{

        public view_detalles_noticias (model_noticias noticia){
			InitializeComponent ();

            BindingContext = new viewmodel_detalles_noticias(noticia);
        }
    }
}