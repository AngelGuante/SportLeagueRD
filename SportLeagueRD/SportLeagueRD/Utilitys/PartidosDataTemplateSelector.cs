using SportLeagueRD.Model;
using Xamarin.Forms;

namespace SportLeagueRD.Utilitys{
    //ESTA CLASE SE ENCARGA DE RETORNAR EL TEMPLATE CORRESPONDIENTE PARA LOS PARTIDOS.
    class PartidosDataTemplateSelector : DataTemplateSelector{
        public DataTemplate PartidosPasados { get; set; }
        public DataTemplate PartidosFuturos { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container){
            DataTemplate elemento = null;
            try{
                string estado = ((model_marcador)item)._estado;
                //SI ESTADO ES PENDIENTE O EN VIVO SE TRAE LA MISMA PLANTILLA.
                elemento = (estado.Equals("7") || estado.Equals("8")) ? PartidosFuturos : PartidosPasados;
            }catch (System.NullReferenceException) { }
            return elemento;
        }
    }
}