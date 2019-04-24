using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.Utilitys{
    //ESTA CLASE LA UTILIZO PARA PODER UTILIZAR EL VALOR OPUESTO DE UNA PROPIEDAD BOOLEANA DESDE EL XAML
    public class InverseBoolConverter : IValueConverter, IMarkupExtension{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture){
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture){
            return value;
        }

        public object ProvideValue(IServiceProvider serviceProvider){
            return this;
        }
    }
}