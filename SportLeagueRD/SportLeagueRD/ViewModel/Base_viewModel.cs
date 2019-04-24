using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SportLeagueRD.ViewModel{
    public class Base_viewModel: INotifyPropertyChanged {
        #region INotityPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
