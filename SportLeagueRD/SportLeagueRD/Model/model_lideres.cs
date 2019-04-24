using Xamarin.Forms;

namespace SportLeagueRD.Model{
    class model_lideres{
        #region VARIABLES
        private ImageSource SourceJugadorA;
        private ImageSource SourceJugadorB;
        #endregion

        public string _id { get; set; }
        //EQUIPO A
        public string _idJugadorA { get; set; }
        public ImageSource _sourceJugadorA {
            get => SourceJugadorA;
            set {
                //EN CASO DE, PARA ENTENDER ESTA LINEA IR A OTRO DE LOS MODELOS QUE HACEN ESTE MISMO PROCESO, AQUI EL PROCESO ES MAS LARGO PARA EVITAR CREAR VARIABLES INECESARIAS.
                SourceJugadorA = value.ToString().Equals("File: ") ? "" : $"http://{App.ServerUrl}{App.ServerImagesPath}{(value.ToString().Substring(value.ToString().LastIndexOf("/") + 1)).ToString().Replace("File: ", "")}";
            }
        }
        public string _nombreJugadorA { get; set; }
        public string _apellidoJugadorA { get; set; }
        public string _posicionJugadorA { get; set; }
        //ALMACENARA LA CANTIDAD DEPENDIENDO DE LO QUE SE ESTE HABLANDO, SI ES LIDER EN PUNTOS, EN REBOTES, ETC..
        public string _totalJugadorA { get; set; }
        //EQUIPO B
        public string _idJugadorB { get; set; }
        public ImageSource _sourceJugadorB {
            get => SourceJugadorB;
            set {
                //EN CASO DE, PARA ENTENDER ESTA LINEA IR A OTRO DE LOS MODELOS QUE HACEN ESTE MISMO PROCESO, AQUI EL PROCESO ES MAS LARGO PARA EVITAR CREAR VARIABLES INECESARIAS.
                SourceJugadorB = value.ToString().Equals("File: ") ? "" : $"http://{App.ServerUrl}{App.ServerImagesPath}{(value.ToString().Substring(value.ToString().LastIndexOf("/") + 1)).ToString().Replace("File: ", "")}";
            }
        }
        public string _nombreJugadorB { get; set; }
        public string _apellidoJugadorB { get; set; }
        public string _posicionJugadorB { get; set; }
        //ALMACENARA LA CANTIDAD DEPENDIENDO DE LO QUE SE ESTE HABLANDO, SI ES LIDER EN PUNTOS, EN REBOTES, ETC..
        public string _totalJugadorB { get; set; }
        //VIDEOS QUE MUESTRAN LO MAS DESTACADO DE CADA EQUIPO
        public string _loMasDestacadoA { get; set; }
        public string _loMasDestacadoB { get; set; }
    }
}