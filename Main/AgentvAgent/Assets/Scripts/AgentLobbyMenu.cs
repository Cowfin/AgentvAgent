using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField]
    private GameObject agentMenu = null;

    [SerializeField]

    private Button confirmLobbyButton = null;

    public bool changeButtonTextWhileConnecting = true;

    public string prefixText = "Join";

    public bool disableButtonWhileClientIsActive = true;

    [SerializeField]
    private TMP_InputField userIpInput = null;
    private object lobby_list;

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
        throw new NotImplementedException();
    }

    private void LobbyJoined()
    {
        string values = null;
        string key = null;
        PlayerPrefs.SetString(key, values);
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

    public void JoinLobby()
    {
        string userHostIp = userIpInput.text;

        networkManager.networkAddress = userHostIp;
        networkManager.StartClient();

        confirmLobbyButton.interactable = false;
    }
}




