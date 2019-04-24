using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace SportLeagueRD.View.TabbedPanes{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tbp4 :  Xamarin.Forms.TabbedPage{
		public tbp4 (){
			InitializeComponent ();

            //COLOCAR EL TABBED PANE ABAJO EN ANDROID.
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
	}
}