using Newtonsoft.Json;
using SportLeagueRD.Messages;
using SportLeagueRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SportLeagueRD.ViewModel{
    public class ServerConnection{
        #region VARIABLES
        private ClientWebSocket client = new ClientWebSocket();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        #endregion

        #region CONSTRUCTOR
        public ServerConnection() => ConnectToServerAsync();
        #endregion

        #region METODOS
        //CREA LA CONEXION CON SEL SERVIDOR
        private async void ConnectToServerAsync(){
            try {
                await client.ConnectAsync(new Uri($"WS://{App.ServerUrl}/websocketendpoint"), _cts.Token);

                await Task.Factory.StartNew(async () => {
                    while (true) {
                        WebSocketReceiveResult result;
                        ArraySegment<byte> message = new ArraySegment<byte>(new byte[4096]);

                        do {
                            result = await client.ReceiveAsync(message, _cts.Token);
                            byte[] messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                            string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

                            try {
                                //DESERIALIZO EL JSON RECIVIDO DESDE EL SERVER, EN UN STRING PARA PODER COMPROBAR A DONDE PERTENECE LO QUE SE ACABA DE RECIBIR.
                                var VerificarComprobante = JsonConvert.DeserializeObject(serialisedMessage);
                                //BUSCO EN EN JSON COMBERTIDO EN STRING, EL COMPROBANTE QUE MANDO DESDE EL SERVER QUE SE POSICIONA EN EL ULTIMO REGISTRO DEL JSON JUSTO EN LA POSICION:
                                //      JSON.lenght - 11, y tomo el numero total de caracteres que tiene este comprobante que son 4 caracteres.
                                switch (VerificarComprobante.ToString().Substring(VerificarComprobante.ToString().Length - 11, 4)) {
                                    //      **NOMECLATURA EN LA QUE TRABAJARA EL SERVIDOR Y LA APLICACION**
                                    //  -NO..   -> NOTICIAS
                                    //  -EV..   -> EVENTOS
                                    //  -EQ..   -> EQUIPOS
                                    //  -JG..   -> JUGADORES
                                    //  -MC..   -> MARCADOR
                                    //  -CM..   -> COMENTARIOS
                                    //  -EJ..   -> JUGADORES BUSCADOS POR EQUIPO
                                    //  -EM..   -> MARCADOR BUSCADO POR EQUIPO
                                    //  -JE..   -> EQUIPO BUSCADO POR JUGADOR
                                    //  -PM..   -> PUNTOS POR SECCION DE UN PARTIDO
                                    //  -LM..   -> LIDERES EN MARCADOR
                                    //  -EB..   -> EQUIPOS BUSCADOS POR SU NOMBRE
                                    //  -JB..   -> JUGADORES BUSCADOS POR SU NOMBRE
                                    //  -VT..   -> VOTACIONES
                                    //*NOTICIAS*//
                                    case "NO01":
                                        MessagingCenter.Send(new Message() { Noticias = JsonConvert.DeserializeObject<List<model_noticias>>(serialisedMessage) }, "cargarNoticias");
                                        break;
                                    //*NOTICIAS*//
                                    case "NO02":
                                        MessagingCenter.Send(new Message() { Noticias = JsonConvert.DeserializeObject<List<model_noticias>>(serialisedMessage) }, "cargarNoticia");
                                        break;

                                    //*EVENTOS*//
                                    case "EV01":
                                        MessagingCenter.Send(new Message() { Eventos = JsonConvert.DeserializeObject<List<model_eventos>>(serialisedMessage) }, "cargarEventos");
                                        break;
                                    case "EV02":
                                        MessagingCenter.Send(new Message() { Eventos = JsonConvert.DeserializeObject<List<model_eventos>>(serialisedMessage) }, "cargarEvento");
                                        break;

                                    //*EQUIPOS*//
                                    case "EQ01":
                                        MessagingCenter.Send(new Message() { Equipos = JsonConvert.DeserializeObject<List<model_equipos>>(serialisedMessage) }, "cargarEquipos");
                                        break;
                                    case "EQ02":
                                        MessagingCenter.Send(new Message() { Equipos = JsonConvert.DeserializeObject<List<model_equipos>>(serialisedMessage) }, "cargarEquiposRanking");
                                        break;
                                    case "EQ03":
                                        MessagingCenter.Send(new Message() { Equipos = JsonConvert.DeserializeObject<List<model_equipos>>(serialisedMessage) }, "cargarEquipo");
                                        break;
                                    case "EQ05":
                                        MessagingCenter.Send(new Message() { Equipos = JsonConvert.DeserializeObject<List<model_equipos>>(serialisedMessage) }, "cargarDetallesEquipoB");
                                        break;

                                    //*JUGADORES*//
                                    case "JG01":
                                        MessagingCenter.Send(new Message() { Jugadores = JsonConvert.DeserializeObject<List<model_jugadores>>(serialisedMessage) }, "cargarJugadores");
                                        break;
                                    case "JG02":
                                        MessagingCenter.Send(new Message() { Jugadores = JsonConvert.DeserializeObject<List<model_jugadores>>(serialisedMessage) }, "cargarJugadoresRanking");
                                        break;
                                    case "JG03":
                                        MessagingCenter.Send(new Message() { Jugadores = JsonConvert.DeserializeObject<List<model_jugadores>>(serialisedMessage) }, "cargarDetallesJugador");
                                        break;

                                    //*JUGADORES QUE VIENEN DE UN EQUIPO**//
                                    case "EJ01":
                                        MessagingCenter.Send(new Message() { Jugadores = JsonConvert.DeserializeObject<List<model_jugadores>>(serialisedMessage) }, "cargarJugadoresEquipo");
                                        break;
                                    case "EJ02":
                                        MessagingCenter.Send(new Message() { Jugadores = JsonConvert.DeserializeObject<List<model_jugadores>>(serialisedMessage) }, "cargarEstadisticas");
                                        break;
                                    //*MARCADOR QUE VIENEN DE UN EQUIPO**//
                                    case "EM01":
                                        MessagingCenter.Send(new Message() { Marcador = JsonConvert.DeserializeObject<List<model_marcador>>(serialisedMessage) }, "cargarMarcadorEquipo");
                                        break;
                                    //*EQUIPO QUE VIENEN DE UN JUGADOR**//
                                    case "JE01":
                                        MessagingCenter.Send(new Message() { Equipos = JsonConvert.DeserializeObject<List<model_equipos>>(serialisedMessage) }, "cargarEquipoPorJugador");
                                        break;

                                    case "LM01":
                                        MessagingCenter.Send(new Message() { Lideres = JsonConvert.DeserializeObject<List<model_lideres>>(serialisedMessage) }, "cargarLideres");
                                        break;
                                    case "PM01":
                                        MessagingCenter.Send(new Message() { PuntosBasquetball = JsonConvert.DeserializeObject<List<model_puntos_bascketball>>(serialisedMessage) }, "cargarPuntosBasquetball");
                                        break;

                                    //*MARCADOR*//
                                    case "MC01":
                                        MessagingCenter.Send(new Message() { Marcador = JsonConvert.DeserializeObject<List<model_marcador>>(serialisedMessage) }, "cargarMarcador");
                                        break;
                                    case "MC02":
                                        MessagingCenter.Send(new Message() { Marcador = JsonConvert.DeserializeObject<List<model_marcador>>(serialisedMessage) }, "cargarMarcadorDetallado");
                                        break;

                                    //*COMENTARIO*//
                                    case "CM02":
                                        MessagingCenter.Send(new Message() { Comentarios = JsonConvert.DeserializeObject<List<model_comentarios>>(serialisedMessage) }, "cargarComentariosUsuarios");
                                        break;
                                }
                            }
                            catch (Exception e) {
                                Console.WriteLine($"-MENSAGE: (#MCTSA_001) Invalide message format. {e.Message}" +
                                    $"\n {JsonConvert.DeserializeObject(serialisedMessage)}");
                            }
                        } while (!result.EndOfMessage);
                    }
                });
            } catch (Exception e) {
                Console.WriteLine($"-MENSAGE: (#MCTSA_002) ERROR: {e.Message}");
            }
        }

        //ENVIAR UNA PETICION AL SERVIDOR
        public async void SendMessageAsync(string message) {
            try {
                string serialisedMessage = JsonConvert.SerializeObject(message);
                var byteMessage = Encoding.UTF8.GetBytes(serialisedMessage);
                var segmnet = new ArraySegment<byte>(byteMessage, 0, byteMessage.Length);

                await client.SendAsync(segmnet, WebSocketMessageType.Text, true, _cts.Token);
            } catch (Exception e) {
                Console.WriteLine($"-MENSAGE: ERROR: {e.Message}");
            }
        }
        #endregion
    }
}