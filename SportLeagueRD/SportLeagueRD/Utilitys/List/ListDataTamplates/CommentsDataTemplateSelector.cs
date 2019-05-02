using SportLeagueRD.Model;
using Xamarin.Forms;

namespace SportLeagueRD.Utilitys{
    //ESTA CLASE SE ENCARGA DE RETORNAR EL TEMPLATE CORRESPONDIENTE PARA LOS COMENTSRIOS.
    class CommentsDataTemplateSelector : DataTemplateSelector{
        public DataTemplate FromTemplate { get; set; }
        public DataTemplate ToTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container){
            //EN CASO DE QUE LA PROPIEDAD DEL OBJETO COMENTARIO, PROCEDENCIA SE ENVIARA UNA PLANTILLA U OTRA.
            return ((model_comentarios)item)._procedencia == 1 ? FromTemplate : ToTemplate;
        }
    }
}
