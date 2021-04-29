using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AgentLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField]
    private GameObject agentMenu = null;
    
 
    [HideInInspector]
    public NetworkIdentity connectIdentity
    {
        get
        {
            return netIdentity ?? (netIdentity = GetComponent<NetworkIdentity>());
        }
    }
    private NetworkIdentity netIdentity;
    [SerializeField]

    private Button confirmLobbyButton = null;

    public bool changeButtonTextWhileConnecting = true;

    public string prefixText = "Join";

    public bool disableButtonWhileClientIsActive = true;

    [SerializeField]
    private TMP_InputField userIpInput = null;
    private object AgentCurrentListing;
    private object AgentCurrentRoom;

    public object room
    {
        get;
        private set;
    }

    private void gameNetworkConfirmed()
    {
        confirmLobbyButton.interactable = true;

        gameObject.SetActive(false);
        agentMenu.SetActive(false);
    }
  
    private void gameNetworkFailed()
    {
        confirmLobbyButton.interactable = true;
    }
    private void OnEnable()
    {
        NetworkManagerLobby.confirmPlayerConnection += gameNetworkConfirmed;
        NetworkManagerLobby.confirmPlayerLeaves += gameNetworkFailed;
    }

    public static bool agentUserCodeName(string user)
    {

        if (user.Length < agentSmallName() || user.Length > agentLargeName())
        {
            return false;
        }

        if (user.Contains("<"))
            return false;
        if (user.Contains(">"))
            return false;


        return true;
    }

    private static int agentSmallName()
    {
        return 2;
    }

    private void LobbyJoined()
    {
        string values = null;
        string key = null;
        PlayerPrefs.SetString(key, values);
    }


    public struct CustomPacket
    {
        public string packet;
        public object nameInput;

        public CustomPacket(string packet, object name)
        {
            this.packet = packet;
            nameInput = name;
        }
    }



    private static int agentLargeName()
    {
        return 52;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.confirmPlayerConnection -= gameNetworkConfirmed;
        NetworkManagerLobby.confirmPlayerLeaves -= gameNetworkFailed;
    }
    public struct SendMessageInfo
    {
        public string callByName;
        public object[] messageLength;
        public bool validText;

        public SendMessageInfo(string textName, object[] text)
        {
            callByName = textName;
            messageLength = text.Length > 0 ? text : null;
            validText = text.Length > 0;
        }
    }
    public void JoinLobby()
    {
        string userHostIp = userIpInput.text;

        networkManager.networkAddress = userHostIp;
        networkManager.StartClient();

        confirmLobbyButton.interactable = false;
    }
}




