using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace SportLeagueRD.View.TabbedPanes{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tbp2 : Xamarin.Forms.TabbedPage{
		public tbp2 (){
			InitializeComponent ();

            //COLOCAR EL TABBED PANE ABAJO EN ANDROID.
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
	}
}