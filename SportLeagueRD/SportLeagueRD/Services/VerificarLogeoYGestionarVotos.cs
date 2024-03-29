﻿using SportLeagueRD.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using SportLeagueRD.View.Popups;
using SportLeagueRD.Messages;

namespace SportLeagueRD.Services {
    class VerificarLogeoYGestionarVotos {
        #region VARIABLES
        private string id_jugador = "";
        private string nombre = "";
        private string idEquipo = "";

        private Entity_usuario usuario;

        private readonly string Comprobante1 = "VT01";
        private readonly string Comprobante2 = "VT02";
        private readonly string Comprobante3 = "VT03";

        //  PARA QUE NO REALICE MAS DE UNA PETICION AL SERVER.
        private bool continuar = true;
        #endregion;

        #region METODOS
        //  VERIFICA QUE EL USUARIO ESTE LOGEADO
        public async Task<bool> VerificarLogeo() {
            usuario = App.usuario ;
            //  SI EL CORREO ES "" ES PORQUE AUN NO SE A LOGEADO
            if (string.IsNullOrEmpty(usuario.Correo)) {
                await Application.Current.MainPage.DisplayAlert("NO PUEDES REALIZAR ESTA ACCION :(", "Debes registrarte o iniciar seccion", "OK");
                return false;
            }
            return true;
        }

        //  HACE LA VOTACION POR UN EQUIPO
        public async Task<bool> VotarPorEquipo(string id, string nombre) {
            if (!await VerificarLogeo())
                return false;
            //  SI EL USUARIO NO TIENE NINGUN EQUIPO VOTADO COMO FAVORITA, SE ASIGNA EL NUEVO EQUIPO COMO FAVORITO,
            //  EN CASO DE QUE SI TENA UIN EQUIPO SELECCIONADO SE LE PREGUNTA SI ESTA SEGURO QUE DESEA CAMBIARLO.
            if (!string.IsNullOrEmpty(usuario.EquipoFavorito_id)) {
                //VERIFICO QUE EL EQUIPO SELECCIONADO NO ESTE YA GUARDADO
                if (id.Equals(usuario.EquipoFavorito_id)){
                    await Application.Current.MainPage.DisplayAlert("Este Equipo Ya Es Tu Favorito", "Ya has seleccionado a este Equipo como favorito, Selecciona otro.", "OK");
                    return false;
                }
                if ((await Application.Current.MainPage.DisplayActionSheet($"Cambiar {usuario.EquipoFavorito_nombre.ToUpper()}", "NO!", "SI!")).Equals("NO!"))
                    return false;
                }
            //  ACUALIZAR LOS DATOS EN LA BASE DE DATOS LOCAL
            usuario.EquipoFavorito_id = id;
            usuario.EquipoFavorito_nombre = nombre;
            await App.DB.UpdateItemAsync(usuario);
            //  ACTUALIZAR LOS DATOS EN EL SERVIDOR
            App.ServerC.SendMessageAsync($"{Comprobante1}-{usuario.Correo}-{id}");
            return true;
        }

        //  HACE LA VOTACION POR UN EQUIPO EN UN PARTIDO
        public async Task<bool> VotarPorEquipoEnPartido(string idPartido, string idEquipo) {
            if (!await VerificarLogeo())
                return false;
            if ((await Application.Current.MainPage.DisplayActionSheet($"Seguro? No podra cambiar su voto luego de realizarlo. ", "NO!", "SI!")).Equals("NO!"))
                return false;
            //  ACTUALIZAR LOS DATOS EN EL SERVIDOR
            App.ServerC.SendMessageAsync($"{Comprobante3}-{usuario.Correo}-{idPartido}-{idEquipo}");
            return true;
        }

        // SI SE VA A VOTAR POR UN JUGADOR 
        public async void VotarPorJugador(string id, string nombre, string idEquipo, string posicion) {
            if (!await VerificarLogeo())
                return;
            this.id_jugador = id;
            this.nombre = nombre;
            this.idEquipo = idEquipo;
            continuar = true;
            StarMessaginCenter(1);
            await PopupNavigation.Instance.PushAsync(new view_popup_listadoRankingJugadores(posicion));
        }

