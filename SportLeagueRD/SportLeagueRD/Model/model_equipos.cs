using Xamarin.Forms;

namespace SportLeagueRD.Model{
    public class model_equipos : Base_model{
        #region VARIABLES
        private ImageSource Source;
        #endregion

        //DATOS DE EL EQUIPO
        public string _id { get; set; }
        public ImageSource _source { get => Source;
            set{
                //BUSCO EN LA CADENA RECIVIDA SI SE A MANDADO COMO UNA RUTA AL SERVIDOR O COMO NOMBRE DIRECTO DE LA IMAGEN,
                //SI FUE COMO UNA RUTA SE REMUEVE EL NOMBRE DIRECTO DE LA IMAGEN, ANTES DE ASIGNARLA ALA PROPIEDAD.
                string ruta = value.ToString().Substring(value.ToString().LastIndexOf("/") + 1);
                Source = $"http://{App.ServerUrl}{App.ServerImagesPath}{ruta.ToString().Replace("File: ", "")}";
            }
        }
        public string _nombreEquipo { set; get; }
        public string _siglas { set; get; }
        public string _representante { set; get; }
        public string _localidad { set; get; }
        public string _ubicacionLocalidad { set; get; }
        public string _votos { set; get; }
        //INDICARA EL ESTADO DE EL EQUIPO EN EL QUE SE ENCUENTRA EN DICHO MOMENTO SEGUN EL VALOR DE ESTA PROPIEDAD SE PODRA DETERMINAR LO SIGUIENTE:
        //  0 - ACTIVO
        //  1 - SUSPENDIDO
        //  2 - RETIRADO
        public string _estado { set; get; }



        private bool siguiendo = false;
        public bool _switcherValue { get => siguiendo;
            set {
                siguiendo = value;
                DependencyService.Get<IToast>().Show("Pressed " + value);
                OnPropertyChanged();
            }
        }
    }
}
