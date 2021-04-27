using Mirror;

    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        private string computeNetwork = "Loading...";

        private NetworkManagerLobby agentLobby;
        private NetworkManagerLobby lobbyInUse
        {
            get
            {
                if (agentLobby != null)
             {
                return agentLobby;
             }
                return agentLobby = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void agentNetworkInitiated()
        {
            DontDestroyOnLoad(gameObject);

            lobbyInUse.PlayersReadyLobby.Add(this);
        }

        public override void agentNetworkHalt()
        {
            lobbyInUse.PlayersReadyLobby.Remove(this);
        }

        [Server]
        public void SetNetworkLabel(string computeNetwork)
        {
            this.computeNetwork = computeNetwork;
        }
    }
