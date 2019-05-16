using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SportLeagueRD.Model{
    public class Base_model : INotifyPropertyChanged{

        #region INotityPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