        //PROCESO DE VOTACION PARA UN JUGADOR
        private async void GestionarVoto(string posicion) {
                //  ALMACENA EL ID DEL EQUIPO DEL JUGADOR FAVORITO  ANTERIOR EN LA POSICION X AL QUE PERTENECIA ESTE CUANDO SE HIZO LA ANTERIOR VOTACION
                //  PARA PODER DISMINUIR LA VOTACION DE ESE JUGADOR EN ESE EQUIPO ESPESIFIO Y NO EN EL EQUIPO QUE PUEDA LLEGAR A PERTENECER EN UN FUTURO.
                string idEquipoJugador = "";
                    if (continuar) {
                        continuar = false;

                        switch (posicion) {
                            case "0":
                                //  SI EL USUARIO NO TIENE NINGUN JUGADOR VOTADO COMO FAVORITA, SE ASIGNA EL NUEVO EQUIPO COMO FAVORITO,
                                //  EN CASO DE QUE SI TENA UN JUGADOR FAVORITO SELECCIONADO SE LE PREGUNTA SI ESTA SEGURO QUE DESEA CAMBIARLO.
                                if (!string.IsNullOrEmpty(usuario.JugadorFavorito_id)) {
                                    //VERIFICO QUE EL JUGADOR SELECCIONADO NO ESTE YA GUARDADO EN ESTA MISMA VOTASION SELCCIONADA
                                    if (id_jugador.Equals(usuario.JugadorFavorito_id) && !string.IsNullOrEmpty(usuario.JugadorFavorito_EquipoId)) {
                                        await Application.Current.MainPage.DisplayAlert("Este Jugador Ya Es Tu Favorito", "Ya has seleccionado a este jugador como favorito en esta misma posicion, selecciona otro.", "OK");
                                        return;
                                    }
                                    if ((await Application.Current.MainPage.DisplayActionSheet($"Cambiar {usuario.JugadorFavorito_nombre.ToUpper()}", "NO!", "SI!")).Equals("NO!"))
                                        return;
                                }
                                //  ACUALIZAR LOS DATOS EN LA BASE DE DATOS LOCAL
                                usuario.JugadorFavorito_id = id_jugador;
                                usuario.JugadorFavorito_nombre = nombre;
                                idEquipoJugador = usuario.JugadorFavorito_EquipoId;
                                usuario.JugadorFavorito_EquipoId = idEquipo;
                                break;
                            case "1":
                                //  SI EL USUARIO NO TIENE NINGUN JUGADOR VOTADO COMO BASE, SE ASIGNA EL NUEVO EQUIPO COMO FAVORITO,
                                //  EN CASO DE QUE SI TENA UN JUGADOR FAVORITO BASE SELECCIONADO SE LE PREGUNTA SI ESTA SEGURO QUE DESEA CAMBIARLO.
                                if (!string.IsNullOrEmpty(usuario.BaseFavorito_id) && !string.IsNullOrEmpty(usuario.BaseFavorito_EquipoId)) {
                                    //VERIFICO QUE EL JUGADOR SELECCIONADO NO ESTE YA GUARDADO EN ESTA MISMA VOTASION SELCCIONADA
                                    if (id_jugador.Equals(usuario.BaseFavorito_id)) {
                                        await Application.Current.MainPage.DisplayAlert("Este Jugador Ya Es Tu Base Favorito", "Ya has seleccionado a este jugador como favorito en esta misma posicion, selecciona otro.", "OK");
                                        return;
                                    }
                                    if ((await Application.Current.MainPage.DisplayActionSheet($"Cambiar {usuario.BaseFavorito_nombre.ToUpper()}", "NO!", "SI!")).Equals("NO!"))
                                        return;
                                }
                                //  ACUALIZAR LOS DATOS EN LA BASE DE DATOS LOCAL
                                usuario.BaseFavorito_id = id_jugador;
                                usuario.BaseFavorito_nombre = nombre;
                                idEquipoJugador = usuario.BaseFavorito_EquipoId;
                                usuario.BaseFavorito_EquipoId = idEquipo;
                                break;
                            case "2":
                                //  SI EL USUARIO NO TIENE NINGUN JUGADOR VOTADO COMO BASE, SE ASIGNA EL NUEVO EQUIPO COMO FAVORITO,
                                //  EN CASO DE QUE SI TENA UN JUGADOR FAVORITO BASE SELECCIONADO SE LE PREGUNTA SI ESTA SEGURO QUE DESEA CAMBIARLO.
                                if (!string.IsNullOrEmpty(usuario.AleroFavorito_id) && !string.IsNullOrEmpty(usuario.AleroFavorito_EquipoId)) {
                                    //VERIFICO QUE EL JUGADOR SELECCIONADO NO ESTE YA GUARDADO EN ESTA MISMA VOTASION SELCCIONADA
                                    if (id_jugador.Equals(usuario.AleroFavorito_id)) {
                                        await Application.Current.MainPage.DisplayAlert("Este Jugador Ya Es Tu Alero Favorito", "Ya has seleccionado a este jugador como favorito en esta misma posicion, selecciona otro.", "OK");
                                        return;
                                    }
                                    if ((await Application.Current.MainPage.DisplayActionSheet($"Cambiar {usuario.AleroFavorito_nombre.ToUpper()}", "NO!", "SI!")).Equals("NO!"))
                                        return;
                                }
                                //  ACUALIZAR LOS DATOS EN LA BASE DE DATOS LOCAL
                                usuario.AleroFavorito_id = id_jugador;
                                usuario.AleroFavorito_nombre = nombre;
                                idEquipoJugador = usuario.AleroFavorito_EquipoId;
                                usuario.AleroFavorito_EquipoId = idEquipo;
                                break;
                            case "3":
                                //  SI EL USUARIO NO TIENE NINGUN JUGADOR VOTADO COMO BASE, SE ASIGNA EL NUEVO EQUIPO COMO FAVORITO,
                                //  EN CASO DE QUE SI TENA UN JUGADOR FAVORITO BASE SELECCIONADO SE LE PREGUNTA SI ESTA SEGURO QUE DESEA CAMBIARLO.
                                if (!string.IsNullOrEmpty(usuario.PivotFavorito_id) && !string.IsNullOrEmpty(usuario.PivotFavorito_EquipoId)) {
                                    //VERIFICO QUE EL JUGADOR SELECCIONADO NO ESTE YA GUARDADO EN ESTA MISMA VOTASION SELCCIONADA
                                    if (id_jugador.Equals(usuario.PivotFavorito_id)) {
                                        await Application.Current.MainPage.DisplayAlert("Este Jugador Ya Es Tu Pivot Favorito", "Ya has seleccionado a este jugador como favorito en esta misma posicion, selecciona otro.", "OK");
                                        return;
                                    }
                                    if ((await Application.Current.MainPage.DisplayActionSheet($"Cambiar {usuario.PivotFavorito_nombre.ToUpper()}", "NO!", "SI!")).Equals("NO!"))
                                        return;
                                }
                                //  ACUALIZAR LOS DATOS EN LA BASE DE DATOS LOCAL
                                usuario.PivotFavorito_id = id_jugador;
                                usuario.PivotFavorito_nombre = nombre;
                                idEquipoJugador = usuario.PivotFavorito_EquipoId;
                                usuario.PivotFavorito_EquipoId = idEquipo;
                                break;
                            case "4":
                                //  SI EL USUARIO NO TIENE NINGUN JUGADOR VOTADO COMO BASE, SE ASIGNA EL NUEVO EQUIPO COMO FAVORITO,
                                //  EN CASO DE QUE SI TENA UN JUGADOR FAVORITO BASE SELECCIONADO SE LE PREGUNTA SI ESTA SEGURO QUE DESEA CAMBIARLO.
                                if (!string.IsNullOrEmpty(usuario.AlaPivotFavorito_id) && !string.IsNullOrEmpty(usuario.AlaPivotFavorito_EquipoId)) {
                                    //VERIFICO QUE EL JUGADOR SELECCIONADO NO ESTE YA GUARDADO EN ESTA MISMA VOTASION SELCCIONADA
                                    if (id_jugador.Equals(usuario.AlaPivotFavorito_id)) {
                                        await Application.Current.MainPage.DisplayAlert("Este Jugador Ya Es Tu Ala-Pivot Favorito", "Ya has seleccionado a este jugador como favorito en esta misma posicion, selecciona otro.", "OK");
                                        return;
                                    }
                                    if ((await Application.Current.MainPage.DisplayActionSheet($"Cambiar {usuario.AlaPivotFavorito_nombre.ToUpper()}", "NO!", "SI!")).Equals("NO!"))
                                        return;
                                }
                                //  ACUALIZAR LOS DATOS EN LA BASE DE DATOS LOCAL
                                usuario.AlaPivotFavorito_id = id_jugador;
                                usuario.AlaPivotFavorito_nombre = nombre;
                                idEquipoJugador = usuario.AlaPivotFavorito_EquipoId;
                                usuario.AlaPivotFavorito_EquipoId = idEquipo;
                                break;
                        }
                        await App.DB.UpdateItemAsync(usuario);
                        //  ACTUALIZAR LOS DATOS EN EL SERVIDOR
                        App.ServerC.SendMessageAsync($"{Comprobante2}-{usuario.Correo}-{idEquipo}-{id_jugador}-{posicion}-{idEquipoJugador}");
                        DependencyService.Get<IToast>().Show("Voto Realizado Con Exito!!");
                        StopMessaginCenter();
            }
        }

        //INICIA EL MESAGING CENTER.
        private void StarMessaginCenter(int cargar) => MessagingCenter.Subscribe<Message>(this, "CargarVotoJugador", Llamar => { GestionarVoto((string)Llamar.Variable[0]); });

        //DESUSCRIBIR EL MESSANGINGCENTER PARRA LIBERAR MEMORIA
        private void StopMessaginCenter() => MessagingCenter.Unsubscribe<Message>(this, "CargarVotoJugador");
        #endregion
    }
}