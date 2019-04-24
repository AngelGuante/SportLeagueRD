using Rg.Plugins.Popup.Services;
using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using SportLeagueRD.View.Popups;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace SportLeagueRD.ViewModel{
    class viewmodel_jugadores : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;
        private string Variacion = "-1";


        //VARIABLES PARA REALIZAR CAMBIOS EN LA INTERFAZ LUEGO DE HABER CARGADO
        private string BusquedaDeJugador = "";
        private bool VerTodo = true;

        //ESTAS VARIABLES ALMACENARAN EL NUMERO DE REGISTROS QUE SE VAN A BUSCAR EN EL SERVIDOR Y DESDE DONDE SE EMPEZARA A BUSCAR
        private string CantidadDatosBuscar = "18";
        private string ValorInicial = "0";
        private string ComprobanteEstandar1 = "";
        private string ComprobanteEstandar2 = "";
        private string Comprobante1 = "JG01";
        private string Comprobante2 = "JG02";
        private string Comprobante3 = "JB01";
        private string Comprobante4 = "JB02";

        //VARIABLES PARA GESTIONAR EL TIPO DE LISTA DE RANKING QUE SE VA A VISUALIZAR
        private string NombreListaRanking = "FAVORITOS DE LA GENTE  ";
        private string IdNombreListaRanking = "0";
        #endregion

        #region ICOMMNADS
        //ICOMMAND PARA CUANDO SE SELECCIONE UN ELEMENTO DEL VIEW CELL.
        public ICommand _elementoSeleccionado{
            get { return new Command((item) => { AbrirVentana_detalle(item as model_jugadores); }); }
        }
        public ICommand _btn_tipoBusquedaRanking { get; set; }

        //PROPIEDAD PARA EL BOTON DE BUSCAR LOS JUGADORES POR SU NOMBRE
        public ICommand _btn_buscar { get => new Command(() => {
            if (string.IsNullOrWhiteSpace(_parametroJugadorABuscar))
                return;
            LimpiarAntesDeBuscarJugador(false);
            App.ServerC.SendMessageAsync($"{ComprobanteEstandar2}-{CantidadDatosBuscar}-{ValorInicial}-{_parametroJugadorABuscar}-{IdNombreListaRanking}");
        }); }

        //PROPIEDAD PARA VER TODOS LOS EQUIPOS
        public ICommand _verTodosJugadores { get => new Command(() => {
            _parametroJugadorABuscar = "";
            LimpiarAntesDeBuscarJugador(true);
            App.ServerC.SendMessageAsync($"{ComprobanteEstandar1}-{CantidadDatosBuscar}-{ValorInicial}-0");
        }); }
        #endregion

        #region PROPIEDADES
        public InfiniteScrollCollection<model_jugadores> _lista { set; get; }

        //PROPIEDAD PARA CAMBIAR EL TITULO DE LA LISTA
        public string tipoLista { get => NombreListaRanking;
            set {
                NombreListaRanking = value;
                OnPropertyChanged();
            }
        }

        //PARA ALMACENAR LA BUSQUEDA REALIZADA POR EL USUARIO.
        public string _parametroJugadorABuscar {
            get => BusquedaDeJugador;
            set {
                BusquedaDeJugador = value;
                OnPropertyChanged();
            }
        }

        //ESTA PROPIEDAD INDICARA SI SE MOSTRARAN TODOS LOS EQUIPOS O SOLO LOS QUE ENCAJEN CON LA BUSQUEDA HECHA POR PARAMETRO.
        public bool _verTodo {
            get => VerTodo;
            set {
                VerTodo = value;
                OnPropertyChanged();
            }
        }

        //PROPIEDAD QUE DETERMINA SI ESTA PAGINA ESTA REALIZANDO ALGUN TRABAJO, ASI OCULTARLA DEBAJO DE UUNA PAGINA CON UN ActivityIndicator
        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_jugadores(string variacion){
            Variacion = variacion;
            //CADA VEZ QUE EL USUARIO LLEGE AL PIE DE LA PAGINA SE BUSCARAN MAS DATOS AL SERVIDOR.
            _lista = new InfiniteScrollCollection<model_jugadores>{
                OnLoadMore = async () => {
                    ValorInicial = _lista.Count.ToString();
                    //DEPENDIENDO DE SI SE ESTA VIENDO TODOS LOS EQUIPOS O SOLO LOS EQUIPOS QUE ENCAJEN CON LA BUSQUEDA ESCRITA EN EL CAMPO
                    if (_verTodo)
                        App.ServerC.SendMessageAsync($"{ComprobanteEstandar1}-{CantidadDatosBuscar}-{ValorInicial}-{IdNombreListaRanking}");
                    else
                        App.ServerC.SendMessageAsync($"{ComprobanteEstandar2}-{CantidadDatosBuscar}-{ValorInicial}-{_parametroJugadorABuscar}-{IdNombreListaRanking}");
                    return null;
                }
            };
            _btn_tipoBusquedaRanking = new Command(MC_btn_listadoRankingJugador);
        }
        #endregion

        #region METODOS
        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DEL EVENTO SELECCIONADA.
        private void AbrirVentana_detalle(model_jugadores jugador) => Application.Current.MainPage.Navigation.PushAsync(new view_detalles_jugadores(jugador, Variacion));

        //LLENA LA LISTA DE MANERA ASINCRONA
        private async void LlenarListView(List<model_jugadores> jugadores){
            IsBusy = true;
            await Task.Delay(500);
            await Task.Run(() => {
                _lista.AddRange(jugadores);
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                _lista.RemoveAt(_lista.Count - 1);
            });
            IsBusy = false;
        }

        //ABRIR EL POPUP QUE MUESTRA EL LISTADO DE RANKINGS A MOSTRAR
        private void MC_btn_listadoRankingJugador() => PopupNavigation.Instance.PushAsync(new view_popup_listadoRankingJugadores(string.IsNullOrWhiteSpace(_parametroJugadorABuscar) ? ComprobanteEstandar1 : ComprobanteEstandar2, _parametroJugadorABuscar));

        //LLENA LA TABLA CON LOS PRIMEROS REGISTROS LA PRIMERA VEZ QUE ESTA PAGINA APAREZCA
        public void LlenarTablaPrimeraVez(){
            //SI VARIACION ES IGUAL A 0, SE TRAE EL SOURCE DEL EQUIPO AL QUE PERTENEC EL EQUIPO
            //SI VARIACION ES IGUAL A 1 SE TRAEN LOS VOTOS
            if (_lista.Count == 0){
                ComprobanteEstandar1 = Variacion.Equals("0") ? Comprobante1 : Comprobante2;
                //PARA LAS BUSQUEDAS
                ComprobanteEstandar2 = Variacion.Equals("0") ? Comprobante3 : Comprobante4;
                //EL ULTIMO DIGITO TIENE QUE VER CON EL TIPO DE LISTA QUE SE VA A TRAER PARA EL CASO DE RANKING. PARA EL CASO DE JUGADORES NORMALES SIEMPRE SERA 0
                App.ServerC.SendMessageAsync($"{ComprobanteEstandar1}-{CantidadDatosBuscar}-{ValorInicial}-0");
            }
        }

        //ESTE METODO ES PARA REDUCIR CODIGO DE LOS COMANDOS QUE TIENEN QUE VER CON LA PROPIEDAD '_verTodo'
        private void LimpiarAntesDeBuscarJugador(bool valor) {
            CantidadDatosBuscar = "18";
            ValorInicial = "0";
            _verTodo = valor;
            _lista.Clear();
        }

        //INICIA EL MESAGING CENTER.
        public void StarMessaginCenter(){
            //COMO HAY DOS PAGINAS QUE UTILIZAN ESTA MISMA CLASE PARA MOSTRAR SUS DATOS, HAY QUE SEPARAR LOS MESSAGIN CENTER UTILIZANDO LA VARIABLE "VARIACION" PARA IDENTIFICAR UNO DE OTRO.
            if (Variacion.Equals("0"))
                MessagingCenter.Subscribe<Message>(this, "cargarJugadores", Llamar => { LlenarListView(Llamar.Jugadores); });
            else {
                MessagingCenter.Subscribe<Message>(this, "cargarJugadoresRanking", Llamar => { LlenarListView(Llamar.Jugadores); });
                //LIMPIA LA LISTA Y LLENA LAS PROPIEDADES CORRESPONDIENTES, SE UTILIZA PARA CUANDO SE CAMBIE EL TIPO DE RANKING QUE SE QUIERE VER SE SEPA CUAL SE ESTA REQUIRIENDO Y QUE DATO SE
                //MANDARA AL SERVIDOR PARA HACER LA PETICION.
                MessagingCenter.Subscribe<Message>(this, "Limpiar", Llamar => {
                    tipoLista = ((string)Llamar.Variable[1]).ToUpper();
                    IdNombreListaRanking = (string)Llamar.Variable[0];
                    _lista.Clear();
                });
            }
        }

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        public void StopMessaginCenter() {
            if(Variacion.Equals("0"))
                MessagingCenter.Unsubscribe<Message>(this, "cargarJugadores");
            else
                MessagingCenter.Unsubscribe<Message>(this, "cargarJugadoresRanking");
            MessagingCenter.Unsubscribe<Message>(this, "Limpiar");
        }
        #endregion
    }
}
