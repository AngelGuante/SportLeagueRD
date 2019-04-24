using Rg.Plugins.Popup.Services;
using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_popup_listadoRankingJugador{
        #region VARIABLES
        private string Comprobante = "";
        private string ParameteoBusqueda = "";
        #endregion

        #region PROPIEDADES
        //PROPIEDAD PARA LLENAR LA LISTA.
        public ObservableCollection<model_popup_listadoRankingJugadores> _lista { set; get; }
        #endregion

        #region ICOMMNADS
        //ICOMMAND PARA CUANDO SE SELECCIONE UN ELEMENTO DEL VIEW CELL.
        public ICommand _elementoSeleccionado { get; }
        #endregion

        #region CONSTRUCTOR
        //  PARA LA VENTANA DE VER LOS JUGADORES POR RANKING
        public viewmodel_popup_listadoRankingJugador(string comprobante, string parametroBsuqueda) {
            _elementoSeleccionado = new Command((item) => { ElementoSeleccionadoRanking(item as model_popup_listadoRankingJugadores); });

            Comprobante = comprobante;
            ParameteoBusqueda = parametroBsuqueda;
           
            _lista = new ObservableCollection<model_popup_listadoRankingJugadores>(InicializarLisado("Favoritos De La Gente"));
        }

        //  PARA LA VENTANA DE VOTACIONES DE JUGADORES
        public viewmodel_popup_listadoRankingJugador(string posicion) {
            _elementoSeleccionado = new Command((item) => { ElementoSeleccionadoVotos(item as model_popup_listadoRankingJugadores); });

            List<model_popup_listadoRankingJugadores> lista = InicializarLisado("Votar Como Mi Favorito");

            model_popup_listadoRankingJugadores elemento = (from posicionAVotar in lista where posicionAVotar._id.Equals(posicion) select posicionAVotar).FirstOrDefault();

            _lista = new ObservableCollection<model_popup_listadoRankingJugadores> {
                lista[0],
                elemento
            };
        }
        #endregion

        #region METODS
        //  PARA LA VENTANA DE VER LOS JUGADORES POR RANKING
        private void ElementoSeleccionadoRanking(model_popup_listadoRankingJugadores elementoSeleccionado) {
            MessagingCenter.Send(new Message() { Variable = new object[] {
                    elementoSeleccionado._id,
                    elementoSeleccionado._nombreRank
                }
            }, "Limpiar");
            if(string.IsNullOrWhiteSpace(ParameteoBusqueda))
                App.ServerC.SendMessageAsync($"{Comprobante}-18-0-{elementoSeleccionado._id}");
            else
                App.ServerC.SendMessageAsync($"{Comprobante}-18-0-{ParameteoBusqueda}-{elementoSeleccionado._id}");
            //  CIERRA EL POPUP PAGE AL SELECCIONAR UN ELEMENTO DE LA LISTA 
            PopupNavigation.Instance.PopAsync(true);
        }

        //  PARA LA VENTANA DE VER LOS JUGADORES POR VOTOS
        private void ElementoSeleccionadoVotos(model_popup_listadoRankingJugadores elementoSeleccionado) {
            MessagingCenter.Send(new Message() { Variable = new object[] {
                    elementoSeleccionado._id,
                    elementoSeleccionado._nombreRank
                }
            }, "CargarVotoJugador");
            //  CIERRA EL POPUP PAGE AL SELECCIONAR UN ELEMENTO DE LA LISTA 
            PopupNavigation.Instance.PopAsync(true);
        }

        //  RETORNA UNA LISTA CON TODOS LOS ELEMENTOS POSIBLES DE POSICIONE QUE HAY EN UN EQUIPO
        private List<model_popup_listadoRankingJugadores> InicializarLisado(string descripcion0) {
            List<model_popup_listadoRankingJugadores> listadoRanking = new List<model_popup_listadoRankingJugadores> {
                new model_popup_listadoRankingJugadores() {
                    _nombreRank = descripcion0 + "  ",
                    _id = "0"
                },
                
                new model_popup_listadoRankingJugadores() {
                    _nombreRank = "Mejor Base  ",
                    _id = "1"
                },

                new model_popup_listadoRankingJugadores() {
                    _nombreRank = "Mejor Alero  ",
                    _id = "2"
                },
                new model_popup_listadoRankingJugadores() {
                    _nombreRank = "Mejor Pivot  ",
                    _id = "3"
                },
                new model_popup_listadoRankingJugadores() {
                    _nombreRank = "Mejor Ala-Pivot  ",
                    _id = "4"
                }
            };
            return listadoRanking;
        }
        #endregion
    }
}
