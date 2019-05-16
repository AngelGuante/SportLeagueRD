using SportLeagueRD.Model;
using SportLeagueRD.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel {
    class viewmodel_pedirDatosUsuarioRegistro : Base_viewModel{
        #region VARIABLES
        private string UserEmail = "";
        private int Fuente = 0;
        private readonly string Comprobante = "US01";
        #endregion

        #region PROPERTIS
        public string _entryUserName { get; set; } = "";

        public ObservableCollection<model_equipos> _equipos { get; set; }
        #endregion

        public ICommand _continuar { get; set; }
        public ICommand _btnSiguiente { get; set; }

        //  EL CONSTRUCTOR PIDE COMO PARAMETRO 2 DATOS, userEmail ES O EL EMAIL DEL USUARIO EN CASO DE QUE SE REGISTRE CON GOOGLE
        //  O DIRECTAMENTE INGRESANDO SU CORREO, Y EN CASO DE SER CON FB GUARDARA EL ID DE USUARIO.
        //  EL SEGUNDO PARAMETRO ALMACENA EL TIPO DE REGISTRO QUE HIZO, SI FUE POR:
        //  1 - EMAIL
        //  2 - GOOGLE
        //  3 - FACEBOOK
        public viewmodel_pedirDatosUsuarioRegistro(string userEmail, int fuente) {
            _equipos = new ObservableCollection<model_equipos> {
                new model_equipos() {
                    _id = "1",
                    _nombreEquipo = "Nombre 1",
                    _siglas = "S33GL1"
                },
                new model_equipos() {
                    _id = "2",
                    _nombreEquipo = "Nombre 2",
                    _siglas = "SG32L1"
                },
                new model_equipos() {
                    _id = "3",
                    _nombreEquipo = "Nombre 3",
                    _siglas = "SG23L1"
                }
            };

            Fuente = fuente;
            UserEmail = userEmail;
            _continuar = new Command(continuar);
            _btnSiguiente = new Command(RegistrarUsuarioNuevo);
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
                    EquiposSeguidosID = ""
                });
                //  GUARDARLO EN EL SERVIDOR
                App.ServerC.SendMessageAsync($"{Comprobante}-{UserEmail}-{_entryUserName}-{Fuente}");
                //  Mandar al usuario al servidor
                await Application.Current.MainPage.Navigation.PushAsync(new mdp()); 
            }
            else
                DependencyService.Get<IToast>().Show("Debe seleccionar almenos un equipo.");
        }

        private void continuar() {
            int CaracteresMinimosNombreUsuario = 2;
            if (_entryUserName.Length > CaracteresMinimosNombreUsuario) {
                _uno = false;
            } else
                DependencyService.Get<IToast>().Show($"Debe ingresar un nombre de mas de {CaracteresMinimosNombreUsuario} caracteres.");

        }

        private bool Uno = true;
        public bool _uno { get => Uno;
            set {
                Uno = value;
                OnPropertyChanged();
            }
        }
    }
}
