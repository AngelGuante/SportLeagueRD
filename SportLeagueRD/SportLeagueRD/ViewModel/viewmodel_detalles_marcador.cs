using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_detalles_marcador : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        private bool SwicherValue;

        private string Comprobante1 = "PM01";
        private string Comprobante2 = "LM01";
        private string Comprobante3 = "EQ05";

        //PROPIEDADES QUE EN CIERTOS CASOS NECESITARAN ACTUALIZARCE LUEGO DE HABERCE CARGADO LA PAGINA.
        private ImageSource SourceB;
        private string SiglasB;

        //ESTA VARIEBLE ES PARA EVITAR UN ERROR DE QUE SE ENVIABA AL SERVER 2 VECES EL MISMO MENSAGE ASI QUE CREE ESTA VARIABLE QUE EVITA QUE LUEGO DE LA PRIMERA VEZ QUE SE ENVIA
        //AL SERVER LA PETICION DE LOS PUNTOS SE VUELVA A ENVIAR DE NUEVO COLOCANDOLA FALSE.
        bool PuedeSolicitarPuntos = true;
        #endregion

        #region PROPIEDADES
        //DEMAS DATOS
        public string _id { set; get; }
        public string _estado { set; get; }
        public string _fecha { set; get; }
        //EQUIPO A
        public string _idA;
        public ImageSource _sourceA { set; get; }
        public string _nomnbreEquipoA { set; get; }
        public string _siglasA { set; get; }
        public string _puntuacionEquipoA { set; get; }
        //EQUIPO B
        public string _idB;
        public ImageSource _sourceB {
            get => SourceB;
            set {
                SourceB = value;
                OnPropertyChanged();
            }
        }
        public string _nomnbreEquipoB { set; get; }
        public string _siglasB {
            get => SiglasB;
            set {
                SiglasB = value;
                OnPropertyChanged();
            }
        }
        public string _puntuacionEquipoB { set; get; }
        //SI EL SWITCHER ESTA EN FALSE SE MOSTRARA LOS DATOS DEL EQUIPO A Y SI ES TRUE LOS DEL B
        public bool _switcherValue {
            get => SwicherValue;
            set {
                SwicherValue = value;
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

        #region LISTAS
        public ObservableCollection<model_lideres> _lista_lideresPuntosA { get; set; }
        public ObservableCollection<model_lideres> _lista_lideresRebotesA { get; set; }
        public ObservableCollection<model_lideres> _lista_lideresAsistenciasA { get; set; }
        public ObservableCollection<model_lideres> _lista_lideresPuntosB { get; set; }
        public ObservableCollection<model_lideres> _lista_lideresRebotesB { get; set; }
        public ObservableCollection<model_lideres> _lista_lideresAsistenciasB { get; set; }
        public ObservableCollection<model_lideres> _lista_loMasDestacadoA { get; set; }
        public ObservableCollection<model_lideres> _lista_loMasDestacadoB { get; set; }

        public ObservableCollection<model_puntos_bascketball> _puntuaciones { get; set; }
        #endregion
        #endregion

        #region ICOMMANDS
        public ICommand _label_verTodo_tiro { get; set; }
        public ICommand _label_verTodo_general { get; set; }
        public ICommand _label_siglasA { get; set; }
        public ICommand _label_siglasB { get; set; }

        //ICOMMAND PARA CUANDO SE SELECCIONE UN ELEMENTO DEL VIEW CELL.
        public ICommand _elementoSeleccionado_puntos { get => new Command((item) => { AbrirVentana_detalle(item as model_lideres, "Tiro"); }); }
        public ICommand _elementoSeleccionado_general{ get => new Command((item) => { AbrirVentana_detalle(item as model_lideres, "General"); }); }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_detalles_marcador(model_marcador marcador){
            #region INICIALIZAR LISTAS
            _lista_lideresPuntosA = new ObservableCollection<model_lideres>();
            _lista_lideresRebotesA = new ObservableCollection<model_lideres>();
            _lista_lideresAsistenciasA = new ObservableCollection<model_lideres>();
            _lista_lideresPuntosB = new ObservableCollection<model_lideres>();
            _lista_lideresRebotesB = new ObservableCollection<model_lideres>();
            _lista_lideresAsistenciasB = new ObservableCollection<model_lideres>();
            _lista_loMasDestacadoA = new ObservableCollection<model_lideres>();
            _lista_loMasDestacadoB = new ObservableCollection<model_lideres>();
            _puntuaciones = new ObservableCollection<model_puntos_bascketball>();
            #endregion

            #region INICIALIZAR COMANDOS
            _label_verTodo_tiro = new Command(()=> MC_label_verTodo_estadisticas("Tiro"));
            _label_verTodo_general = new Command(() => MC_label_verTodo_estadisticas("General"));
            _label_siglasA = new Command(() => MC_labels_abrirDetallesEquipos("A")); 
            _label_siglasB = new Command(() => MC_labels_abrirDetallesEquipos("B"));
            #endregion

            #region INICIALIZAR PROPIEDSADES DEL MODEL EQUIPO QUE VIENEN DE LA VENTANA ANTERIOR
            //DATOS GENERALES
            _id = marcador._id;
            _estado = marcador._estado;
            //EQUIPO A
            _idA = marcador._idA;
            _sourceA = marcador._sourceA;
            _nomnbreEquipoA = marcador._nomnbreEquipoA;
            _puntuacionEquipoA = marcador._puntuacionEquipoA;
            _siglasA = marcador._siglasA;
            //EQUIPO B
            _idB = marcador._idB;
            _sourceB = marcador._sourceB;
            _nomnbreEquipoB = marcador._nomnbreEquipoB;
            _puntuacionEquipoB = marcador._puntuacionEquipoB;
            _siglasB = marcador._siglasB;
            #endregion

            _switcherValue = marcador._estado == "4" ? false : true;

            _id = marcador._id;
            StarMessaginCenter(1);
            App.ServerC.SendMessageAsync($"{Comprobante1}-{_id}");
        }
        #endregion

        #region METODOS
        //CARGA LA TABLA DE PUNTUACION DE LOS EQUIPOS
        private async void Async_llenarPuntos(List<model_puntos_bascketball> puntos) {
            await Task.Delay(500);
            await Task.Run(() => {
                int i = 1;
                foreach(model_puntos_bascketball tmp in puntos) {
                    //COMO EL LIST TRAE EN EL ULTIMO REGISTRO EL COMPROBANTE EL ULTIMO REGISTRO ESTA VACIO PARA LA LISTA DE PUNTOS, PERO SI LO ELIMINO LA PRIMERA VEZ SE EJECUTA BIEN Y LE  SEGUNDA DA ERROR
                    //ASI QUE LO QUE HAGO ES VERIFICAR EL EL REGISTRO CONTENGA LOS DATOS NECESARIOS Y SI NO NO LOS AGREGO AL LIST ASI EVITO MOSTRAR UN REGISTRO VACIO.
                    if (tmp._puntos_EquipoA == null)
                        continue;
                    _puntuaciones.Add(new model_puntos_bascketball {
                        _cuarto = i++.ToString(),
                        _puntos_EquipoA = tmp._puntos_EquipoA,
                        _puntos_EquipoB = tmp._puntos_EquipoB
                    });
                }
                StopMessaginCenter(1);
                StarMessaginCenter(2);
                if (PuedeSolicitarPuntos) {
                    PuedeSolicitarPuntos = false;
                    App.ServerC.SendMessageAsync($"{Comprobante2}-{_id}");
                }
            });
        }

        //CARGA LOS LIDERES DEL PARTIDO
        private async void Async_llenarListas(List<model_lideres> lideres) {
            await Task.Run(() => {
                for(int i = 0; i < 2; i++) {
                    _lista_lideresPuntosA.Add(new model_lideres {
                            _idJugadorA = lideres[i]._idJugadorA,
                            _sourceJugadorA = lideres[i]._sourceJugadorA,
                            _nombreJugadorA = lideres[i]._nombreJugadorA,
                            _apellidoJugadorA = lideres[i]._apellidoJugadorA,
                            _posicionJugadorA = lideres[i]._posicionJugadorA,
                            _totalJugadorA = lideres[i]._totalJugadorA
                        });
                    _lista_lideresPuntosB.Add(new model_lideres {
                            _idJugadorB = lideres[i]._idJugadorB,
                            _sourceJugadorB = lideres[i]._sourceJugadorB,
                            _nombreJugadorB = lideres[i]._nombreJugadorB,
                            _apellidoJugadorB = lideres[i]._apellidoJugadorB,
                            _posicionJugadorB = lideres[i]._posicionJugadorB,
                            _totalJugadorB = lideres[i]._totalJugadorB
                        });

                    _lista_lideresRebotesA.Add(new model_lideres {
                            _idJugadorA = lideres[i + 2]._idJugadorA,
                            _sourceJugadorA = lideres[i + 2]._sourceJugadorA,
                            _nombreJugadorA = lideres[i + 2]._nombreJugadorA,
                            _apellidoJugadorA = lideres[i + 2]._apellidoJugadorA,
                            _posicionJugadorA = lideres[i + 2]._posicionJugadorA,
                            _totalJugadorA = lideres[i + 2]._totalJugadorA
                        });
                    _lista_lideresRebotesB.Add(new model_lideres {
                            _idJugadorB = lideres[i + 2]._idJugadorB,
                            _sourceJugadorB = lideres[i + 2]._sourceJugadorB,
                            _nombreJugadorB = lideres[i + 2]._nombreJugadorB,
                            _apellidoJugadorB = lideres[i + 2]._apellidoJugadorB,
                            _posicionJugadorB = lideres[i + 2]._posicionJugadorB,
                            _totalJugadorB = lideres[i + 2]._totalJugadorB
                        });

                    _lista_lideresAsistenciasA.Add(new model_lideres {
                            _idJugadorA = lideres[i + 4]._idJugadorA,
                            _sourceJugadorA = lideres[i + 4]._sourceJugadorA,
                            _nombreJugadorA = lideres[i + 4]._nombreJugadorA,
                            _apellidoJugadorA = lideres[i + 4]._apellidoJugadorA,
                            _posicionJugadorA = lideres[i + 4]._posicionJugadorA,
                            _totalJugadorA = lideres[i + 4]._totalJugadorA
                        });
                    _lista_lideresAsistenciasB.Add(new model_lideres {
                            _idJugadorB = lideres[i + 4]._idJugadorB,
                            _sourceJugadorB = lideres[i + 4]._sourceJugadorB,
                            _nombreJugadorB = lideres[i + 4]._nombreJugadorB,
                            _apellidoJugadorB = lideres[i + 4]._apellidoJugadorB,
                            _posicionJugadorB = lideres[i + 4]._posicionJugadorB,
                            _totalJugadorB = lideres[i + 4]._totalJugadorB
                        });

                     _lista_loMasDestacadoA.Add(new model_lideres {
                         _loMasDestacadoA = lideres[i + 6]._loMasDestacadoA
                     });
                     _lista_loMasDestacadoB.Add(new model_lideres {
                         _loMasDestacadoB = lideres[i + 6]._loMasDestacadoB
                     });
                }
                StopMessaginCenter(2);
                //SI AL MOMENTO DE CARGARSE LA PAGINA FALTARON ALGUNOS DATOS, SE HACE OTRA PETICION AL SERVER PARA TRAERLOS.
                if(_sourceB == null && _siglasB == null) {
                    StarMessaginCenter(3);
                    App.ServerC.SendMessageAsync($"{Comprobante3}-{_idB}");
                }else{
                    IsBusy = false;
                }
            });
        }

        //TRAE LOS DATOS DEL EQUIPO B FALTANTES.
        private async void Async_cargarDatosFaltantes(List<model_equipos> equipoB) {
            await Task.Run(() => {
                _sourceB = equipoB[0]._source;
                _siglasB = equipoB[0]._siglas;

                StopMessaginCenter(3);
                IsBusy = false;
            });
        }

        //ABRE LA VENTANA DE ESTADISTICAS DE TIRO DEL EQUIPO EN CUESTION.
        private void MC_label_verTodo_estadisticas(string estadisticaAVer){
            //SI EL SWICHER DE ESTA APAGADO, OSEA PARA EL LADO DEL EQUIPO A, SE ABRE LA VENTANA CON LOS DATOS DEL EQUIPO A.
            if (!_switcherValue){
                Application.Current.MainPage.Navigation.PushAsync(new view_detalle_estadisticaJugadores(
                    new model_equipos{
                        _id = _idA,
                        _source = _sourceA,
                        _nombreEquipo = _nomnbreEquipoA,
                        _siglas = _siglasA
                    }, estadisticaAVer, "2", "@" + _siglasA + "  VS  @" + _siglasB));
            }
            //SI EL SWICHER DE ESTA ENCENDIDO, OSEA PARA EL LADO DEL EQUIPO B, SE ABRE LA VENTANA CON LOS DATOS DEL EQUIPO B.
            else{
                Application.Current.MainPage.Navigation.PushAsync(new view_detalle_estadisticaJugadores(
                    new model_equipos{
                        _id = _idB,
                        _source = _sourceB,
                        _nombreEquipo = _nomnbreEquipoB,
                        _siglas = _siglasB
                    }, estadisticaAVer, "2", "@" + _siglasA + "  VS  @" + _siglasB));
            }
        }

        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DEL JUGADOR SELECCIONADO.
        private void AbrirVentana_detalle(model_lideres jugador, string tipoEstadisticasAMostrar) {
            if (!_switcherValue){
                Application.Current.MainPage.Navigation.PushAsync(new view_detalle_estadisticaJugadores(
                new model_equipos{
                    _id = _idA,
                    _source = _sourceA,
                    _nombreEquipo = _nomnbreEquipoA,
                    _siglas = _siglasA
                },
                new model_jugadores{
                    _idJugador = jugador._idJugadorA,
                    _source = jugador._sourceJugadorA,
                    _nombreJugador = jugador._nombreJugadorA,
                    _apellidoJugador = jugador._apellidoJugadorA,
                    _posicion = jugador._posicionJugadorA,
                }, tipoEstadisticasAMostrar, "2", true, "@" + _siglasA + "  VS  @" + _siglasB));
            }
            else{
                Application.Current.MainPage.Navigation.PushAsync(new view_detalle_estadisticaJugadores(
                new model_equipos{
                    _id = _idB,
                    _source = _sourceB,
                    _nombreEquipo = _nomnbreEquipoB,
                    _siglas = _siglasB
                },
                new model_jugadores{
                    _idJugador = jugador._idJugadorB,
                    _source = jugador._sourceJugadorB,
                    _nombreJugador = jugador._nombreJugadorB,
                    _apellidoJugador = jugador._apellidoJugadorB,
                    _posicion = jugador._posicionJugadorB,
                }, tipoEstadisticasAMostrar, "2", true, "@" + _siglasA + "  VS  @" + _siglasB));
            }
        }

        //ABRE LA VENTANDA DEDETALLES DEL EQUIPO SELECCIONADO
        private void MC_labels_abrirDetallesEquipos(string equipo){
            if (equipo.Equals("A")) {
                Application.Current.MainPage.Navigation.PushAsync(new view_detalles_equipos(
                    new model_equipos {
                        _id = _idA,
                        _source = _sourceA,
                        _nombreEquipo = _nomnbreEquipoA,
                        _siglas = _siglasA
                    }, 0)); }
            else { Application.Current.MainPage.Navigation.PushAsync(new view_detalles_equipos(
                new model_equipos {
                    _id = _idB,
                    _source = _sourceB,
                    _nombreEquipo = _nomnbreEquipoB,
                    _siglas = _siglasB
                }, 0)); }
        }

        //ESTE METODO ES PARA LIMPIAR LAS LISTAS PORQUE POR ALGUNA RAZON, AL ABRIR LA VENTANA DE HISTORIAL DE PARTIDOS Y VOLVER A CARGAR ESTA VENTANA PERO EN OTRO OBJETO
        //LA VENTANA ORIGINAL SE QUEDABA CON ESOS DATOS Y LA LISTA IBA AUMENTANDO CONSECUTIVAMENTE. **BUSCANDO UNA FORMA MAS OPTIMA DE HACER ESTE PROCESO**
        public void LimpiarListasSiTienenDatosDeMas() {
            List<model_lideres> tmp = new List<model_lideres>();
            if (_lista_lideresPuntosA.Count > 2) {
                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_lideresPuntosA[i]);
                _lista_lideresPuntosA.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_lideresPuntosA.Add(tmp[i]);

                tmp.Clear();

                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_lideresPuntosB[i]);
                _lista_lideresPuntosB.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_lideresPuntosB.Add(tmp[i]);

                tmp.Clear();

                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_lideresRebotesA[i]);
                _lista_lideresRebotesA.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_lideresRebotesA.Add(tmp[i]);

                tmp.Clear();

                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_lideresRebotesB[i]);
                _lista_lideresRebotesB.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_lideresRebotesB.Add(tmp[i]);

                tmp.Clear();

                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_lideresAsistenciasA[i]);
                _lista_lideresAsistenciasA.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_lideresAsistenciasA.Add(tmp[i]);

                tmp.Clear();

                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_lideresAsistenciasB[i]);
                _lista_lideresAsistenciasB.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_lideresAsistenciasB.Add(tmp[i]);

                tmp.Clear();

                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_loMasDestacadoA[i]);
                _lista_loMasDestacadoA.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_loMasDestacadoA.Add(tmp[i]);

                tmp.Clear();

                for (int i = 0; i < 2; i++)
                    tmp.Add(_lista_loMasDestacadoB[i]);
                _lista_loMasDestacadoB.Clear();
                for (int i = 0; i < 2; i++)
                    _lista_loMasDestacadoB.Add(tmp[i]);

                tmp.Clear();
            }
        }

        //INICIA EL MESAGING CENTER.
        //COMO HAY DOS PAGINAS QUE UTILIZAN ESTA MISMA CLASE PARA MOSTRAR SUS DATOS, HAY QUE SEPARAR LOS MESSAGIN CENTER UTILIZANDO LA VARIABLE "VARIACION" PARA IDENTIFICAR UNO DE OTRO.
        private void StarMessaginCenter(int variacion) {
            if (variacion == 1)
                MessagingCenter.Subscribe<Message>(this, "cargarPuntosBasquetball", Llamar => { Async_llenarPuntos(Llamar.PuntosBasquetball); });
            if (variacion == 2)
                MessagingCenter.Subscribe<Message>(this, "cargarLideres", Llamar => { Async_llenarListas(Llamar.Lideres); });
            if (variacion == 3)
                MessagingCenter.Subscribe<Message>(this, "cargarDetallesEquipoB", Llamar => { Async_cargarDatosFaltantes(Llamar.Equipos); });
        }

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        //COMO HAY DOS PAGINAS QUE UTILIZAN ESTA MISMA CLASE PARA MOSTRAR SUS DATOS, HAY QUE SEPARAR LOS MESSAGIN CENTER UTILIZANDO LA VARIABLE "VARIACION" PARA IDENTIFICAR UNO DE OTRO.
        private void StopMessaginCenter(int variacion) {
            if (variacion == 1)
                MessagingCenter.Unsubscribe<Message>(this, "carcargarPuntosBasquetballgarLideres");
            if (variacion == 2)
                MessagingCenter.Unsubscribe<Message>(this, "cargarLideres");
            if (variacion == 3)
                MessagingCenter.Unsubscribe<Message>(this, "cargarDetallesEquipoB");
        }
        #endregion
    }
}
