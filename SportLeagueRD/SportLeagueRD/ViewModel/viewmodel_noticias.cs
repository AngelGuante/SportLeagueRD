using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace SportLeagueRD.ViewModel{
    class viewmodel_noticias : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        //ESTAS VARIABLES ALMACENARAN EL NUMERO DE REGISTROS QUE SE VAN A BUSCAR EN EL SERVIDOR Y DESDE DONDE SE EMPEZARA A BUSCAR
        private string CantidadDatosBuscar = "18";
        private string ValorInicial = "0";
        private string Comprobante = "NO01";
        #endregion

        #region PROPIEDADES
        public InfiniteScrollCollection<model_noticias> _lista { set; get; }

        //PROPIEDAD QUE DETERMINA SI ESTA PAGINA ESTA REALIZANDO ALGUN TRABAJO, ASI OCULTARLA DEBAJO DE UNA PAGINA CON UN ActivityIndicator
        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion
        
        #region ICOMMNADS
        //ICOMMAND PARA CUANDO SE SELECCIONE UN ELEMENTO DEL VIEW CELL.
        public ICommand _elementoSeleccionado{
            get{ return new Command((item) => { AbrirVentana_detalle(item as model_noticias); }); }
        }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_noticias(){
            //CADA VEZ QUE EL USUARIO LLEGE AL PIE DE LA PAGINA SE BUSCARAN MAS DATOS AL SERVIDOR.
            _lista = new InfiniteScrollCollection<model_noticias>{
                OnLoadMore = async () => {
                    ValorInicial = _lista.Count.ToString();
                    App.ServerC.SendMessageAsync($"{Comprobante}-{CantidadDatosBuscar}-{ValorInicial}");
                    return null;
                }
            };
        }
        #endregion

        #region METODOS
        //ABRE LA VENTANA DE DATALLES DE NOTICIA PARA MOSTRAR LOS DETALLES DE LA NOTICIA SELECCIONADA.
        private void AbrirVentana_detalle(model_noticias noticia) => Application.Current.MainPage.Navigation.PushAsync(new view_detalles_noticias(noticia));

        //METODO QUE CARGA LOS DATOS EN LA TABLA DE VIEW_EQUIPOS, EL PARAMETRO ES PARA SABER QUE PAQUETE DE DATOS SE VA A TRAER.
        public async void LlenarListView(List<model_noticias> noticias){
            IsBusy = true;
            await Task.Delay(500);
            await Task.Run(() => {
                _lista.AddRange(noticias);
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                _lista.RemoveAt(_lista.Count - 1);
            });
            IsBusy = false;
        }

        //INICIA EL MESAGING CENTER.
        public void StarMessaginCenter() =>
            MessagingCenter.Subscribe<Message>(this, "cargarNoticias", Llamar => {
                LlenarListView(Llamar.Noticias);
            });

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        public void StopMessaginCenter() => MessagingCenter.Unsubscribe<Message>(this, "cargarNoticias");

        //LLENA LA TABLA CON LOS PRIMEROS REGISTROS LA PRIMERA VEZ QUE ESTA PAGINA APAREZCA
        public void LlenarTablaPrimeraVez(){
            if(_lista.Count == 0)
                App.ServerC.SendMessageAsync($"{Comprobante}-{CantidadDatosBuscar}-{ValorInicial}");
        }
        #endregion
    }
}
