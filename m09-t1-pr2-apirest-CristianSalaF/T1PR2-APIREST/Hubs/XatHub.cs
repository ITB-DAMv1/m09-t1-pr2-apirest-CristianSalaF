using Microsoft.AspNetCore.SignalR;

namespace T1PR2_APIREST.Hubs
{
    /// <summary>
    /// SignalR Hub for managing chat functionality.
    /// </summary>
    public class XatHub : Hub
    {
        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="usuari">The user sending the message.</param>
        /// <param name="missatge">The message content.</param>
        public async Task EnviaMissatge(string usuari, string missatge)
        {
            // Envia el missatge a tots els clients connectats
            await Clients.All.SendAsync("RepMissatge", usuari, missatge);
        }

        /// <summary>
        /// Event triggered when a client connects.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Nou client: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Event triggered when a client disconnects.
        /// </summary>
        /// <param name="ex">The exception that caused the disconnection, if any.</param>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Console.WriteLine($"Client desconnectat: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(ex);
        }
    }
}
