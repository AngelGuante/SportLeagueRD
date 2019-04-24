using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_mdp_master{
        #region VARIABLES
        //VARIABLE PARA ALMACENAR EL ITEM SELECCIONADO DE LA LISTA.
        private model_mdp_master _itemSeleccionado;
        //PARA ALMACENAR LA PAGINA QUE SE ABRIRA EN EL DETAIL.
        private Message numero;
        #endregion

        #region PROPIEDADES
        //PROPIEDAD PARA LLENAR LA LISTA.
        public ObservableCollection<model_mdp_master> _lista { set; get; }

        //PROPIEDAD PARA CUANDO SE SELECCIONE UN ITEM.
        public model_mdp_master SelectedItem {
            set{
                if(_itemSeleccionado != value){
                    _itemSeleccionado = value;
                    AbrirPaginaCorrespondiente();
                }
            }
            get{ return _itemSeleccionado; }
        }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_mdp_master(){
            numero = new Message();

            _itemSeleccionado = null;

            _lista = new ObservableCollection<model_mdp_master>(){
                new model_mdp_master(){
                _source = "icon_portada.png",
                _text = "Portada"},

                new model_mdp_master(){
                _source = "icon_NoticiasEventos.png",
                _text = "Noticias/Eventos"},

                new model_mdp_master(){
                _source = "icon_list.png",
                _text = "Equipos/Jugadores"},

                new model_mdp_master(){
                _source = "icon_ranking.png",
                _text = "Ranking"},

                new model_mdp_master(){
                _source = "icon_setting.png",
                _text = "Configuracion"},

                new model_mdp_master(){
                _source = "icon_ads.png",
                _text = "Anunciate Con Nosotros"}
            };
        }
        #endregion

        #region METODOS
        //ABRE LA PAGINA CORRESPONDIENTE SEGUN EL ITEM SELECCIONADO.
        private void AbrirPaginaCorrespondiente(){
            switch(_itemSeleccionado._text){
                case "Portada":
                    numero.Variable = new object[] {1};
                    MessagingCenter.Send(numero, "cambiarDetail");
                    break;
                case "Noticias/Eventos":
                    numero.Variable = new object[] {2};
                    MessagingCenter.Send(numero, "cambiarDetail");
                    break;
                case "Ranking":
                    numero.Variable = new object[] {3};
                    MessagingCenter.Send(numero, "cambiarDetail");
                    break;
                case "Equipos/Jugadores":
                    numero.Variable = new object[] {4};
                    MessagingCenter.Send(numero, "cambiarDetail");
                    break;
                case "Configuracion":
                    Application.Current.MainPage.DisplayAlert("Elemento no Configurado", "Codigo no desplegado", "OK");
                    break;
                case "Anunciate Con Nosotros":
                    Application.Current.MainPage.DisplayAlert("Elemento no Configurado", "Codigo no desplegado", "OK");
                    break;
                default:
                    Application.Current.MainPage.DisplayAlert("Elemento no encontrado", "Verificar Codigo", "OK");
                    break;
            }
        }
        #endregion
    }
}