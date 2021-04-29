using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

    public class NetworkRoomPlayer : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private GameObject agentMultiplayer = null;
        public Button changeButton;
        public Button playerLeavesConfirm;
        [SerializeField]
        private TMP_Text[] userIDLobby = new TMP_Text[4];
        public InputField userInitialInput;
        public Button playerConfirmsAction;
        [SerializeField]
        private TMP_Text[] userReadyConfirm = new TMP_Text[4];
        [SerializeField]
        private Button lobbyInitiateGame = null;
        public Button waitForUser;

    [SyncVar(hook = "Player Name")]
    public string playerName = "";
    [SyncVar(hook = "Player Color")]
    public Color playerColor = Color.white;
    [SyncVar(load = nameof(agentLobbyUserIDStatus))]
    public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
    public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);


    public string computeNetwork = "Loading...";

        [SyncVar(load = nameof(agentLobbyPlayStatus))]

        public bool userConfirmPlay = false;

        private bool lobbyHost;
        public bool lobbyHostUser
        {
            set
            {
                lobbyHost = value;
                lobbyInitiateGame.gameObject.SetActive(value);
            }
        }

        private NetworkManagerLobby agentLobby;
        private NetworkManagerLobby agentLobbyInPlay
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

        public override void OnStartAuthority()
        {
            SetUserNameLobby(MultiplayerUsers.computeNetwork);

            agentMultiplayer.SetActive(true);
        }

        public override void agentNetworkInitiated()
        {
            agentLobbyInPlay.UsersInLobby.Add(this);

            verifyGamePlay();
        }

        public override void agentNetworkHalt()
        {
            agentLobbyInPlay.UsersInLobby.Remove(this);

            verifyGamePlay();
        }
    public void OnMyName(string newName)
    {
        playerName = newName;
        userInitialInput.text = playerName;
    }

    public void OnMyColor(Color newColor)
    {
        playerColor = newColor;
        changeButton.GetComponent<Image>().color = newColor;
    }

    public void agentLobbyPlayStatus(bool wasConfirmed, bool isConfirmed)
    {
        verifyGamePlay();
    }
    public void agentLobbyUserIDStatus(string userIDSet, string userIDConfirmed)
    {
        verifyGamePlay();
    }

        private void verifyGamePlay()
        {
            if (!successInConnection)
            {
                foreach (var agentUser in agentLobbyInPlay.UsersInLobby)
                {
                    if (agentUser.successInConnection)
                    {
                        agentUser.verifyGamePlay();
                        break;
                    }
                }

                return;
            }
       
        
        for (int userName = 0; userName < userIDLobby.Length; userName++)
            {
                userIDLobby[userName].text = "Waiting For Player...";
                userReadyConfirm[userName].text = string.Empty;
            }

            for (int usersInLobby = 0; usersInLobby < agentLobbyInPlay.UsersInLobby.Count; usersInLobby++)
            {
                userIDLobby[usersInLobby].text = agentLobbyInPlay.UsersInLobby[usersInLobby].computeNetwork;
                userReadyConfirm[usersInLobby].text = agentLobbyInPlay.UsersInLobby[usersInLobby].userConfirmPlay ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not Ready</color>";
            }
        }

        public void agentLobbyPlayGame(bool initiateAgentGame)
        {
            if (!lobbyHost)
        {
            return;
        }

            lobbyInitiateGame.interactable = initiateAgentGame;
        }

    void colorChangeButton(Color nameColor)
    {
        ColorBlock styleOfColor = playerConfirmsAction.colors;
        styleOfColor.normalColor = nameColor;
        styleOfColor.pressedColor = nameColor;
        styleOfColor.highlightedColor = nameColor;
        styleOfColor.disabledColor = nameColor;
        playerConfirmsAction.colors = styleOfColor;
    }
    [Command]
        private void SetUserNameLobby(string userAgentName)
        {
            computeNetwork = userAgentName;
        }

        [Command]
        public void userConfirmPlayStatus()
        {
            userConfirmPlay = !userConfirmPlay;

            agentLobbyInPlay.confirmUserStatus();
        }

        [Command]
        public void userConfirmPlayGame()
        {
            if (agentLobbyInPlay.UsersInLobby[0].connectionToClient != connectionToClient)
        {
            return;
        }

            agentLobbyInPlay.userInitiatesGame();
        }
    }
