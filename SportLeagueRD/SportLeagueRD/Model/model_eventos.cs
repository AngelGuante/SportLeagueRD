using Xamarin.Forms;

namespace SportLeagueRD.Model{
    public class model_eventos : Base_model{
        #region VARIABLES
        private ImageSource sourceEvento; 
        #endregion

        public string _id { set; get; }
        public string _titulo { set; get; }
        public string _lugar { set; get; }
        public string _fecha { set; get; }
        public string _hora { set; get; }
        public string _texto { set; get; }
        public ImageSource _sourceEvento { get => sourceEvento;
            set{
                sourceEvento = $"http://{App.ServerUrl}{App.ServerImagesPath}{value.ToString().Replace("File: ", "")}";
            }
        }
        public string _video { set; get; }
    }
}
