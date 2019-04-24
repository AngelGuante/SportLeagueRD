using SportLeagueRD.Messages;
using SportLeagueRD.View;
using SportLeagueRD.View.TabbedPanes;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_mdp{
        #region VARIABLES
        private MasterDetailPage Mdp;
        //ALMACENARA LAS PAGINAS PARA QUE SOLO SE CARGEN 1 VEZ Y NO CADA VEZ QUE SE SELECCIONE UN ELEMENTO DEL Master
        private NavigationPage pG1 = null;
        private NavigationPage pG2 = null;
        private NavigationPage pG3 = null;
        private NavigationPage pG4 = null;
        #endregion

        #region CONSTRUCTOR
        public viewmodel_mdp(MasterDetailPage mdp){
            Mdp = mdp;

            StarMessaginCenter();
        }
        #endregion

        #region METODOS
        //CAMBIA EL DETAIL PAGE DE EL MASTER DETAIL PAGE SEGUN EL ID PASADO POR PARAMETRO.
        private  async void changeDetail(int numDetail){
            Mdp.IsPresented = false;

            await Task.Delay(160);
            await Task.Run(() => {
                switch (numDetail){
                    case 1:
                        if (pG1 == null) pG1 = new NavigationPage(new view_calendario());
                        Device.BeginInvokeOnMainThread(() => Mdp.Detail = pG1);
                        break;
                    case 2:
                        if (pG2 == null) pG2 = new NavigationPage(new tbp2());
                        Device.BeginInvokeOnMainThread(() => Mdp.Detail = pG2);
                        break;
                    case 3:
                        if (pG3 == null) pG3 = new NavigationPage(new tbp3());
                        Device.BeginInvokeOnMainThread(() => Mdp.Detail = pG3);
                        break;
                    case 4:
                        if (pG4 == null) pG4 = new NavigationPage(new tbp4());
                        Device.BeginInvokeOnMainThread(() => Mdp.Detail = pG4);
                        break;
                }
            });
        }

        //INICIA LOS MESAGING CENTER.
        private void StarMessaginCenter(){
            MessagingCenter.Subscribe<Message>(this, "cambiarDetail", cambiar => {
                changeDetail((int)cambiar.Variable[0]);
            });
        }
        #endregion
    }
}
