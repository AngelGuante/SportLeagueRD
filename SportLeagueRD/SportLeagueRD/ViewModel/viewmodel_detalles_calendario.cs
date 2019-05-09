using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.Services;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_detalles_calendario : Base_viewModel {
        #region VARIABLES
        private string Comentario = "";
        private bool NoHanComntado = true;
        private bool _busy = true;
        private ListView ListViewComponent;

        //PARA ACTUALIZAR LAS PROPIEDADES LUEGO DE CARGAR LA PAGINA
        private string Localidad = "";
        private string VotosA = "";
        private string VotosB = "";
        private bool EnVivo = false;
        private bool MostrarBotonesVotos = false;
        private Color ColorFecha = Color.White;

        private string Comprobante1 = "MC02";
        private string Comprobante2 = "CM02";
        private string Comprobante3 = "CM01";
        private string Comprobante4 = "PM01";

        //  ESTAS VARIABLES SON PARA REPARAR UN PROBLEMA QUE SE DABA AL MOMENTO DE CARGAR LA PAGINA ESTA MANDABA LAS PETICIONES DE PUNTOS Y COMENTARIOS MAS DE UNA VEZ
        //  POR ENDE SE TRAIAN LOS DATOS REPETIDOS Y DABA UN IndexOutOfRageExceprion.
        private bool PuedeHacerPeticion_Comentarios = true;
        private bool PuedeHacerPeticion_Puntos = true;
        #endregion

        #region PROPIEDADES
        #region LISTAS
        // PUNTOS EN VIVO
        public ObservableCollection<model_puntos_bascketball> _puntuaciones { get; set; }
        //COMENTARIOS
        public ObservableCollection<model_comentarios> _lista { get; set; }
        #endregion

        // CAMBIAR EL COLOR DE EL CAMPO DE FECHA, ROJO SI ES EN VIVO Y BLANCO SI EL PARTIDO YA PASO
        public Color _colorFecha { get => ColorFecha;
            set {
                ColorFecha = value;
                OnPropertyChanged();
            }
        }

        // PARA SABER CUANDO EL PARTIDO SEA EN VIVO
        public bool _enVivo { get => EnVivo;
            set {
                EnVivo = value;
                _colorFecha = Color.FromHex("FF2027");
                OnPropertyChanged();
            }
        }

        #region PROPIEDADES DE LOS EQUIPOS
        //GENERAL
        public string _id { set; get; }
        public string _fecha { set; get; }
        public string _hora { set; get; }
        public string _localidad { get => Localidad;
            set {
                Localidad = value;
                OnPropertyChanged();
            }
        }
        public string _votosA { get => VotosA;
            set {
                VotosA = value;
                OnPropertyChanged();
            }
        }
        public string _votosB { get => VotosB;
            set {
                VotosB = value;
                OnPropertyChanged();
            }
        }
        public bool _mostrarBotonesVotos {
            get => MostrarBotonesVotos;
            set {
                MostrarBotonesVotos = value;
                OnPropertyChanged();
            }
        }

        //EQUIPO A
        public string _idA { set; get; }
        public ImageSource _sourceA { set; get; }
        public string _nomnbreEquipoA { set; get; }
        public string _siglasA { set; get; }
        //EQUIPO B
        public string _idB { set; get; }
        public ImageSource _sourceB { set; get; }
        public string _nomnbreEquipoB { set; get; }
        public string _siglasB { set; get; }
        #endregion

        //CONTENIDO DEL COMENTARIO ESCRITO POR EL USUARIO
        public string _comentario { get => Comentario;
            set{
                Comentario = value;
                OnPropertyChanged();
            }
        }

        //PARA MOSTRAR U OCULTAR EL ESTADO DEL UN CARTER QUE MOTIVA A QUE EL USUARIO COMENTE EN CASO DE QUE NADIE LO HAYA HECHO
        public bool _sinComentarios{
            get => NoHanComntado;
            set {
                NoHanComntado = value;
                OnPropertyChanged();
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

        #region ICOMMANDS
        public ICommand _btn_enviar { get; }
        public ICommand _label_siglasA { get; }
        public ICommand _label_siglasB { get; }
        public ICommand _button_votarA { get { return new Command(() => { votar(_idA); }); } }
        public ICommand _button_votarB { get { return new Command(() => { votar(_idB); }); } }
        #endregion

        #region CONSTRUCTORR
        public viewmodel_detalles_calendario(model_marcador calendario, ListView lista){
            #region INICIALIZAR POPIEDADES DE LOS EQUIPOS
            //GENERAL
            _id = calendario._id;
            _fecha = calendario._fecha;
            _hora = calendario._hora;
            //EQUIPO A
            _idA = calendario._idA;
            _sourceA = calendario._sourceA;
            _nomnbreEquipoA = calendario._nomnbreEquipoA;
            _siglasA = calendario._siglasA;
            //EQUIPO B
            _idB = calendario._idB;
            _sourceB = calendario._sourceB;
            _nomnbreEquipoB = calendario._nomnbreEquipoB;
            _siglasB = calendario._siglasB;
            #endregion

            #region INICIALIZAR COMANDOS
            _label_siglasA = new Command(() => {
                MC_labels_abrirVentanaEquipo(new model_equipos{
                    _id = _idA,
                    _source = _sourceA,
                    _nombreEquipo = _nomnbreEquipoA,
                    _siglas = _siglasA
                });
            });
            _label_siglasB = new Command(() => {
                MC_labels_abrirVentanaEquipo(new model_equipos{
                    _id = _idB,
                    _source = _sourceB,
                    _nombreEquipo = _nomnbreEquipoB,
                    _siglas = _siglasB
                });
            });
            _btn_enviar = new Command(async () => {
                if (await new VerificarLogeoYGestionarVotos().VerificarLogeo())
                    if (!string.IsNullOrWhiteSpace(_comentario)) {
                        _lista.Add(new model_comentarios {
                            _usuario = App.usuario.Nombre,
                            _comentario = _comentario,
                            _procedencia = 1
                        });
                        //ENVIAR EL COMENTARIO HECHO POR EL USUARIO AL SERVER
                        App.ServerC.SendMessageAsync($"{Comprobante3}-{App.usuario.ID}-{_id}-{_comentario}");
                        _sinComentarios = false;
                        _comentario = "";
                        ScrollDown(true);
                    }
            });
            #endregion

            _lista = new ObservableCollection<model_comentarios>();
            _puntuaciones = new ObservableCollection<model_puntos_bascketball>();
            
            // SI EL ESTADO DEL PARTIDO ES 8 ENTONCES ES UN PARTIDO EN VIVO POR ENDE SE PROCEDE A MOSTRAR Y CAMBIAR LOS COMPONENTES NECESARIOS.
            if (calendario._estado.Equals("8")) _enVivo = true;

            ListViewComponent = lista;

            StarMessaginCenter(0);
            App.ServerC.SendMessageAsync($"{Comprobante1}-{calendario._id}");
        }
        #endregion

        #region METODOS
        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DE LA EVENTO SELECCIONADA.
        private void MC_labels_abrirVentanaEquipo(model_equipos equipo) => Application.Current.MainPage.Navigation.PushAsync(new view_detalles_equipos(equipo, 0));

        //TRAE LOS DATOS BASICOS DE EL MARCADOR QUE NO VIENEN DE LA VENTANA ANTERIOR
        private async void LlenarVentana(List<model_marcador> marcador) {
            IsBusy = true;
            await Task.Run(() => {
                _localidad = marcador[0]._localidad;
                _votosA = marcador[0]._votosA;
                _votosB = marcador[0]._votosB;
                if(!EnVivo)
                    _mostrarBotonesVotos = marcador[0]._usuarioVotado.Equals("true") ? false : true;

                StopMessaginCenter(0);
                if (PuedeHacerPeticion_Comentarios) {
                    PuedeHacerPeticion_Comentarios = false;
                    StarMessaginCenter(1);
                    App.ServerC.SendMessageAsync($"{Comprobante2}-{_id}");
                }
            });
        }

        //BUSCA LOS COMENTARIOS EN EL SERVIDOR DEL PARTIDO
        private async void LlenarListaComentarios(List<model_comentarios> comentarios) {
            await Task.Run(() => {
                foreach (model_comentarios comentario in comentarios) {
                    if(!string.IsNullOrEmpty(comentario._usuario))
                        _lista.Add(new model_comentarios {
                            _usuario = comentario._usuario,
                            _comentario = comentario._comentario,
                            _procedencia = comentario._procedencia
                        });
                }
                ScrollDown(false);
                //SI NO HAY COMENTARIOS SE MOSTRARA UN COMENTARIO MOTIVANDO A QUE SE COMENTE
                if (_lista.Count > 0)
                    _sinComentarios = false;

                if (_enVivo && PuedeHacerPeticion_Puntos) {
                    PuedeHacerPeticion_Puntos = false;
                    StarMessaginCenter(2);
                    App.ServerC.SendMessageAsync($"{Comprobante4}-{_id}");
                } else
                    IsBusy = false;
            });
        }

        // TRAE LOS PUNTOS DEL PARTIDO EN CASO DE QUE ESTE SEA EN VIVO
        private async void LlenarPuntos(List<model_puntos_bascketball> puntos) {
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
                    StopMessaginCenter(2);
                    IsBusy = false;
                }
            });
        }

        //HACER AUTO-SCROLL HACIA ABAJO PARA MOSTRAR EL MENSAGE ENVIADO
        private void ScrollDown(bool scroll) => Xamarin.Forms.Device.BeginInvokeOnMainThread(() => ListViewComponent.ScrollTo(_lista[_lista.Count() - 1], ScrollToPosition.MakeVisible, scroll));

        //  METODO QUE LLAMAN LOS BOTONES DE VOTACION
        private async void votar(string idEquipo) {
            if (await votara(idEquipo))
                if(idEquipo.Equals(_idA))
                    _votosA = (int.Parse(_votosA) + 1).ToString();
                else
                    _votosB = (int.Parse(_votosB) + 1).ToString();
        }

        //  GESTIONA LOS DATOS DE LA VOTACION INDEPENDIENTEMENTE DEL VOTON DE VOTACION QUE LO LLAME
        private async Task<bool> votara(string id) {
             if(await new VerificarLogeoYGestionarVotos().VotarPorEquipoEnPartido(_id, id)) { 
                _mostrarBotonesVotos = false;
                return true;
            }
            return false;
        }

        //INICIA EL MESAGING CENTER.
        public void StarMessaginCenter(int variacion) {
            if(variacion == 0)
                MessagingCenter.Subscribe<Message>(this, "cargarMarcadorDetallado", Llamar => { LlenarVentana(Llamar.Marcador); });
            if (variacion == 1)
                MessagingCenter.Subscribe<Message>(this, "cargarComentariosUsuarios", Llamar => { LlenarListaComentarios(Llamar.Comentarios); });
            if (variacion == 2)
                MessagingCenter.Subscribe<Message>(this, "cargarPuntosBasquetball", Llamar => { LlenarPuntos(Llamar.PuntosBasquetball); });
        }

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        public void StopMessaginCenter(int variacion) {
            if(variacion == 0)
                MessagingCenter.Unsubscribe<Message>(this, "cargarMarcadorDetallado");
            if (variacion == 1)
                MessagingCenter.Unsubscribe<Message>(this, "cargarComentariosUsuarios");
            if (variacion == 2)
                MessagingCenter.Unsubscribe<Message>(this, "cargarPuntosBasquetball");
        }
        #endregion
    }
}
