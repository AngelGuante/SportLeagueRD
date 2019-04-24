using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SportLeagueRD.Model{
    public class model_marcador : INotifyPropertyChanged {
        #region VARIABLES
        private string fecha = "";
        private string NombreEquipoA = "";
        private ImageSource SourceA;
        private ImageSource SourceB;
        #endregion

        //INDICARA EL ESTADO FINAL DEL PARTIDO SEGUN EL VALOR DE ESTA PROPIEDAD SE PODRA DETERMINAR LO SIGUIENTE:
        //  1 - SUSPENDIDO
        //  4 - EQUIPO A GANADOR
        //  5 - EQUIPO B GANADOR
        //  6 - EMPATE
        //  7 - PENDIENTE
        //  8 - EN VIVO
        public string _estado { set; get; }

        //EQUIPO A
        public string _idA { set; get; }
        public ImageSource _sourceA {
            get => SourceA;
            set {
                //EN CASO DE, PARA ENTENDER ESTA LINEA IR A OTRO DE LOS MODELOS QUE HACEN ESTE MISMO PROCESO, AQUI EL PROCESO ES MAS LARGO PARA EVITAR CREAR VARIABLES INECESARIAS.
                SourceA = value.ToString().Equals("File: ") ? "" : $"http://{App.ServerUrl}{App.ServerImagesPath}{(value.ToString().Substring(value.ToString().LastIndexOf("/") + 1)).ToString().Replace("File: ", "")}";
                }
        }
        public string _nomnbreEquipoA {
            get => NombreEquipoA;
            set {
                NombreEquipoA = value;
                OnPropertyChanged();
            }
        }
        public string _siglasA { set; get; }
        public string _puntuacionEquipoA { set; get; }
        public string _votosA { get; set; }
        public bool _AGaanador { get { return _estado == "4" ? true : false; } }
        //ESTA PROPIEDAD SOLO ES PARA LA VENTANA DE MARCADOR
        public Color _colorTextoA { get { return _estado == "4" ? Color.White : Color.Black; } }
        //ESTA PROPIEDAD ES PARA PODER DARLE INTERACTIVIDAD A CIERTAS PAGINAS, NO SE LLENARAN DESDE EL SERVIDOR SI NO QUE DEPENDIENDO EL 
        //VALOR DE LAP PROPIEDAD _estado, ESTA PROPIEDAD ALMACENARA UN COLOR CORRESPONDIENTE.
        public Color _coLorA { get { return AsignarColorEquipo(); } }

        //EQUIPO B
        public string _idB { set; get; }
        public ImageSource _sourceB {
            get => SourceB;
            set =>
                SourceB = $"http://{App.ServerUrl}{App.ServerImagesPath}{value.ToString().Replace("File: ", "")}";
        }
        public string _nomnbreEquipoB { set; get; }
        public string _siglasB { set; get; }
        public string _puntuacionEquipoB { set; get; }
        public string _votosB { get; set; }
        public bool _BGaanador { get { return _estado == "5" ? true : false; } }
        //ESTA PROPIEDAD SOLO ES PARA LA VENTANA DE MARCADOR
        public Color _colorTextoB { get { return _estado == "5" ? Color.White : Color.Black; } }

        //DATOS GENERALES
        public string _id { get; set; }
        public string _fecha { get => fecha;
            set {
                fecha = $" {value}";
                OnPropertyChanged();
            }
        }
        public string _hora { set; get; }
        public string _localidad { set; get; }
        public string _descripcionEstadoPartido {
            get {
                if (_estado == "1") return "SUSPENDIDO";
                else if (_estado == "6") return "EMPATE";
                else return "TERMINADO";
            }
        }

        //  ESTA VARIABLE ES PARA MOSTRAR LOS VOTONES DE VOTOS O NO EN EL MARCADOR SI EL USUARIO YA VOTO LOS VOTONES NO SE MUESTRAN
        public string _usuarioVotado { get; set; }

        //SI EL PARTIDO ES EN VIVO LE COLOCA EL COLOR DE LA FECHA EN ROJO
        public Color _colorFecha { get => string.IsNullOrEmpty(_hora) ? Color.Red : Color.FromHex("57AF5C"); }

        #region METODOS
        //SEGUN EL VALOR DE LA PROPIEDAD _estado ASIGNARA LOS COLORES CORRESPONDIENTES
        private Color AsignarColorEquipo()
        {
            switch (_estado)
            {
                case "6":
                    return Color.FromHex("39A1BF");
                case "1":
                    return Color.Black;
                case "4":
                    return Color.FromHex("57AF5C");
                case "5":
                    return Color.FromHex("FF2027");
                default:
                    return Color.White;
            }
        }
        #endregion

        #region INotityPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
