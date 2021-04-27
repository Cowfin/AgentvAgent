using UnityEngine;



    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
         private NetworkManagerLobby agentNetworkLobby = null;

        [Header("UI")]
        [SerializeField]
        private GameObject  agentRoomMenu = null;

        public void HostLobby()
        {
            agentNetworkLobby.StartHost();

            agentRoomMenu.SetActive(false);
        }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}



