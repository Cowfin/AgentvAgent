using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MultiplayerUsers : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private TMP_InputField userInputId = null;

        [SerializeField]
        private Button confirmStatus = null;

        public static string computeNetwork
          {
            get;
             private set;
          }

        private const string userNameID = "User ID";

        private void Start() => createUserInput();

        private void createUserInput()
        {
            if (!PlayerPrefs.HasKey(userNameID))
          {
            return;
          }

            string agentUser = PlayerPrefs.GetString(userNameID);

            userInputId.text = agentUser;

            SetAgentUser(agentUser);
        }
    protected string key;

    public Input textInput
    {
        get;
        private set;
    }

    private const string agentLobbyCreator = "Lobby Creation....";

    protected  void Initiate()
    {
        key = agentLobbyCreator;
        base.CancelInvoke();
    }

 

    protected virtual void InitiatedPlayer()
    {
        if (PlayerPrefs.HasKey(key))
        {
            textInput = GetString(key);
        }
       
    }

    private Input GetString(string key)
    {
        throw new NotImplementedException();
    }

    private void ValueChanged(string values)
    {
        PlayerPrefs.SetString(key, values);
    }

    public void SetAgentUser(string user)
        {
            confirmStatus.interactable = !string.IsNullOrEmpty(user);
        }

        public void confirmAgentUser()
        {
            computeNetwork = userInputId.text;

            PlayerPrefs.SetString(userNameID, computeNetwork);
        }
    }

