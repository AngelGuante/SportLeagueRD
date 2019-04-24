namespace SportLeagueRD.Model{
    public class model_comentarios{
        public string _usuario { set; get; }
        public string _comentario { set; get; }
        //PROCEDENCIA ES PARA VERIFICAR SI EL COMENTARIO ES RECIBIDO DESDE EL SERVIDOR O EL USUARIO ES QUIEN LO A ENVIADO DIRECTAMENTE Y DEMAS.
        //VALORES POR DEFECTO:
        // 1 - RECIVIDO DESDE EL SERVIDOR
        // 2 - ENVIADO POR EL USUARIO
        public int _procedencia { set; get; }
    }
}
