using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    class viewmodel_calendario : Base_viewModel{
        #region VARIABLES
        private bool _busy = true;

        private DateTime Fecha;
        
        private DatePicker picker;

        private bool HayPartido = false;

        //ESTAS VARIABLES ALMACENARAN EL NUMERO DE REGISTROS QUE SE VAN A BUSCAR EN EL SERVIDOR Y DESDE DONDE SE EMPEZARA A BUSCAR
        private string Comprobante = "MC01";
        #endregion

        #region PROPIEDADES
        public bool _hayPartidosEnFecha {
            get => HayPartido;
            set {
                HayPartido = value;
                OnPropertyChanged();
            }
        }

        public DateTime _fechaBuscador {
            get { return Fecha; }
            set {
                Fecha = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<model_marcador> _lista { set; get; }

        //PROPIEDAD QUE DETERMINA SI ESTA PAGINA ESTA REALIZANDO ALGUN TRABAJO, ASI OCULTARLA DEBAJO DE UUNA PAGINA CON UN ActivityIndicator
        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ICOMMNADS
        //ICOMMAND PARA CUANDO SE SELECCIONE UN ELEMENTO DEL VIEW CELL.
        public ICommand _elementoSeleccionado { get => new Command((item) => { AbrirVentana_detalle(item as model_marcador); }); } 

        //BOTONES QUE AGREGAN Y QUITAN UN DIA A LA FECHA.s
        public ICommand _btn_fechaAnterior { get; set; }
        public ICommand _btn_fechaSiguiente { get; set; }
        #endregion

        #region CONSTRUCTOR
        public viewmodel_calendario(DatePicker picker){
            _lista = new ObservableCollection<model_marcador>();

            Async_inicializaciones(picker);

            App.ServerC.SendMessageAsync($"{Comprobante}-{DateTime.Now.ToString("dd/MM/yyyy")}");

            _btn_fechaAnterior = new Command((day) => {
                _fechaBuscador = _fechaBuscador.AddDays(double.Parse(day.ToString())).Date;
            });
            _btn_fechaSiguiente = new Command((day) => {
                _fechaBuscador = _fechaBuscador.AddDays(double.Parse(day.ToString())).Date;
            });
        }
        #endregion

        #region METODOS
        //ABRE LA VENTANA DE DATALLES DE EVENTO PARA MOSTRAR LOS DETALLES DE EL PARTIDO SELECCIONADO.
        private void AbrirVentana_detalle(model_marcador calendario){
            //SI EL _estado ES IGUAL A 7 u 8, ES PORQUE ES UN PARTIDO QUE AUN NO A PASADO ASI QUE SE ABRE EN UNA VENTANA Y SI NO, ES PORQUE EL PARTIDO YA PASO
            //Y SE ABRE OTR VENTANA.
            if (calendario._estado.Equals("7") || calendario._estado.Equals("8")) Application.Current.MainPage.Navigation.PushAsync(new view_detalles_calendario(calendario));
            else Application.Current.MainPage.Navigation.PushAsync(new view_detalles_marcador(calendario));
        }

        //LLENA LA LISTA DE MANERA ASINCRONA
        private async void Async_llenarListView(List<model_marcador> marcador){
            _lista.Clear();
            if(_lista.Count == 0)
                await Task.Delay(300);
            await Task.Run(() => {
                marcador.RemoveAt(marcador.Count - 1);

                foreach (model_marcador tmp in marcador) {
                    //VERIFICO SI SE A MANDADO ALGUNA FECHA DEL SERVIDOR, DE NO SER ASI ES PORQUE EL PARTIDO ES EN VIVO ASI QUE SE MUESTRA 'EN VIVO' EN LA ETIQUETA DE FECHA.
                    tmp._fecha = tmp._fecha.Equals("") ? " • EN VIVO" : tmp._fecha;
                    _lista.Add(tmp);
                }

                IsBusy = false;
                _hayPartidosEnFecha = _lista.Count > 0 ? false : true;
            });
        }

        //REALIZA LAS INICIALIZACIONES DE MANERA ASINCRONA INTENTANDO NO FORZAR TANTO EL HILO PRINCIPAL
        private async void Async_inicializaciones(DatePicker picker){
            await Task.Run(() => {
                this.picker = picker;
                this.picker.DateSelected += Picker_SelectedEvent;

                _fechaBuscador = DateTime.Now.Date;
            });
        }

        //EVENTO DEL PICKER PARA LLENAR EL LISTVIEW CON LOS PARTIDOS DE LA FECHA NUEVA SELECCIONADA
        private void Picker_SelectedEvent(object sender, DateChangedEventArgs e){
            if ((e.NewDate != e.OldDate) && (e.OldDate > new DateTime(2000, 1, 1))){
                IsBusy = true;
                App.ServerC.SendMessageAsync($"{Comprobante}-{e.NewDate.ToString("dd/MM/yyyy")}");
            }
        }

        //INICIA EL MESAGING CENTER.
        public void StarMessaginCenter() => MessagingCenter.Subscribe<Message>(this, "cargarMarcador", Llamar => { Async_llenarListView(Llamar.Marcador); });

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        public void StopMessaginCenter() => MessagingCenter.Unsubscribe<Message>(this, "cargarMarcador");
        #endregion
    }
}
