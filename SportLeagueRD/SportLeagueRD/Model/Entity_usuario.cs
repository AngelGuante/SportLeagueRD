﻿using SQLite;
namespace SportLeagueRD.Model {
    public class Entity_usuario {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        [NotNull]
        public string Nombre { get; set; }
        [NotNull]
        public string Correo { get; set; }
        [NotNull]
        public string EquipoFavorito_id { get; set; }
        [NotNull]
        public string EquipoFavorito_nombre { get; set; }
        [NotNull]
        public string JugadorFavorito_id { get; set; }
        [NotNull]
        public string JugadorFavorito_nombre { get; set; }
        [NotNull]
        public string JugadorFavorito_Equipo { get; set; }
        [NotNull]
        public string BaseFavorito_id { get; set; }
        [NotNull]
        public string BaseFavorito_nombre { get; set; }
        [NotNull]
        public string BaseFavorito_Equipo { get; set; }
        [NotNull]
        public string AleroFavorito_id { get; set; }
        [NotNull]
        public string AleroFavorito_nombre { get; set; }
        [NotNull]
        public string AleroFavorito_Equipo { get; set; }
        [NotNull]
        public string PivotFavorito_id { get; set; }
        [NotNull]
        public string PivotFavorito_nombre { get; set; }
        [NotNull]
        public string PivotFavorito_Equipo { get; set; }
        [NotNull]
        public string AlaPivotFavorito_id { get; set; }
        [NotNull]
        public string AlaPivotFavorito_nombre { get; set; }
        [NotNull]
        public string AlaPivotFavorito_Equipo { get; set; }
    }
}
