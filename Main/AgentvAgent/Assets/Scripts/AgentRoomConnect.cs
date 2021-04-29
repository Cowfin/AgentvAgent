using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentRoomConnect : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField]
    private GameObject agentMenu = null;

    [SerializeField]
    private TMP_InputField userIpInput = null;

    [SerializeField]
    private Button confirmLobbyButton = null;

    private void OnEnable()
    {
        NetworkManagerLobby.confirmPlayerConnection += gameNetworkConfirmed;
        NetworkManagerLobby.confirmPlayerLeaves += gameNetworkFailed;
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
}




