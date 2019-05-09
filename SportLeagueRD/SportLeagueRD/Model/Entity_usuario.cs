using SQLite;
namespace SportLeagueRD.Model {
    public class Entity_usuario {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        [NotNull]
        public string Nombre { get; set; }
        [NotNull]
        public string Correo { get; set; }
        public string EquipoFavorito_id { get; set; }
        public string EquipoFavorito_nombre { get; set; }
        public string JugadorFavorito_id { get; set; }
        public string JugadorFavorito_nombre { get; set; }
        public string JugadorFavorito_EquipoId { get; set; }
        public string BaseFavorito_id { get; set; }
        public string BaseFavorito_nombre { get; set; }
        public string BaseFavorito_EquipoId { get; set; }
        public string AleroFavorito_id { get; set; }
        public string AleroFavorito_nombre { get; set; }
        public string AleroFavorito_EquipoId { get; set; }
        public string PivotFavorito_id { get; set; }
        public string PivotFavorito_nombre { get; set; }
        public string PivotFavorito_EquipoId { get; set; }
        public string AlaPivotFavorito_id { get; set; }
        public string AlaPivotFavorito_nombre { get; set; }
        public string AlaPivotFavorito_EquipoId { get; set; }
    }
}
