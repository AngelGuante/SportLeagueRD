using Xamarin.Forms;

namespace SportLeagueRD.Model{
    public class model_jugadores : Base_model{
        #region VARIABLES
        //Almacenara el numero del jugador luego de que un metodo lo evalue.
        private string Numero;
        private ImageSource Source;
        private ImageSource SourceEquipo;
        private string Posicion;
        #endregion

        //DATOS DEL JUGADOR
        public string _idJugador { set; get; }
        public ImageSource _source{
            get => Source;
            set {
                //EN CASO DE, PARA ENTENDER ESTA LINEA IR A OTRO DE LOS MODELOS QUE HACEN ESTE MISMO PROCESO, AQUI EL PROCESO ES MAS LARGO PARA EVITAR CREAR VARIABLES INECESARIAS.
                Source = value.ToString().Equals("File: ") ? "" : $"http://{App.ServerUrl}{App.ServerImagesPath}{(value.ToString().Substring(value.ToString().LastIndexOf("/") + 1)).ToString().Replace("File: ", "")}";
            }
        }
        public string _nombreJugador { set; get; }
        public string _apellidoJugador { set; get; }
        public string _votos { set; get; }
        public string _fechaNacimientoEdad { set; get; }
        //ESTADISTICAS DEL JUGADOR
        public string _MJ { get; set; }
        public string _JJ { get; set; }
        public string _RB { get; set; }
        public string _A { get; set; }
        public string _RO { get; set; }
        public string _F { get; set; }
        public string _BL { get; set; }
        public string _BP { get; set; }
        public string _T2H { get; set; }
        public string _T2F { get; set; }
        public string _T3H { get; set; }
        public string _T3F { get; set; }
        public string _TLH { get; set; }
        public string _TLF { get; set; }

        // **PROPIEDADES DE EL EQUIPO AL QUE PERTENECE**
        public string _idEquipo { get; set; }
        public string _posicion {
            get => Posicion;
            set => Posicion = VerificarPropiedadPosicion(value);
        }
        public ImageSource _sourceEquipo { get => SourceEquipo;
            set { SourceEquipo = $"http://{App.ServerUrl}{App.ServerImagesPath}{value.ToString().Replace("File: ", "")}"; }
        }
        public string _nombreEquipo { get; set; }
        public string _siglasEquipo { get; set; }
        public string _fechaIngreso { get; set; }
        //PROPIEDAD QUE LLAMARA A UN METODO PARA EVALUAR EL NUMERO DEL JUGADOR.
        public string _numero{
            set { VerificarPropiedadNumero(value); }
            get { return Numero; }
        }

        #region METODOS
        //DEPENDIENDO DE EL PARAMETRO SI TIENE 1 SOLO CARACTER LE AGREGARA 2 ESPACIOS DELANTE A ESE PARAMETRO.
        private void VerificarPropiedadNumero(string _numero) => Numero = _numero.Length == 1 ? _numero + "  " : _numero;

        //  DEPENDIENDO DE EL VALOR DE LA PROPIEDRAS TRAIDA DEL SERVER, SE LE ASIGNARA EL NOMBRE CORRESPONDIENTE A LA POSICION
        private string VerificarPropiedadPosicion(string posicion) {
            switch (posicion) {
                case "1":
                    return "Base";
                case "2":
                    return "Alero";
                case "3":
                    return "Pivot";
                case "4":
                    return "Ala-Pivot";
                default:
                    return posicion;
            }
        }
        #endregion
    }
}
