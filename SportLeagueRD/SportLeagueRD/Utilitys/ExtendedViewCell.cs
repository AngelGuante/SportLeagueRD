using Xamarin.Forms;

namespace SportLeagueRD.Utilitys{

    //ESTA CLASE ES PARA PODER COSTUMIZAR EL COLOR DE EL ITEM SELECCIONADO DEL LISTVIEW.
    public class ExtendedViewCell : ViewCell{
        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create("SelectedBackgroundColor",
                                    typeof(Color),
                                    typeof(ExtendedViewCell),
                                    Color.Default);

        public Color SelectedBackgroundColor{
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }
    }
}