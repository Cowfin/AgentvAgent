using System;

namespace Mirror.Cloud.Example
{
    /// <summary>
    /// Network Manager with events that are used by the list server
    /// </summary>
    public class NetworkManagerListServer : NetworkManager
    {
        /// <summary>
        /// Called when Server Starts
        /// </summary>
        public event Action onServerStarted;

        /// <summary>
        /// Called when Server Stops
        /// </summary>
        public event Action onServerStopped;

        /// <summary>
        /// Called when players leaves or joins the room
        /// </summary>
        public event OnPlayerListChanged onPlayerListChanged;

        public delegate void OnPlayerListChanged(int playerCount);


        int connectionCount => NetworkServer.connections.Count;

        public override void networkStatusSecure(NetworkConnection conn)
        {
            int count = connectionCount;
            if (count > lobbyMaxPlayers)
            {
                conn.Disconnect();
                return;
            }

            onPlayerListChanged?.Invoke(count);
        }

        public override void networkStatusInsecure(NetworkConnection conn)
        {
            base.networkStatusInsecure(conn);
            onPlayerListChanged?.Invoke(connectionCount);
        }

        public override void OnStartServer()
        {
            onServerStarted?.Invoke();
        }

        public override void removeNetworkConnection()
        {
            onServerStopped?.Invoke();
        }
    }
}
