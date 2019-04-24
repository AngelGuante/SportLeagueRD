using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_detalle_estadisticas : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        private string ItemSeleccionado;
        private bool EstadisticasAMostrar;

        private string variacion;

        //ESTA VARIABLE ES PARA PERMITIR LA NAVEGACION DESDE ESTA PAGINA HASATA "DETALLES_JUGADOR", SI LA VENTANA QUE ABRE ESTA VENTANA ES "DETALLE_JUGADOR", NO SE VOLVERA
        //A MANDAR DE NUEVO A ESTA VENTA DE MANERA INFINITA.
        private bool Continuar = true;

        //ALMACENO EL ID DEL JUGADOR EN CASO DE QUE SE PASE ELOBJETO SE INICIALISE CON EL PRIMER CONSTRUCTOR PERO EN VEZ DE UN CONJUNTO DE JUGADORES SE PASE SOLO UN SOLO JUGADOR
        //ASI ALMACENO SUS DATOS PARA PODER ENVIARLO AL SERVER Y SOLO TRAER LAS ESTADISTICAS DE ESE JUGADOR EN ESPESIFICO.
        private string IdJugador;

        //ALMACENARAN LOS OBJETOS DE LOS JUGADORES PARA NO TENER QUE BUSCAR DOS VECES LOS DATOS GENERALES DEL JUGADOR COMO SU NOMBRE, APELLIDO, ETC,
        //SINO QUE CUANDO DE LLAME OTRO JSON CON LAS ESTADISTICAS FALTANTES SOLO 
        private ObservableCollection<model_jugadores> BackupJugadores = null;
        private model_jugadores BackupJugador = null;

        //ESTA VARIABLE ES PARA VERIFICAR SI YA SE HAN BUSCADO TODAS LAS ESTADISTICAS AL SERVIDOR, DE SER ASI AL MOMENTO DE CAMBIAR DE ELEMENTO EN 
        //EL PICKER, SOLO SE CAMBIA DE DATOS A MOSTRAR PERO NO SE BUSCAN EN LA BASE DE DATOS.
        private bool TodoCargado = true;

        private string Comprobante1 = "EJ02";
        private string Comprobante2 = "EJ03";
        private string Comprobante3 = "EJ04";
        private string Comprobante4 = "EJ05";

        //PARA SABER SI HAY QUE TRAER A LOS JUGADORES DE LA BASE DE DATOS, ESTA VARIABLE SOLO ES PARA EL MARCADOR CUANDO SE QUIEREN VER TODOS LOS JUGADORES.
        private bool TraerJugadores = false;
        #endregion

        #region ICOMMANDS
        public ICommand _elementoSeleccionado{get { return new Command((item) => { MC_label_estatisticaJugador(item as model_jugadores); }); } }
        #endregion

        #region PROPIEDADES
        #region **PROPIEDADES DEL MODEL JUGADOR
        public ImageSource _source { set; get; }
        #endregion

        #region **PROPIEDADES DE MODEL EQUIPO
        public string _idEquipo { set; get; }
        public ImageSource _sourceEquipo { set; get; }
        public string _nombreEquipo { set; get; }
        public string _siglas { set; get; }
        #endregion

        #region GENERAL
        public ObservableCollection<model_jugadores> _jugadores { set; get; } = null;

        //VERIFICA CUAL DE LAS DOS ESTADISTICAS SE VAN A MOSTRAR, SI LA DE ESTADISTICAS GENERALES O LAS DE TIRO.
        public bool _estadisticaAMostrar {
            get => EstadisticasAMostrar;
            set {
                EstadisticasAMostrar = value;
                OnPropertyChanged();
            }
        }

        //ESTA VARIABLE ES POR EL SIGUIENTE MOTIVO: CUANDO LA PAGINA SE CARGABA, SE ENVIABAN 2 PETICIONES AL SERVER AL MISMO TIEMPO, LA MISMA PETICION DOS VECES,
        //PARA EVITAR ESTO SE CREO UNA ESPECIA DE SINGLELTON, PARA QUE LA PRIMERA VEZ QUE ENTRE ESTA VARIABLE SE PONGA EN FALSE, ASI NO VELVE A ENVIARSE OTRA 
        //PETICION HASTA QUE LA PRIMERA TERMINE.
        private bool puedeHacerPeticionAlServer = true;

        //PARA DETECTAR CADA VEZ QUE EL USUARIO SELECCIONE UNA OPCION DE PICKER DIFERENTE.
        public string _itemSeleccionado {
            get => ItemSeleccionado;
            set {
                ItemSeleccionado = value;

                _estadisticaAMostrar = ItemSeleccionado == "General" ? true : false;

                if (!puedeHacerPeticionAlServer)
                    return;

                puedeHacerPeticionAlServer = false;

                if (TodoCargado){
                    StarMessaginCenter();
                    //VERIFICAR SI LOS JUGADORES ESTAN TRAIDOS O NO
                    if (!TraerJugadores) {
                        string Comprobante = ItemSeleccionado.Equals("General") ? Comprobante1 : Comprobante2;
                        if (IdJugador != null)
                            App.ServerC.SendMessageAsync($"{Comprobante}-{_idEquipo}-{IdJugador}");
                        else
                            App.ServerC.SendMessageAsync($"{Comprobante}-{_idEquipo}");
                    } else {
                        string Comprobante = ItemSeleccionado.Equals("General") ? Comprobante3 : Comprobante4;
                        App.ServerC.SendMessageAsync($"{Comprobante}-{_idEquipo}");
                    }
                }
            }
        }

        //PROPIEDAD QUE DETERMINA SI ESTA PAGINA ESTA REALIZANDO ALGUN TRABAJO, ASI OCULTARLA DEBAJO DE UNA PAGINA CON UN ActivityIndicator
        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region CONSTRUCTOR
        //PARA RECIBIR UN CONJUNTO DE JUGADORES
        public viewmodel_detalle_estadisticas(model_equipos equipo, ObservableCollection<model_jugadores> jugadores, string datosAMostrar, string variacion){
            //SI EN VES DE ENVIARSE UNA LISTA DE JUGADORES, SOLO SE ENVIA UNO SOLO, SE ALMACENA EL ID DE ESTE PARA TRAER DEL SERVER SOLAMENTE LAS ESTADISTICAS DE ESTE
            IdJugador = jugadores.Count == 1 ? jugadores[0]._idJugador : null;
            Inicializaciones(equipo, datosAMostrar, variacion);

            BackupJugadores = jugadores;
        }

        //PARA RECIBIR UN UNICO JUGAODR
        public viewmodel_detalle_estadisticas(model_equipos equipo, model_jugadores jugador, string datosAMostrar, string variacion, bool continuar){
            IdJugador = jugador._idJugador;
            Inicializaciones(equipo, datosAMostrar, variacion);

            Continuar = continuar;
            BackupJugador = jugador;
        }

        //SI NO SE RECIBE NINGUN JUGADOR Y HAY QUE BUSCARLO DESDE ESTA PAGINA
        public viewmodel_detalle_estadisticas(model_equipos equipo, string datosAMostrar, string variacion){
            TraerJugadores = true;
            Inicializaciones(equipo, datosAMostrar, variacion);
        }
        #endregion

        #region METODOS
        /**/
        /*SE CREA SOBRECARGA DE ESTE METODO PORQUE ESTA PAGINA SE PUEDE LLAMAR DESDE VARIAS PARTES Y UNA PUEDE PASAR
         UN OBSERVABLECOLLECTION Y EL OTRO UN UNICO OBJETO DE LOS MISMOS DATOS*/
        /**/
        //METODO QUE CARGA LOS DATOS EN LA TABLA
        private async void LlenarTabla(ObservableCollection<model_jugadores> jugadores, List<model_jugadores> jugadoresEstadisticas) {
            await Task.Delay(500);
            await Task.Run(() => {
                int i = 0;
                //SI _itemSeleccionado ES IGUAL A GENERAL SE TRAERAN LAS ESTADISTICAS GENERALES
                if (_itemSeleccionado.Equals("General")){
                    foreach (model_jugadores elementos in jugadores){
                        elementos._MJ = jugadoresEstadisticas[i]._MJ;
                        elementos._JJ = jugadoresEstadisticas[i]._JJ;
                        elementos._RB = jugadoresEstadisticas[i]._RB;
                        elementos._A = jugadoresEstadisticas[i]._A;
                        elementos._RO = jugadoresEstadisticas[i]._RO;
                        elementos._F = jugadoresEstadisticas[i]._F;
                        elementos._BL = jugadoresEstadisticas[i]._BL;
                        elementos._BP = jugadoresEstadisticas[i]._BP;
                        i++; 

                        _jugadores.Add(elementos);
                    }
                }
                //SI _itemSeleccionado ES IGUAL A TIROS SE TRAERAN LAS ESTADISTICAS DE TIRO
                else{
                    foreach (model_jugadores elementos in jugadores){
                        elementos._T2H = jugadoresEstadisticas[i]._T2H;
                        elementos._T2F = jugadoresEstadisticas[i]._T2F;
                        elementos._T3H = jugadoresEstadisticas[i]._T3H;
                        elementos._T3F = jugadoresEstadisticas[i]._T3F;
                        elementos._TLH = jugadoresEstadisticas[i]._TLH;
                        elementos._TLF = jugadoresEstadisticas[i]._TLF;
                        i++;

                        _jugadores.Add(elementos);
                    }
                }
                IsBusy = false;
                StopMessaginCenter();
            });
        }

        //LUEGO DE CARGAR LAS ESTADISTICAS CON LA QUE SE ABRIO LA PAGINA (GENERAL O TIRO), SI EL USUARIO CAMBIA EL VALOR DEL PICKER PARA VER LAS OTRAS ESTADISTICAS
        //RESTANTES (POR EJEMPLO SI ABRIO LA PAGINA CON LAS ESTADISTICAS DE TIRO, FALTARIAN LAS GENERALES, PARA VER LAS GENERALES HABRIA QUE CAMBIAR EL PICKER),
        //ESTE METODO CARGA ESAS ESTADISTICAS RESTANTES, PARA QUE NO SE CARGEN DATOS INECESARIOS.
        private async void CargarEstadisticasRestantes(ObservableCollection<model_jugadores> jugadores, List<model_jugadores> jugadoresEstadisticas) {
            await Task.Run(() => {
                TodoCargado = false;
                int i = 0;
                //SE CARGARAN LAS NUEVAS ESTADISTICAS DEPENDIENDO DE EL NUEVO ITEM SELECCIONADO
                switch (ItemSeleccionado){
                    case "General":
                        foreach (model_jugadores elementos in jugadores){
                            _jugadores[i]._MJ = jugadoresEstadisticas[i]._MJ;
                            _jugadores[i]._JJ = jugadoresEstadisticas[i]._JJ;
                            _jugadores[i]._RB = jugadoresEstadisticas[i]._RB;
                            _jugadores[i]._A = jugadoresEstadisticas[i]._A;
                            _jugadores[i]._RO = jugadoresEstadisticas[i]._RO;
                            _jugadores[i]._F = jugadoresEstadisticas[i]._F;
                            _jugadores[i]._BL = jugadoresEstadisticas[i]._BL;
                            _jugadores[i]._BP = jugadoresEstadisticas[i]._BP; i++;
                        }
                        break;
                    case "Tiro":
                        foreach (model_jugadores elementos in jugadores){
                            _jugadores[i]._T2H = jugadoresEstadisticas[i]._T2H;
                            _jugadores[i]._T2F = jugadoresEstadisticas[i]._T2F;
                            _jugadores[i]._T3H = jugadoresEstadisticas[i]._T3H;
                            _jugadores[i]._T3F = jugadoresEstadisticas[i]._T3F;
                            _jugadores[i]._TLH = jugadoresEstadisticas[i]._TLH;
                            _jugadores[i]._TLF = jugadoresEstadisticas[i]._TLF;
                            i++;
                        }
                        break;
                }
                StopMessaginCenter();
            });
        }

        //METODO QUE CARGA LOS DATOS EN LA TABLA
        private async void LlenarTabla(model_jugadores jugadores, List<model_jugadores> jugadoresEstadisticas) {
            await Task.Delay(500);
            await Task.Run(() => {
                //SI _itemSeleccionado ES IGUAL A GENERAL SE TRAERAN LAS ESTADISTICAS GENERALES
                if (_itemSeleccionado.Equals("General")){
                    _jugadores.Add(new model_jugadores{
                        _idJugador = jugadores._idJugador,
                        _nombreJugador = jugadores._nombreJugador,
                        _apellidoJugador = jugadores._apellidoJugador,
                        _posicion = jugadores._posicion,
                        _MJ = jugadoresEstadisticas[0]._MJ,
                        _JJ = jugadoresEstadisticas[0]._JJ,
                        _RB = jugadoresEstadisticas[0]._RB,
                        _A = jugadoresEstadisticas[0]._A,
                        _RO = jugadoresEstadisticas[0]._RO,
                        _F = jugadoresEstadisticas[0]._F,
                        _BL = jugadoresEstadisticas[0]._BL,
                        _BP = jugadoresEstadisticas[0]._BP
                    });
                }
                //SI _itemSeleccionado ES IGUAL A TIROS SE TRAERAN LAS ESTADISTICAS DE TIRO
                else{
                    _jugadores.Add(new model_jugadores{
                        _idJugador = jugadores._idJugador,
                        _nombreJugador = jugadores._nombreJugador,
                        _apellidoJugador = jugadores._apellidoJugador,
                        _posicion = jugadores._posicion,
                        _T2H = jugadoresEstadisticas[0]._T2H,
                        _T2F = jugadoresEstadisticas[0]._T2F,
                        _T3H = jugadoresEstadisticas[0]._T3H,
                        _T3F = jugadoresEstadisticas[0]._T3F,
                        _TLH = jugadoresEstadisticas[0]._TLH,
                        _TLF = jugadoresEstadisticas[0]._TLF
                    });
                }
                IsBusy = false;
                StopMessaginCenter();
            });
        }
        /**/

        //ESTE METODO BUSCA LOS JUGADORES DE UN EQUIPO EN CASO DE QUE NO VENGAN DE LA VENTANA ANTERIOR Y JUNTO A ELLOS TAMBIEN TRAE LAS ESTADISTICAS REQUERIDAS PARA NO IR 2 VECES AL SERVIDOR.
        private void BuscarJugadoresConEstadisticas(List<model_jugadores> jugadoresEstadisticas) {
            ObservableCollection<model_jugadores> jugadores = new ObservableCollection<model_jugadores>();
            foreach(model_jugadores tmp in jugadoresEstadisticas) {
                jugadores.Add(new model_jugadores {
                    _idJugador = tmp._idJugador,
                    _nombreJugador = tmp._nombreJugador,
                    _apellidoJugador = tmp._apellidoJugador,
                    _posicion = tmp._posicion
                });
            }
            BackupJugadores = jugadores;
            LlenarTabla(jugadores, jugadoresEstadisticas);
        }

        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DEL JUGADOR SELECCIONADO.
        private void MC_label_estatisticaJugador(model_jugadores jugador){
            if (Continuar){
                /*SI BackupJugador ESTA VACIO, EL SOURCE DEL JUGADOR NO SE HABRA INICIALIZADO, POR ESO SE PONE "" COMO VALOR PARA QUE LA VENTANA QUE SE HABRA BUSQUE 
                    EL SOURCE CORRESPONDIENTE EVALUANDO SI EL CAMPO _source ESTA "".*/
                _source = BackupJugador != null ? BackupJugador._source : "";
                Application.Current.MainPage.Navigation.PushAsync(
                    new view_detalles_jugadores(new model_jugadores{
                    //DATOS DEL JUGADOR
                        _idJugador = jugador._idJugador,
                        _source = _source,
                        _nombreJugador = jugador._nombreJugador,
                        _apellidoJugador = jugador._apellidoJugador,
                        _posicion = jugador._posicion,
                        _numero = jugador._numero != null ? jugador._numero : "-",
                    //DATOS DEL EQUIPO
                        _idEquipo = _idEquipo,
                        _sourceEquipo = _sourceEquipo,
                        _nombreEquipo = _nombreEquipo,
                        _siglasEquipo = _siglas
                    }, variacion)); 
            }
        }

        //LLENA LAS PROPIEDADES NECESARIAS DE ESTA PAGINA.
        private void Inicializaciones(model_equipos equipo, string datosAMostrar, string variacion){
            #region INICIALIZAR PROPIEDSADES DEL MODEL EQUIPO QUE VIENEN DE LA VENTANA ANTERIOR
            _idEquipo = equipo._id;
            _sourceEquipo = equipo._source;
            _nombreEquipo = equipo._nombreEquipo;
            _siglas = equipo._siglas;
            #endregion

            this.variacion = variacion;

            _jugadores = new ObservableCollection<model_jugadores>();

            _itemSeleccionado = datosAMostrar;
        }

        //INICIA EL MESAGING CENTER.
        private void StarMessaginCenter() => MessagingCenter.Subscribe<Message>(this, "cargarEstadisticas", Llamar => {
                if (_jugadores.Count == 0){
                    if (BackupJugadores != null)
                        LlenarTabla(BackupJugadores, Llamar.Jugadores);
                    else if (BackupJugador != null)
                        LlenarTabla(BackupJugador, Llamar.Jugadores);
                    //PARA CARGAR LOS DATOS CUANDO NO SE TRAIGAN LOS JUGADORES DE LA VENTANA ANTERIOR Y HAYA QUE TRAERLOS DESDE EL SERVER
                    else if(BackupJugadores == null)
                        BuscarJugadoresConEstadisticas( Llamar.Jugadores);
                }
                else
                    if (BackupJugadores != null)
                    CargarEstadisticasRestantes(BackupJugadores, Llamar.Jugadores);
                else if (BackupJugador != null)
                    CargarEstadisticasRestantes(new ObservableCollection<model_jugadores> { BackupJugador }, Llamar.Jugadores);
            }); 

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        private void StopMessaginCenter() {
            MessagingCenter.Unsubscribe<Message>(this, "cargarEstadisticas");
            puedeHacerPeticionAlServer = true;
        }
        #endregion
    }
}
