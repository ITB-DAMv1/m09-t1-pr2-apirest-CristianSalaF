using Microsoft.AspNetCore.SignalR;

namespace T1PR2_APIREST.Hubs
{
    public class XatHub : Hub
    {
        // Mètode invocable des del client
        public async Task EnviaMissatge(string usuari, string missatge)
        {
            // Envia el missatge a tots els clients connectats
            await Clients.All.SendAsync("RepMissatge", usuari, missatge);
        }

        //Event per control de qui es connecta
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Nou client: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        //Event per control de qui es desconnecta
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Console.WriteLine($"Client desconnectat: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(ex);
        }

    }
}
