using SportLeagueRD.Model;
using System.Collections.Generic;

namespace SportLeagueRD.Messages{
    class Message{
        public object[] Variable { get; set; }
        public List<model_eventos> Eventos { get; set; }
        public List<model_noticias> Noticias { get; set; }
        public List<model_equipos> Equipos { get; set; }
        public List<model_jugadores> Jugadores { get; set; }
        public List<model_marcador> Marcador { get; set; }
        public List<model_lideres> Lideres { get; set; }
        public List<model_puntos_bascketball> PuntosBasquetball { get; set; }
        public List<model_comentarios> Comentarios { get; set; }
    }
}
