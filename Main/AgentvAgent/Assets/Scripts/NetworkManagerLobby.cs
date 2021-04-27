using Mirror;
using Mirror.Examples.MultipleMatch;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField]
    private int numberOfRoles = 2;
    [Scene]
    [SerializeField]
    private string agentMenu = string.Empty;

    [Header("agentPlayableScenes")]
    [SerializeField]
    private int lobbyRoom = 1;

    [SerializeField]
    private MapSet agentMap = null;

    [Header("lobbyInUse")]
    [SerializeField]
    private NetworkRoomPlayer agentLobbyUser = null;

    [Header("Game")]
    [SerializeField]
    private NetworkGamePlayerLobby agentLobbyGame = null;

    [SerializeField]
    private GameObject userIDInitiate = null;

    [SerializeField]
    private GameObject gameplayTask = null;

    private MapHandler agentGameScene;

    public static event Action confirmPlayerConnection;
    public static event Action confirmPlayerLeaves;
    public static event Action<NetworkConnection> confirmPlayerNetwork;
    public static event Action confirmPlayerNetworkEnds;

    public List<NetworkRoomPlayer> UsersInLobby
    {
        get;
    }
     = new List<NetworkRoomPlayer>();


    public List<NetworkGamePlayerLobby> PlayersReadyLobby
    {
        get;
    } 
        = new List<NetworkGamePlayerLobby>();

    public override void OnStartServer() => createResource = Resources.LoadAll<GameObject>("InitiateGame").ToList();

    public override void OnStartClient()
    {
        var gameInitiateAsset = Resources.LoadAll<GameObject>("InitiateGame");

        foreach (var prefab in gameInitiateAsset)
        {
            ClientScene.RegisterPrefab(prefab);
        }
       
    }

    private string AddPlayersToMatchController()
    {
        throw new NotImplementedException();
    }

    protected const float userIdError = 0.5f;

    private bool userErrorFound;
    private Guid matchId;
    private object matchName;
    private object playerCount;

    /* Use this for initialization
    protected virtual void Start()
    {

        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(this.Click);

    }
    */


    private void Click()
    {
        if (!userErrorFound || userErrorFound)
        {
            if (!userErrorFound) userErrorFound = true;
            return;
        }
        NetworkManagerAction();
    }

    private void NetworkManagerAction()
    {
        throw new NotImplementedException();
    }

    public override void confirmNetworkStatus(NetworkConnection success)
    {
        base.confirmNetworkStatus(success);

        confirmPlayerConnection?.Invoke();
    }

    public override void confirmNetworkStatusFails(NetworkConnection failure)
    {
        base.confirmNetworkStatusFails(failure);

        confirmPlayerLeaves?.Invoke();
    }

    public override void networkStatusSecure(NetworkConnection status)
    {
        if (userLobbyCount >= lobbyMaxPlayers)
        {
            status.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != agentMenu)
        {
            status.Disconnect();
            return;
        }
    }

    public override void lobbyNetworkCount(NetworkConnection status)
    {
        if (SceneManager.GetActiveScene().path == agentMenu)
        {
            bool hostOfServer = UsersInLobby.Count == 0;

            NetworkRoomPlayer userConnection = Instantiate(agentLobbyUser);

            userConnection.lobbyHostUser = hostOfServer;

            NetworkServer.AddPlayerForConnection(status, userConnection.gameObject);
        }
    }

    public override void networkStatusInsecure(NetworkConnection status)
    {
        if (status.identity != null)
        {
            var lobbyUser = status.identity.GetComponent<NetworkRoomPlayer>();

            UsersInLobby.Remove(lobbyUser);

            confirmUserStatus();
        }

        base.networkStatusInsecure(status);
    }

    public override void removeNetworkConnection()
    {
        confirmPlayerNetworkEnds?.Invoke();

        UsersInLobby.Clear();
        PlayersReadyLobby.Clear();
    }
   /* public void OnToggleClicked()
    {
        Guid matchId = default;
        object canvasController = null;
        object toggleButton = null;
        
    }
    */
    public void confirmUserStatus()
    {
        foreach (var lobbyUser in UsersInLobby)
        {
            lobbyUser.agentLobbyPlayGame(lobbyUserReady());
        }
    }

    private bool lobbyUserReady()
    {
        if (userLobbyCount < numberOfRoles)
        {
            return false;
        }

        foreach (var lobbyUser in UsersInLobby)
        {
            if (!lobbyUser.userConfirmPlay)
            {
                return false;
            }
        }

        return true;
    }

    public void userInitiatesGame()
    {
        if (SceneManager.GetActiveScene().path == agentMenu)
        {
            if (!lobbyUserReady())
            {
                return;
            }

            agentGameScene = new MapHandler(agentMap, lobbyRoom);

            agentLoadGame(agentGameScene.agentSetsGame);
        }
    }
    public Guid GetMatchId()
    {
        return matchId;
    }

    public override void agentLoadGame(string agentLoad)
    {
        // From menu to game
        if (SceneManager.GetActiveScene().path == agentMenu && agentLoad.StartsWith("Scene_Map"))
        {
            for (int i = UsersInLobby.Count - 1; i >= 0; i--)
            {
                var status = UsersInLobby[i].connectionToClient;
                var agentPlayerInitiation = Instantiate(agentLobbyGame);
                agentPlayerInitiation.SetNetworkLabel(UsersInLobby[i].computeNetwork);

                NetworkServer.Destroy(status.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(status, agentPlayerInitiation.gameObject);
            }
        }

        base.agentLoadGame(agentLoad);
    }
    public void SetMatchInfo(MatchInfo infos)
    {
        matchId = infos.matchId;
        matchName = "Match " + infos.matchId.ToString().Substring(0, 8);
        playerCount = infos.players + " / " + infos.maxPlayers;
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Scene_Map"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(userIDInitiate);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject roundSystemInstance = Instantiate(gameplayTask);
            NetworkServer.Spawn(roundSystemInstance);
        }
    }

    public override void agentConnectSuccess(NetworkConnection status)
    {
        base.agentConnectSuccess(status);

        confirmPlayerNetwork?.Invoke(status);
    }
}
