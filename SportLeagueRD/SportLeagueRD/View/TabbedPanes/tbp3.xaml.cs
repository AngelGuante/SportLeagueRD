using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace SportLeagueRD.View.TabbedPanes{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tbp3 : Xamarin.Forms.TabbedPage{
		public tbp3 (){
			InitializeComponent ();

            //COLOCAR EL TABBED PANE ABAJO EN ANDROID.
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
	}
}