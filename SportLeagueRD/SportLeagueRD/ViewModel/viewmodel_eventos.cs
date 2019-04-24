using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace SportLeagueRD.ViewModel{
    class viewmodel_eventos : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        //ESTAS VARIABLES ALMACENARAN EL NUMERO DE REGISTROS QUE SE VAN A BUSCAR EN EL SERVIDOR Y DESDE DONDE SE EMPEZARA A BUSCAR
        private string CantidadDatosBuscar = "18";
        private string ValorInicial = "0";
        private string Comprobante = "EV01";
        #endregion

        #region ICOMMNADS
        //ICOMMAND PARA CUANDO SE SELECCIONE UN ELEMENTO DEL VIEW CELL.
        public ICommand _elementoSeleccionado { get => new Command((item) => { AbrirVentana_detalle(item as model_eventos); }); }
        #endregion

        #region PROPIEDADES
        public InfiniteScrollCollection<model_eventos> _lista { set; get; }

        //PROPIEDAD QUE DETERMINA SI ESTA PAGINA ESTA REALIZANDO ALGUN TRABAJO, ASI OCULTARLA DEBAJO DE UUNA PAGINA CON UN ActivityIndicator
        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_eventos(){
            //CADA VEZ QUE EL USUARIO LLEGE AL PIE DE LA PAGINA SE BUSCARAN MAS DATOS AL SERVIDOR.
            _lista = new InfiniteScrollCollection<model_eventos>{
                OnLoadMore = async () => {
                    ValorInicial = _lista.Count.ToString();
                    App.ServerC.SendMessageAsync($"{Comprobante}-{CantidadDatosBuscar}-{ValorInicial}");
                    return null;
                }
            };
        }
        #endregion

        #region METODOS
        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DE LA EVENTO SELECCIONADA.
        private void AbrirVentana_detalle(model_eventos evento) => Application.Current.MainPage.Navigation.PushAsync(new view_detalles_eventos(evento));
        
        //LLENA LA LISTA DE MANERA ASINCRONA
        private async void LlenarListView(List<model_eventos> eventos){
            IsBusy = true;
            await Task.Delay(500);
            await Task.Run(() => {
                _lista.AddRange(eventos);
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                _lista.RemoveAt(_lista.Count - 1);
            });
            IsBusy = false;
        }

        //LLENA LA TABLA CON LOS PRIMEROS REGISTROS LA PRIMERA VEZ QUE ESTA PAGINA APAREZCA
        public void LlenarTablaPrimeraVez(){
            if (_lista.Count == 0)
                App.ServerC.SendMessageAsync($"{Comprobante}-{CantidadDatosBuscar}-{ValorInicial}");
        }

        //INICIA EL MESAGING CENTER.
        public void StarMessaginCenter() =>
            MessagingCenter.Subscribe<Message>(this, "cargarEventos", Llamar => {
                LlenarListView(Llamar.Eventos);
            });

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        public void StopMessaginCenter() => MessagingCenter.Unsubscribe<Message>(this, "cargarEventos");
        #endregion
    }
}
