using Xamarin.Forms;

namespace SportLeagueRD.Utilitys{

    //ESTA CLASE ME PERMITE PONER UN UNDERLINE A UN LABEL
    public class UnderlineEffect : RoutingEffect{
        public const string EffectNamespace = "Example";

        public UnderlineEffect() : base($"{EffectNamespace}.{nameof(UnderlineEffect)}"){}
    }
}