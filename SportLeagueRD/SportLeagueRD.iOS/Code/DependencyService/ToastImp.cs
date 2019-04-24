using SportLeagueRD.iOS.Code.DependencyService;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastImp))]
namespace SportLeagueRD.iOS.Code.DependencyService {
    class ToastImp : IToast {
        public void Show(string message) { }
        //ToastIOS.MakeText(message).Show();
    }
}