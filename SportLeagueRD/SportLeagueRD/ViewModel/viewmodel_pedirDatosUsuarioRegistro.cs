using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class viewmodel_pedirDatosUsuarioRegistro : Base_viewModel{
        #region VARIABLES
        private string UserEmail = "";
        private int Fuente = 0;
        private readonly string Comprobante = "US01";
        private readonly string Comprobante1 = "EQ01";
        private bool _busy = false;
        private bool SeguirEquiposVisible = true;
        #endregion

        #region PROPERTIES
         public bool _MostrarEquiposASeguir { get => SeguirEquiposVisible;
            set {
                SeguirEquiposVisible = value;
                OnPropertyChanged();
            }
        }
        public string _entryUserName { get; set; } = "";

        public ObservableCollection<model_equipos> _equipos { get; set; }

        //PROPIEDAD QUE DETERMINA SI ESTA PAGINA ESTA REALIZANDO ALGUN TRABAJO, ASI OCULTARLA DEBAJO DE UNA PAGINA CON UN ActivityIndicator
        public bool IsBusy { get => _busy;
            set {
                _busy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ICOMMANDS
        public ICommand _continuar { get; set; }
        public ICommand _btnSiguiente { get; set; }
        #endregion

        #region CONSTRUCTOR
        //  EL CONSTRUCTOR PIDE COMO PARAMETRO 2 DATOS, userEmail ES O EL EMAIL DEL USUARIO EN CASO DE QUE SE REGISTRE CON GOOGLE
        //  O DIRECTAMENTE INGRESANDO SU CORREO, Y EN CASO DE SER CON FB GUARDARA EL ID DE USUARIO.
        //  EL SEGUNDO PARAMETRO ALMACENA EL TIPO DE REGISTRO QUE HIZO, SI FUE POR:
        //  1 - EMAIL
        //  2 - GOOGLE
        //  3 - FACEBOOK
        public viewmodel_pedirDatosUsuarioRegistro(string userEmail, int fuente) {
            _equipos = new ObservableCollection<model_equipos>();

            Fuente = fuente;
            UserEmail = userEmail;
            _continuar = new Command(Continuar);
            _btnSiguiente = new Command(RegistrarUsuarioNuevo);
        }
        #endregion

        #region METHODS
        //METODO QUE CARGA LOS EQUIPOS QUE PUEDE SEGUIR EL USUARIO
        private async void LlenarListView(List<model_equipos> equipos) {
            IsBusy = true;
            await Task.Run(() => {
                //ELIMINAR EL ULTIMO REGISTRO QUE PERTENECE AL 'comprobante'
                equipos.RemoveAt(equipos.Count - 1);

                foreach (var tmp in equipos)
                    _equipos.Add(tmp);
            });
            StopMessaginCenter();
            IsBusy = false;
        }

        //SI EL USUARIO A INGRESADO UN NOMBRE CON MAS DE 2 CARACTERES, ESTE GUARDARA SUS DATOS.
        private async void RegistrarUsuarioNuevo() {
            string EquiposSeguidosID = string.Join(",", _equipos.Where(e => e._switcherValue)
                                                                .Select(i => i._id));
            if(EquiposSeguidosID.Length > 0) {
                //  GUARDARLO EN LA BASE DE DATOS
                await App.DB.UpdateItemAsync(new Entity_usuario {
                    ID = 1,
                    Nombre = _entryUserName,
                    Correo = UserEmail,
                    EquiposSeguidosID = EquiposSeguidosID
                });
                //  GUARDARLO EN EL SERVIDOR
                App.ServerC.SendMessageAsync($"{Comprobante}-{UserEmail}-{_entryUserName}-{Fuente}-{EquiposSeguidosID}");
                await Application.Current.MainPage.Navigation.PushAsync(new mdp()); 
            }
            else
                DependencyService.Get<IToast>().Show("Debe seleccionar almenos un equipo.");
        }

        //  ESTE METODO PIDE EL NOMBRE DE USUARIO Y LUEGO LO MANDA A LA VENTANA DE SELECCION DE EQUIPOS A SEGUIR
        private void Continuar() {
            int CaracteresMinimosNombreUsuario = 2;
            if (_entryUserName.Length > CaracteresMinimosNombreUsuario) {
                _MostrarEquiposASeguir = false;
                IsBusy = true;
                StarMessaginCenter();
                App.ServerC.SendMessageAsync($"{Comprobante1}");
            } else
                DependencyService.Get<IToast>().Show($"Debe ingresar un nombre de mas de {CaracteresMinimosNombreUsuario} caracteres.");
        }

        //INICIA EL MESAGING CENTER.
        private void StarMessaginCenter() => MessagingCenter.Subscribe<Message>(this, "cargarEquipos", Llamar => { LlenarListView(Llamar.Equipos); });

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        private void StopMessaginCenter() => MessagingCenter.Unsubscribe<Message>(this, "cargarEquipos");
        #endregion
    }
}
