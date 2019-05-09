using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.Services;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_detalles_equipos : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        private bool CargandoAlgo = false;

        private int OpacidadFrame = 0;

        private int Variacion;

        //GUARDARAN LOS ESTADOS DE VISIVILIDAD DE LOS LISTVIEWS DE JUGADORES Y ULTIMOS PARTIDOS
        private bool Lista_jugadores_visibilidad = true;
        private bool Lista_ultimosPartidos_visibilidad = false;

        //PARA QUE SOLO SE LLENEN LAS LISTAS LA PRIMERA VEZ QUE SE LE DE CLICK A LOS BOTONES.
        private bool Lista_jugadores_visibilidad_cargada = false;
        private bool Lista_ultimosPartidos_visibilidad_cargada = false;

        private string Comprobante;
        private string Comprobante1 = "EQ03";
        private string Comprobante2 = "EQ04";
        private string Comprobante3 = "EJ01";

        //PROPIEDADES QUE SE DEBEN ACTUALIZAR LUEGO DE QUE LA PAGINA SE A CARGADO
        private string EstadoEquipo;
        private string Representante;
        private string Localidad;
        private string Votos;
        #endregion

        #region ICOMMANDS
        public ICommand _btn_jugadores { get; set; }
        public ICommand _btn_ultimosJuegos { get; set; }
        public ICommand _btn_votar { get; set; }
        public ICommand _label_localidad { get; set; }
        public ICommand _label_estadisticasJugadores { get; set; }
        public ICommand _elementoSeleccionado_jugadores { get { return new Command((item) => { MC_label_estatisticaJugadores(new ObservableCollection<model_jugadores> { item as model_jugadores }); }); }
        }
        public ICommand _elementoSeleccionado_partidos{
            get => new Command((item) => {
                MC_AbrirVentanaMarcadorSeleccionado(item as model_marcador);
            });
        }
        public ICommand _label_todos_los_partidos { get; set; }
        #endregion

        #region PROPIEDADES
        #region **PROPIEDADES DE MODEL EQUIPO
        public string _id { get; set; }
        public ImageSource _source { get; set; }
        public string _nombreEquipo { get; set; }
        public string _siglas { get; set; }
        public string _representante { get => Representante;
            set{
                Representante = value;
                OnPropertyChanged();
            }
        }
        public string _localidad { get => Localidad;
            set {
                Localidad = value;
                OnPropertyChanged();
            }
        }
        public string _ubicacionLocalidad { get; set; }
        public string _votos { get => Votos;
            set{
                Votos = value;
                OnPropertyChanged();
            }
        }
        public string _estado { get => EstadoEquipo;
            set {
                //SOLO ES PARA ACTUALIZAR LAS PROPIEDADES PERTINENTES, NO ASIGNACION DEL VALUE
                EstadoEquipo = value;
                _EstadoEquipo = "";
                _colorEstadoEquipo = new Color();
            }
        }
        #endregion

        #region **LISTAS
        public ObservableCollection<model_jugadores> _jugadores { set; get; }
        public ObservableCollection<model_marcador> _ultimosPartidos { set; get; }
        #endregion

        #region **GENERAL
        //PROPIEDAD QUE DETERMINA SI ESTA PAGINA ESTA REALIZANDO ALGUN TRABAJO, ASI OCULTARLA DEBAJO DE UUNA PAGINA CON UN ActivityIndicator
        public bool IsBusy{
            get => _busy;
            set{
                _busy = value;
                _OpacidadFrame = 1;
                OnPropertyChanged();
            }
        }

        //ALMACENA LA VISIBILIDAD DE UN LABEL EN LA PAGINA QUE MUESTRA "CARGANDO" EN CASO DE QUE SE ESTE TRATENDO ALGO DESDE EL SERVIDOR
        public bool _cargandoAlgo{
            get => CargandoAlgo;
            set{
                CargandoAlgo = value;
                OnPropertyChanged();
            }
        }

        //PARA QUE EL FRAME QUE MUESTRA EL ESTADO DEL EQUIPO NO SEA VISIBLE DESDE EL INICIO DE LA PAGINA SINO QUE SE HAGA VISIBLE SOLO CUANDO LA PAGINA SE CARGE
        public int _OpacidadFrame{
            get => OpacidadFrame;
            set{
                OpacidadFrame = value;
                OnPropertyChanged();
            }
        }

        #region ****HARA QUE LAS LISTAS SEAN VISIBLES E INVISIBLES SEGUN SEAN EL CASO
        //HARA QUE LA LISTA DE MOSTRAR JUGADORES SE MUESTRO U OCULTE SEGUN SEA EL CASO Y PASANDOLE EL VALOR CONTRARIO A LA OTRA LISTA
        public bool _lista_jugadores_visibilidad{
            get => Lista_jugadores_visibilidad;
            set{
                _lista_ultimosPartidos_visibilidad = !value;
                Lista_jugadores_visibilidad = value;
                OnPropertyChanged();
            }
        }

        //HARA QUE LA LISTA DE MOSTRAR ULTIMOS PARTIDOS SE MUESTRO U OCULTE SEGUN SEA EL CASO Y PASANDOLE EL VALOR CONTRARIO A LA OTRA LISTA
        public bool _lista_ultimosPartidos_visibilidad{
            get => Lista_ultimosPartidos_visibilidad;
            set{
                Lista_ultimosPartidos_visibilidad = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //PROPIEDAD PARA DETECTAR CUANDO LA LISTA DE JUGADORES SEA CARGADO
        public bool _listaJugadoresCargados{
            get => Lista_jugadores_visibilidad_cargada;
            set{
                Lista_jugadores_visibilidad_cargada = value;
                OnPropertyChanged();

                //ACTUALIZAR LA VISIVILIDAD DE EL CUADRO QUE MUESTRA EL ESTADO DEL EQUIPO.
                _algunaListaCargada = true;
            }
        }

        //PROPIEDAD PARA DETECTAR CUANDO LA LISTA DE JUGADORES SEA CARGADO
        public bool _listaUltimosPartidosCargados{
            get => Lista_ultimosPartidos_visibilidad_cargada;
            set{
                Lista_ultimosPartidos_visibilidad_cargada = value;
                OnPropertyChanged();

                //ACTUALIZAR LA VISIVILIDAD DE EL CUADRO QUE MUESTRA EL ESTADO DEL EQUIPO.
                _algunaListaCargada = true;
            }
        }

        //PROPIEDAD QUE GESTIONA LA VISIBILIDAD DE EL CUADRO QUE MUESTRA EL ESTADO DEL EQUIPO
        public bool _algunaListaCargada{ get =>  AlgunaListaCargada(); set => OnPropertyChanged(); }

        //PROPIEDAD QUE ASIGNA EL COLOR DEL ESTADO DEL EQUIPO SEGUN EL ESTADO DEL EQUIPO
        public Color _colorEstadoEquipo { get => ColorTextoSegunEstadoEquipo(); set => OnPropertyChanged(); }

        //PROPIEDAD QUE ASIGNA EL TEXTO DEL ESTADO DEL EQUIPO SEGUN EL ESTADO DEL EQUIPO
        public string _EstadoEquipo { get => EstadoEquipoTexto();  set => OnPropertyChanged();  }
        #endregion
        #endregion

        #region CONSTRUCTOR
        public viewmodel_detalles_equipos(model_equipos equipo, int variacion){
            #region INICIALIZAR LISTAS
            _jugadores = new ObservableCollection<model_jugadores>();
            _ultimosPartidos = new ObservableCollection<model_marcador>();
            #endregion

            #region INICIALIZAR PROPIEDSADES DEL MODEL EQUIPO QUE VIENEN DE LA VENTANA ANTERIOR
            Variacion = variacion;

            _id = equipo._id;
            _source = equipo._source;
            _nombreEquipo = equipo._nombreEquipo;
            _siglas = equipo._siglas;

            if (Variacion == 1)
                _votos = equipo._votos;
            #endregion

            #region INICIALIZAR COMANDOS
            _btn_jugadores = new Command(() => {
                _lista_jugadores_visibilidad = true;

                //SI LOS DATOS YA SE BUSCARON LA PRIMERA VEZ A LA BASE DE DATOS, NO SE VA DE NUEVO AL PRECIONAR EL BOTON
                if (_listaJugadoresCargados)
                    return;
                StarMessaginCenter(1);
                App.ServerC.SendMessageAsync($"{Comprobante3}-{_id}");
            });
            _btn_ultimosJuegos = new Command(() => {
                _lista_jugadores_visibilidad = false;

                //SI LOS DATOS YA SE BUSCARON LA PRIMERA VEZ A LA BASE DE DATOS, NO SE VA DE NUEVO AL PRECIONAR EL BOTON
                if (_listaUltimosPartidosCargados)
                    return;
                StarMessaginCenter(2);
                App.ServerC.SendMessageAsync($"EM01-{_id}");
            });
            _btn_votar = new Command(MC_btn_realizarVoto);
            _label_localidad = new Command(MC_label_localidad);
            _label_estadisticasJugadores = new Command(Parametro_estadisticasJugador);
            #endregion

            StarMessaginCenter(0);
            Comprobante = variacion == 0 ? Comprobante1 : Comprobante2;
            App.ServerC.SendMessageAsync($"{Comprobante}-{_id}");
        }
        #endregion

        #region METODOS
    
        //EVENTO DEL BOTON HACE VISIBLE Y LLENA LA LISTA DE LOS JUGADORES Y HACE INVISIBLE LOS DEMAS LISTAS.
        private async void MC_btn_jugadores(List<model_jugadores> jugadores){
            _cargandoAlgo = true;

            //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
            jugadores.RemoveAt(jugadores.Count - 1);

            await Task.Run(() => {
                foreach (model_jugadores tmp in jugadores)
                    _jugadores.Add(tmp);
            });

            StopMessaginCenter(1);
            _cargandoAlgo = false;
            _listaJugadoresCargados = true;
        }

        //EVENTO DEL BOTON HACE VISIBLE Y LLENA LA LISTA DE LOS ULTIMOS PARTIDOS Y HACE INVISIBLE LOS DEMAS LISTAS.
        private async void MC_btn_ultimosJuegos(List<model_marcador> marcador){
            _cargandoAlgo = true;

            //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
            marcador.RemoveAt(marcador.Count - 1);

            await Task.Run(() => {
                foreach (model_marcador tmp in marcador)
                    _ultimosPartidos.Add(tmp);

                //LE PASA LOS DATOS DE ESTE EQUIPO COMO NOMBRE Y DEMAS PARA QUE NO LOS TENGA QUE TRAER DE EL SERVIDOR.
                foreach (model_marcador elemento in _ultimosPartidos)
                    elemento._nomnbreEquipoA = _nombreEquipo;

                StopMessaginCenter(2);
                _cargandoAlgo = false;
                _listaUltimosPartidosCargados = true;
            });
        }

        //  EVENTO PARA EL BOTON DE VOTAR, VERIFICA QUE EL USUARIO ESTE LOGEADO, SI NO LO ESTA, LO MANDA A LOGEARSE EN CASO CONTRARIO REALIZA EL VOTO
        private void MC_btn_realizarVoto() => new VerificarLogeoYGestionarVotos().VotarPorEquipo(_id, _nombreEquipo);

        //ABRE LA VENTANA DEL MARCADOR DE LOS EQUIPOS QUE SE A SELECCIONADO
        private void MC_AbrirVentanaMarcadorSeleccionado(model_marcador item) => Application.Current.MainPage.Navigation.PushAsync(new view_detalles_marcador(new model_marcador {
            _id = item._id,
            _idA = item._idA,
            _sourceA = _source,
            _siglasA = _siglas,
            _nomnbreEquipoA = _nombreEquipo,
            _puntuacionEquipoA = item._puntuacionEquipoA,
            _idB = item._idB,
            _nomnbreEquipoB = item._nomnbreEquipoB,
            _puntuacionEquipoB = item._puntuacionEquipoB,
            _estado = item._estado
        }));

        //EVENTO DEL LABEL QUE MUESTRA LOS DATOS DE LA CANCHA.
        private void MC_label_localidad() => Application.Current.MainPage.DisplayAlert(_localidad, _ubicacionLocalidad, "OK");

        //**COMO EL COMPONENTE ListView no tiene un CommandParameter, toca llamar este metodo y luego pasarle el parametro al metodo correspondiente.
        private void Parametro_estadisticasJugador() => MC_label_estatisticaJugadores(_jugadores);

        //ABRE LA PAGINA QUE MUESTRA LA ESTADISTICA DE TODOS LOS JUGADORES.
        private void MC_label_estatisticaJugadores(ObservableCollection<model_jugadores> lista) => Application.Current.MainPage.Navigation.PushAsync(new view_detalle_estadisticaJugadores(
            new model_equipos{
                _id = _id,
                _source = _source,
                _nombreEquipo = _nombreEquipo,
                _siglas = _siglas
            }, lista, "General", "3"));

        //BUSCA LOS DATOS DEL EQUIPO A LA BASE DE DATOS
        private void Ini_prop_model_equipo(List<model_equipos> equipo){
            //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
            equipo.RemoveAt(equipo.Count - 1);

            _representante = equipo[0]._representante;
            _localidad = equipo[0]._localidad;
            _ubicacionLocalidad = equipo[0]._ubicacionLocalidad;
            _estado = equipo[0]._estado;

            //SI VARIACION ES IGUAL A 0, SE TRAEN LOS VOTOS
            if (Variacion == 0)
                _votos = equipo[0]._votos;
            
            IsBusy = false;
            StopMessaginCenter(0);
        }

        //VERIFICA SI ALGUNA DE LAS LISTAS SE A CARRGADO, SI SE A CARGADO ALGUNA RETORNA TRUE, EN CASO CONTRARIO, FALSE
        private bool AlgunaListaCargada() => (_listaJugadoresCargados || _listaUltimosPartidosCargados) ? false : true;

        //RETORNA UN COLOR DEPENDIENDO DEL VALOR DE LA PROPIEDAD _estado
        private Color ColorTextoSegunEstadoEquipo(){
            switch (_estado){
                case "0":
                    return Color.FromHex("57AF5C");
                case "1":
                    return Color.FromHex("FF2027");
                case "2":
                    return Color.Black;
                default:
                    return Color.White;
            }
        }

        //RETORNA UN string CON EL ESTADO DEL EQUIPO DEPENDIENDO DEL VALOR DE LA PROPIEDAD _estado
        private string EstadoEquipoTexto(){
            switch (_estado){
                case "0":
                    return "ACTIVO";
                case "1":
                    return "SUSPENDIDO";
                case "2":
                    return "RETIRADO";
                default:
                    return "DEFAULT_VALUE";
            }
        }

        //INICIA EL MESAGING CENTER.
        //COMO HAY VARIAS PARTES QUE ACCEDEN AL SERVIDOR EN ESTA PAGINA SE RECIBE COMO PARAMETRO UN ENTERO QUE DETERMINA CUAL DE TODOS ES EL QUE SE INICIARA
        private void StarMessaginCenter(int cargar){
            if(cargar == 0)
                MessagingCenter.Subscribe<Message>(this, "cargarEquipo", Llamar => { Ini_prop_model_equipo(Llamar.Equipos); });
            else if (cargar == 1)
                MessagingCenter.Subscribe<Message>(this, "cargarJugadoresEquipo", Llamar => { MC_btn_jugadores(Llamar.Jugadores); });
            else if (cargar == 2)
                MessagingCenter.Subscribe<Message>(this, "cargarMarcadorEquipo", Llamar => { MC_btn_ultimosJuegos(Llamar.Marcador); });
        }

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        //COMO HAY VARIAS PARTES QUE ACCEDEN AL SERVIDOR EN ESTA PAGINA SE RECIBE COMO PARAMETRO UN ENTERO QUE DETERMINA CUAL DE TODOS ES EL QUE SE INICIARA
        private void StopMessaginCenter(int cargar) {
            if (cargar == 0)
                MessagingCenter.Unsubscribe<Message>(this, "cargarEquipo");
            else if(cargar == 1)
                MessagingCenter.Unsubscribe<Message>(this, "cargarJugadoresEquipo");
            else if (cargar == 2)
                MessagingCenter.Unsubscribe<Message>(this, "cargarMarcadorEquipo");
        }
        #endregion
    }
}