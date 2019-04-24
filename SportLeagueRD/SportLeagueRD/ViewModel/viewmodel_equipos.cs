using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace SportLeagueRD.ViewModel{
    public class viewmodel_equipos : Base_viewModel {
        #region VARIABLES
        private bool _busy = true;
        private int Variacion = -1;

        //VARIABLES PARA REALIZAR CAMBIOS EN LA INTERFAZ LUEGO DE HABER CARGADO
        private string BusquedaDeEquipo = "";
        private bool VerTodo = true;

        //ESTAS VARIABLES ALMACENARAN EL NUMERO DE REGISTROS QUE SE VAN A BUSCAR EN EL SERVIDOR Y DESDE DONDE SE EMPEZARA A BUSCAR
        private string CantidadDatosBuscar = "18";
        private string ValorInicial = "0";
        private string ComprobanteEstandar1 = "";
        private string ComprobanteEstandar2 = "";
        private string Comprobante1 = "EQ01";
        private string Comprobante2 = "EQ02";
        private string Comprobante3 = "EB01";
        private string Comprobante4 = "EB02";
        #endregion

        #region PROPIEDADES
        public InfiniteScrollCollection<model_equipos> _lista { set; get; }

        //PARA ALMACENAR LA BUSQUEDA REALIZADA POR EL USUARIO.
        public string _parametroEquipoABuscar {
            get => BusquedaDeEquipo;
            set {
                BusquedaDeEquipo = value;
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
        public ICommand _elementoSeleccionado { get => new Command((item) => { AbrirVentana_detalle(item as model_equipos); }); }

        //PROPIEDAD PARA EL BOTON DE BUSCAR LOS EQUIPOS POR SU NOMBRE
        public ICommand _btn_buscar { get => new Command(() => {
            if (string.IsNullOrWhiteSpace(_parametroEquipoABuscar))
                return;
            LimpiarAntesDeBuscarEquipos(false);
            App.ServerC.SendMessageAsync($"{ComprobanteEstandar2}-{CantidadDatosBuscar}-{ValorInicial}-{_parametroEquipoABuscar}");
        }); }
        
        //PROPIEDAD PARA VER TODOS LOS EQUIPOS
        public ICommand _verTodosEquipos { get => new Command(() => {
            _parametroEquipoABuscar = "";
            LimpiarAntesDeBuscarEquipos(true);
            App.ServerC.SendMessageAsync($"{ComprobanteEstandar1}-{CantidadDatosBuscar}-{ValorInicial}");
        }); }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_equipos(int variacion){
            Variacion = variacion;
            //CADA VEZ QUE EL USUARIO LLEGE AL PIE DE LA PAGINA SE BUSCARAN MAS DATOS AL SERVIDOR.
            _lista = new InfiniteScrollCollection<model_equipos>{
                OnLoadMore = async () => {
                    ValorInicial = _lista.Count.ToString();
                    //DEPENDIENDO DE SI SE ESTA VIENDO TODOS LOS EQUIPOS O SOLO LOS EQUIPOS QUE ENCAJEN CON LA BUSQUEDA ESCRITA EN EL CAMPO
                    if(_verTodo)
                        App.ServerC.SendMessageAsync($"{ComprobanteEstandar1}-{CantidadDatosBuscar}-{ValorInicial}");
                    else
                        App.ServerC.SendMessageAsync($"{ComprobanteEstandar2}-{CantidadDatosBuscar}-{ValorInicial}-{_parametroEquipoABuscar}");
                    return null;
                }
            };
        }
        #endregion

        #region METODOS
        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DEL EQUIPO SELECCIONADO.
        private void AbrirVentana_detalle(model_equipos equipo) => Application.Current.MainPage.Navigation.PushAsync(new view_detalles_equipos(equipo, Variacion));

        //METODO QUE CARGA LOS DATOS EN LA TABLA DE VIEW_EQUIPOS, EL PARAMETRO ES PARA SABER QUE PAQUETE DE DATOS SE VA A TRAER.
        private async void LlenarListView(List<model_equipos> equipos){
            IsBusy = true;
            await Task.Delay(500);
            await Task.Run(() => {
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                equipos.RemoveAt(equipos.Count - 1);

                _lista.AddRange(equipos);
            });
            IsBusy = false;
        }

        //LLENA LA TABLA CON LOS PRIMEROS REGISTROS LA PRIMERA VEZ QUE ESTA PAGINA APAREZCA
        public void LlenarTablaPrimeraVez() {
            if (_lista.Count == 0){
                //SI VARIACION ES IGUAL A 0, NO SE TRAEN LOS VOTOS
                //SI VARIACION ES IGUAL A 1 SE TRAEN LOS VOTOS
                ComprobanteEstandar1 = Variacion == 0 ? Comprobante1 : Comprobante2;
                // PARA LAS BUSQUEDAS
                ComprobanteEstandar2 = Variacion == 0 ? Comprobante3 : Comprobante4;
                App.ServerC.SendMessageAsync($"{ComprobanteEstandar1}-{CantidadDatosBuscar}-{ValorInicial}");
            }
        }

        //ESTE METODO ES PARA REDUCIR CODIGO DE LOS COMANDOS QUE TIENEN QUE VER CON LA PROPIEDAD '_verTodo'
        private void LimpiarAntesDeBuscarEquipos(bool valor) {
            CantidadDatosBuscar = "18";
            ValorInicial = "0";
            _verTodo = valor;
            _lista.Clear();
        }

        //INICIA EL MESAGING CENTER.
        public void StarMessaginCenter(){
            //COMO HAY DOS PAGINAS QUE UTILIZAN ESTA MISMA CLASE PARA MOSTRAR SUS DATOS, HAY QUE SEPARAR LOS MESSAGIN CENTER UTILIZANDO LA VARIABLE "VARIACION" PARA IDENTIFICAR UNO DE OTRO.
            if (Variacion == 0)
                MessagingCenter.Subscribe<Message>(this, "cargarEquipos", Llamar => { LlenarListView(Llamar.Equipos); });
            else
                MessagingCenter.Subscribe<Message>(this, "cargarEquiposRanking", Llamar => { LlenarListView(Llamar.Equipos); });
        }

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        public void StopMessaginCenter(){
            //COMO HAY DOS PAGINAS QUE UTILIZAN ESTA MISMA CLASE PARA MOSTRAR SUS DATOS, HAY QUE SEPARAR LOS MESSAGIN CENTER UTILIZANDO LA VARIABLE "VARIACION" PARA IDENTIFICAR UNO DE OTRO.
            if (Variacion == 0)
                MessagingCenter.Unsubscribe<Message>(this, "cargarEquipos");
            else
                MessagingCenter.Unsubscribe<Message>(this, "cargarEquiposRanking");
        }
        #endregion
    }
}