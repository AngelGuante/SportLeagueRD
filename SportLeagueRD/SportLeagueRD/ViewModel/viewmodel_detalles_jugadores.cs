using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.Utilitys.tools;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_detalles_jugadores : Base_viewModel{
        #region VARIABLES
        private bool VerEquiposClickeado = true;

        private string Variacion;

        //PARA ACTUALIZAR LAS PROPIEDADES.
        private string SiglasEquipo = "";
        private string NombreEquipo = "";
        private ImageSource Source;
        private string Votos;
        private string FechaNacimientoEdad;
        private string FechaIngreso;
        private string Numero;
        private string NombreEquipoYSigla;

        private string Comprobante1 = "JG03";
        private string Comprobante3 = "JG04";
        private string Comprobante4 = "JG05";
        private string Comprobante5 = "JG06";
        private string Comprobante2 = "JE01";
        #endregion

        #region ICOMMANDS
        public ICommand _label_equipo { get; set; }
        public ICommand _label_verEquipos { get; set; }
        public ICommand _btn_votar { get; set; }
        //ICOMMAND PARA CUANDO SE SELECCIONE UN ELEMENTO DEL VIEW CELL.
        public ICommand _elementoSeleccionado{ get => new Command((item) => { MC_label_estatisticaJugadores(item as model_equipos);}); }
        #endregion

        #region PROPIEDADES
        #region PROPIEDADES DE EQUIPO Y JUGADOR
        //DATOS DEL EQUIPO
        public string _idEquipo { set; get; }
        public ImageSource _sourceEquipo { set; get; }
        public string _nombreEquipo {
            get => NombreEquipo;
            set {
                NombreEquipo = value;
                OnPropertyChanged();
            }
        }
        public string _siglasEquipo {
            get => SiglasEquipo;
            set{
                SiglasEquipo = value;
                _nomreEquipoYSigla = _nombreEquipo + " @" + _siglasEquipo;
                OnPropertyChanged();
            }
        }
        public string _nomreEquipoYSigla {
            get => NombreEquipoYSigla;
            set {
                NombreEquipoYSigla = value;
                OnPropertyChanged();
            }
        }
        public string _fechaIngreso {
            get => FechaIngreso;
            set {
                FechaIngreso = value;
                OnPropertyChanged();
            }
        }
        //DATOS DEL JUGADOR
        public string _idJugador { set; get; }
        public ImageSource _source {
            get => Source;
            set {
                Source = value;
                OnPropertyChanged();
            }
        }
        public string _nombreJugador { set; get; }
        public string _apellidoJugador { set; get; }
        public string _nombreCompleto { get => Numero;
            set {
                Numero = value;
                OnPropertyChanged();
            }
        }
        public string _posicion { set; get; }
        public string _idPosicion { get; set; }
        public string _fechaNacimientoEdad {
            get => FechaNacimientoEdad;
            set {
                FechaNacimientoEdad = value;
                OnPropertyChanged();
            }
        }
        public string _votos {
            get => Votos;
            set{
                Votos = value;
                OnPropertyChanged();
            }
        }
        public string _numero { set; get; }
        #endregion

        #region LISTAS
        public ObservableCollection<model_equipos> _equipos { set; get; }
        #endregion

        #region GENERAL
        //OCULTA EL LABEL VER_EQUIPOS CUANDO SE LE HAGA CLICK
        public bool _verEquiposClickeado {
            get => VerEquiposClickeado;
            set {
                VerEquiposClickeado = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region COSTRUCTOR
        public viewmodel_detalles_jugadores(model_jugadores jugador, string variacion){
            _equipos = new ObservableCollection<model_equipos>();

            Variacion = variacion;

            #region INICIALIZAR PROPIEDSADES DEL MODEL EQUIPO QUE VIENEN DE LA VENTANA ANTERIOR
            //DATOS DEL EQUIPO
            _idEquipo = jugador._idEquipo;
            _sourceEquipo = jugador._sourceEquipo;
            //DATOS DEL JUGADOR
            _idJugador = jugador._idJugador;
            _source = jugador._source;
            _nombreJugador = jugador._nombreJugador;
            _apellidoJugador = jugador._apellidoJugador;
            _nombreCompleto = jugador._nombreJugador + " " + jugador._apellidoJugador;
            _posicion = jugador._posicion;

            if (variacion == "1")
                _votos = jugador._votos;
            if (variacion == "2" || variacion == "3"){
                _sourceEquipo = jugador._sourceEquipo;
                _nombreEquipo = jugador._nombreEquipo;
                _siglasEquipo = jugador._siglasEquipo;
                if (variacion == "2")
                    _source = jugador._source;
                if (variacion == "3")
                    _numero = jugador._numero;
            }
            #endregion

            #region INICIALIZAR COMANDOS
            _btn_votar = new Command(MC_btn_realizarVoto);
            _label_equipo = new Command(MC_abrirVentana_detalleEquipo);
            _label_verEquipos = new Command(() => {
                StarMessaginCenter(1);
                App.ServerC.SendMessageAsync($"{Comprobante2}-{_idJugador}");
            });
            #endregion

            StarMessaginCenter(0);

            if (Variacion == "3")
                App.ServerC.SendMessageAsync($"{Comprobante3}-{_idJugador}");
            else if (Variacion == "2" || Variacion == "0") {
                if(jugador._source.ToString().Equals("File: "))
                    App.ServerC.SendMessageAsync($"{Comprobante5}-{_idJugador}");
                else
                    App.ServerC.SendMessageAsync($"{Comprobante1}-{_idJugador}");
            }
            else if (Variacion == "1")
                App.ServerC.SendMessageAsync($"{Comprobante4}-{_idJugador}");
        }
        #endregion

        #region METODOS
        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DE EL EQUIPO DEL JUGADOR.
        private void MC_abrirVentana_detalleEquipo() => Application.Current.MainPage.Navigation.PushAsync(new view_detalles_equipos(
            new model_equipos {
                    _id = _idEquipo,
                    _source = _sourceEquipo,
                    _nombreEquipo = _nombreEquipo,
                    _siglas = _siglasEquipo
                }, 0));

        //METODO QUE CARGA LOS DATOS EN LA TABLA DE VER EQUIPOS
        public async void LlenarListaTodosEquipos(List<model_equipos> equipos){
            _verEquiposClickeado = false;
            await Task.Delay(100);
            await Task.Run(() => {
                //AGREGA LOS DATOS DEL EQUIPO AL QUE PERTENECE ACTUALMENTE
                _equipos.Add(new model_equipos {
                    _id = _idEquipo,
                    _nombreEquipo = _nombreEquipo,
                    _siglas = _siglasEquipo
                });

                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                equipos.RemoveAt(equipos.Count - 1);

                //AGREGA LOS DATOS DE LOS EQUIPOS A LOS QUE HAYA PERTENECIDO CON ANTERIORIDAD
                foreach (model_equipos tmp in equipos)
                    _equipos.Add(new model_equipos {
                        _id = tmp._id,
                        _source = tmp._source,
                        _nombreEquipo = tmp._nombreEquipo,
                        _siglas = tmp._siglas
                    });
                StopMessaginCenter(1);
            });
        }

        //  EVENTO PARA EL BOTON DE VOTAR, VERIFICA QUE EL USUARIO ESTE LOGEADO, SI NO LO ESTA, LO MANDA A LOGEARSE EN CASO CONTRARIO REALIZA EL VOTO
        private void MC_btn_realizarVoto() => new VerificarLogeoYGestionarVotos().VotarPorJugador(_idJugador, _nombreJugador, _idEquipo, CastearPosiciones(_posicion));

        //  CASTEA LA POSICION A UN ENTERO PARA SOLO MOSTRAR LAS VOTACIONES CORRESPONDIENTES
        private string CastearPosiciones(string idPosicion) {
            switch (idPosicion) {
                case "Base":
                    return "1";
                case "Alero":
                    return "2";
                case "Pivot":
                    return "3";
                case "Ala-Pivot":
                    return "4";
            }
            return "";
        }

        //LLENA LOS DATOS DEL MODELO QUE FALTARON POR LLENARSE TRAIDOS DESDE LA VENTANA ANTERIOR PERO QUE SE NECESITAN EN ESTA.
        private async void LlenarPropiedades_Server(List<model_jugadores> jugadores) {
            await Task.Run(() => {
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                jugadores.RemoveAt(jugadores.Count - 1);

                //PROPIEDADES QUE SE LLENARAN SIEMPRE SIN IMPORTAR EL CASO
                _nombreCompleto += Variacion == "3" ? $" #{_numero}" : $" #{ jugadores[0]._numero}";
                _fechaIngreso = jugadores[0]._fechaIngreso;
                _fechaNacimientoEdad = $"{jugadores[0]._fechaNacimientoEdad} Años)";

                //SI VARIACION ES IGUAL A 0
                //  SE TRAEN: LOS VOTOS.
                //  NO SE TRAEN: EL SOURCE DEL EQUIPO
                if (Variacion == "0"){
                    _nombreEquipo = jugadores[0]._nombreEquipo;
                    _siglasEquipo = jugadores[0]._siglasEquipo;
                    _numero = jugadores[0]._numero;
                    _votos =  jugadores[0]._votos;
                }
                //SI VARIACION ES IGUAL A 1,
                //  SE TRAE: EL SOURCE DEL EQUIPO
                //  NO SE TRAEN: LOS VOTOS
                else if (Variacion == "1"){
                    _sourceEquipo = jugadores[0]._source;
                    _nombreEquipo = jugadores[0]._nombreEquipo;
                    _siglasEquipo = jugadores[0]._siglasEquipo;
                }

                //SI VARIACION ES IGUAL A 2,
                //  SE TRAE: EL SOURCE DEL JUGADOR, EN CASO DE QUE NO VENGA DESDE LA VENTANA ANTERIOR, LOS VOTOS
                else if (Variacion == "2") {
                    _votos = jugadores[0]._votos;
                    _numero = jugadores[0]._numero;

                    //SE VERIFICA SI _source ES IGUAL A "", EN CASO DE QUE SEA CIERTO, SE TRAE DEL SERVIDOR EL SOURCE DEL JUGADOR
                    string SourceJugador = _source.ToString();
                    if (SourceJugador.Equals("File: "))
                        _source = jugadores[0]._source;
                }

                //SI  VACRIACON ES IGUAL A 3,
                //  ESTA OPCION ES MAS PARA LISTAS, CUANDO VIENEN DIRECTAMENTE DE LA VENTANA "DETALLES_JUGADORES"
                else if (Variacion == "3") {
                    _votos =  jugadores[0]._votos;
                    _source = jugadores[0]._source;
                }
                StopMessaginCenter(0);
            });
        }

        //ABRE LA PAGINA QUE MUESTRA LA ESTADISTICA DE TODOS LOS JUGADORES.
        private void MC_label_estatisticaJugadores(model_equipos equipo) => Application.Current.MainPage.Navigation.PushAsync(new view_detalle_estadisticaJugadores(
            new model_equipos {
                    _id = _idEquipo,
                    _source = (equipo._siglas == _siglasEquipo) ? _sourceEquipo : equipo._source,
                    _nombreEquipo = equipo._nombreEquipo,
                    _siglas = equipo._siglas
                },
            new model_jugadores {
                    _idJugador = _idJugador,
                    _nombreJugador = _nombreJugador,
                    _apellidoJugador = _apellidoJugador,
                    _posicion = _posicion
                }, "General", Variacion, false, ""));

        //INICIA EL MESAGING CENTER.
        //COMO HAY VARIAS PARTES QUE ACCEDEN AL SERVIDOR EN ESTA PAGINA SE RECIBE COMO PARAMETRO UN ENTERO QUE DETERMINA CUAL DE TODOS ES EL QUE SE INICIARA
        private void StarMessaginCenter(int cargar){
            if(cargar == 0)
                MessagingCenter.Subscribe<Message>(this, "cargarDetallesJugador", Llamar => { LlenarPropiedades_Server(Llamar.Jugadores); });
            else if(cargar == 1)
                MessagingCenter.Subscribe<Message>(this, "cargarEquipoPorJugador", Llamar => { LlenarListaTodosEquipos(Llamar.Equipos); });
        }

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        //COMO HAY VARIAS PARTES QUE ACCEDEN AL SERVIDOR EN ESTA PAGINA SE RECIBE COMO PARAMETRO UN ENTERO QUE DETERMINA CUAL DE TODOS ES EL QUE SE DETENDRA
        private void StopMessaginCenter(int cargar){
            if(cargar == 0)
                MessagingCenter.Unsubscribe<Message>(this, "cargarDetallesJugador");
            else
                MessagingCenter.Unsubscribe<Message>(this, "cargarEquipoPorJugador");
        }
        #endregion
    }
}