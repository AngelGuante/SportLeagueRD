using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_detalles_noticias : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        private string Titulo;
        private string Fecha;
        private ImageSource Imagen;
        private string Texto;
        private string VideoEnlace;

        private string Comprobante = "NO02";
        #endregion

        #region PROPIEDADES
        public string _titulo {
            get => Titulo;
            set {
                Titulo = value;
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
        public ImageSource _sourceNoticia {
            get => Imagen;
            set {
                Imagen = value;
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
        public string _videoEnlace {
            get => VideoEnlace;
            set {
                VideoEnlace = value;
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

        #region COSTRUCTOR
        public viewmodel_detalles_noticias(model_noticias noticia){
            #region INICIALIZAR PROPIEDSADES DEL MODEL EQUIPO QUE VIENEN DE LA VENTANA ANTERIOR
            _titulo = noticia._titulo;
            _fecha = noticia._fecha;
            #endregion
            StarMessaginCenter();
            App.ServerC.SendMessageAsync($"{Comprobante}-{noticia._id}");
        }
        #endregion

        #region METODOS
        private async void Async_inicializaciones(List<model_noticias> noticia){
            await Task.Delay(1200);
            await Task.Run(() => {
                #region INICIALIZAR PROPIEDADES DEL MODEL EQUIPO QUE NO VIENEN DE LA VENTANA ANTERIOR
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                noticia.RemoveAt(noticia.Count - 1);

                _texto = noticia[0]._texto;
                _sourceNoticia = noticia[0]._sourceNoticia;
                _videoEnlace = noticia[0]._videoEnlace;
                #endregion
            });
            IsBusy = false;
            StopMessaginCenter();
        }

        //INICIA EL MESAGING CENTER.
        private void StarMessaginCenter() => MessagingCenter.Subscribe<Message>(this, "cargarNoticia", Llamar => { Async_inicializaciones(Llamar.Noticias); });

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        private void StopMessaginCenter() => MessagingCenter.Unsubscribe<Message>(this, "cargarNoticia");
        #endregion
    }
}
