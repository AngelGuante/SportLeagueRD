using SportLeagueRD.Model;
using SQLite;
using System.Threading.Tasks;

namespace SportLeagueRD.Services {
    public class Database {
        private readonly SQLiteAsyncConnection database;

        #region CONSTRUCTOR
        public Database(string dbPath) {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Entity_usuario>();

            AgregarDatosUsuario();
        }
        #endregion

        #region METODOS
        //CREAR LOS DATOS POR DEFECTO DEL USUARIO CON EL ID 1
        private async void AgregarDatosUsuario() {
            if (await GetItemAsync() == null)
               await database.InsertAsync(new Entity_usuario {
                   ID = 1,
                   Nombre = "No Logeado",
                   Correo = "",
                   EquipoFavorito_id = "",
                   EquipoFavorito_nombre = "",
                   JugadorFavorito_id = "",
                   JugadorFavorito_nombre = "",
                   JugadorFavorito_Equipo = "",
                   BaseFavorito_id = "",
                   BaseFavorito_nombre = "",
                   BaseFavorito_Equipo = "",
                   AleroFavorito_id = "",
                   AleroFavorito_nombre = "",
                   AleroFavorito_Equipo = "",
                   PivotFavorito_id = "",
                   PivotFavorito_nombre = "",
                   PivotFavorito_Equipo = "",
                   AlaPivotFavorito_id = "",
                   AlaPivotFavorito_nombre = "",
                   AlaPivotFavorito_Equipo = ""
               });
        }

        //  GUARDA LOS DATOS DEL USUARIO
        public Task<int> UpdateItemAsync(Entity_usuario item) => database.UpdateAsync(item);

        // TRAE LOS DATOS DEL USUARIO REGISTRADO
        public Task<Entity_usuario> GetItemAsync() => database.Table<Entity_usuario>().Where(i => i.ID == 1).FirstOrDefaultAsync();
        #endregion
    }
}

