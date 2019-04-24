using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_detalles_eventos : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        private string Titulo;
        private string Lugar;
        private string Fecha;
        private string Hora;
        private ImageSource SourceEvento;
        private string Video;
        private string Texto;

        private string Comprobante = "EV02";
        #endregion

        #region PROPIEDADES
        public string _titulo {
            get => Titulo;
            set {
                Titulo = value;
                OnPropertyChanged();
            }
        }
        public string _lugar {
            get => Lugar;
            set{
                Lugar = value;
                OnPropertyChanged();
            }
        }
        public string _fecha {
            get => Fecha;
            set{
                Fecha = value;
                OnPropertyChanged();
            }
        }
        public string _hora
        {
            get => Hora;
            set {
                Hora = value;
                OnPropertyChanged();
            }
        }
        public ImageSource _sourceEvento
        {
            get => SourceEvento;
            set{
                SourceEvento = value;
                OnPropertyChanged();
            }
        }
        public string _videoEnlace {
            get => Video;
            set{
                Video = value;
                OnPropertyChanged();
            }
        }
        public string _texto {
            get => Texto;
            set{
                Texto = value;
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
        public viewmodel_detalles_eventos(model_eventos evento){
            #region INICIALIZAR PROPIEDSADES DEL MODEL EQUIPO QUE VIENEN DE LA VENTANA ANTERIOR
            _titulo = evento._titulo;
            #endregion

            StarMessaginCenter();
            App.ServerC.SendMessageAsync($"{Comprobante}-{evento._id}");
        }
        #endregion

        #region METODOS
        private async void Async_inicializaciones(List<model_eventos> evento){
            await Task.Delay(1200);
            await Task.Run(() => {
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                evento.RemoveAt(evento.Count - 1);

                _lugar = evento[0]._lugar;
                _fecha = evento[0]._fecha;
                _hora = evento[0]._hora;
                _texto = evento[0]._texto;
                _sourceEvento = evento[0]._sourceEvento;
                _videoEnlace = evento[0]._video;
            });
            IsBusy = false;
            StopMessaginCenter();
        }

        //INICIA EL MESAGING CENTER.
        private void StarMessaginCenter() => MessagingCenter.Subscribe<Message>(this, "cargarEvento", Llamar => { Async_inicializaciones(Llamar.Eventos); });

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        private void StopMessaginCenter() => MessagingCenter.Unsubscribe<Message>(this, "cargarEvento");
        #endregion
    }
}
