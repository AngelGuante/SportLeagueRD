using Android.Widget;
using SportLeagueRD.Droid.Code;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastImp))]
namespace SportLeagueRD.Droid.Code {
    class ToastImp : IToast {
        void IToast.Show(string message) => Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
    }
}