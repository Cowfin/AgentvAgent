using UnityEngine;



    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
         private NetworkManagerLobby agentLobbyBuild = null;

        [Header("UI")]
        [SerializeField]
        private GameObject LobbyMenu = null;

        public void HostLobby()
        {
            agentLobbyBuild.StartHost();

            LobbyMenu.SetActive(false);
        }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}



