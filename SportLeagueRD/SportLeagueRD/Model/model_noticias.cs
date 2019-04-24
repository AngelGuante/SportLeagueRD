using Xamarin.Forms;

namespace SportLeagueRD.Model{
    public class model_noticias : Base_model{
        #region VARIABLES
        private ImageSource Source;
        private ImageSource SourceNoticia;
        #endregion

        //DATOS DE LA NOTICIA
        public string _id { set; get; }
        public string _titulo { set; get; }
        public string _texto { set; get; }
        public string _fecha { set; get; }
        public string _videoEnlace { set; get; }
        public ImageSource _sourceNoticia { get => SourceNoticia;
            set => SourceNoticia = $"http://{App.ServerUrl}{App.ServerImagesPath}{value.ToString().Replace("File: ", "")}";
        }
        //EQUIPO
        public ImageSource _source{ get => Source;
            set{
                Source = $"http://{App.ServerUrl}{App.ServerImagesPath}{value.ToString().Replace("File: ", "")}";
            }
        }
        public string _nombreEquipo { set; get; }
    }
}
