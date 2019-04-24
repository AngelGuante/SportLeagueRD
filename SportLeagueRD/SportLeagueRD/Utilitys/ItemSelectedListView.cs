using System.Windows.Input;
using Xamarin.Forms;

namespace SportLeagueRD.Utilitys{

    //ESTA CLASE ES PARA PODER PONER UN EVENTO ICOMMAND A UN LISTVIEW.
    public class ItemSelectedListView : ListView{
        public static BindableProperty ItemClickCommandProperty = BindableProperty.Create(nameof(ItemClickCommand), typeof(ICommand), typeof(ItemSelectedListView), null);

        #region CONSTRUCTOR
        public ItemSelectedListView(ListViewCachingStrategy strategy) : base(strategy){
            ItemTapped += OnItemTapped;
        }
        #endregion

        public ICommand ItemClickCommand{
            get{ return (ICommand)GetValue(ItemClickCommandProperty); }
            set{ SetValue(ItemClickCommandProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e){
            if (e.Item != null){
                ItemClickCommand?.Execute(e.Item);
                SelectedItem = null;
            }
        }
    }
}