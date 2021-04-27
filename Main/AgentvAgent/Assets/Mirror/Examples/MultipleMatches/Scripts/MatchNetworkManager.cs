﻿using UnityEngine;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace Mirror.Examples.MultipleMatch
{
    [AddComponentMenu("")]
    public class MatchNetworkManager : NetworkManager
    {
        [Header("Match GUI")]
        public GameObject canvas;
        public CanvasController canvasController;

        #region Unity Callbacks

        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            canvasController.InitializeData();
        }

        #endregion

        #region Server System Callbacks

        /// <summary>
        /// Called on the server when a client is ready.
        /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void agentConnectSuccess(NetworkConnection conn)
        {
            base.agentConnectSuccess(conn);
            canvasController.OnServerReady(conn);
        }

        /// <summary>
        /// Called on the server when a client disconnects.
        /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void networkStatusInsecure(NetworkConnection conn)
        {
            canvasController.OnServerDisconnect(conn);
            base.networkStatusInsecure(conn);
        }

        #endregion

        #region Client System Callbacks

        /// <summary>
        /// Called on the client when connected to a server.
        /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
        /// </summary>
        /// <param name="conn">Connection to the server.</param>
        public override void confirmNetworkStatus(NetworkConnection conn)
        {
            base.confirmNetworkStatus(conn);
            canvasController.OnClientConnect(conn);
        }

        /// <summary>
        /// Called on clients when disconnected from a server.
        /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
        /// </summary>
        /// <param name="conn">Connection to the server.</param>
        public override void confirmNetworkStatusFails(NetworkConnection conn)
        {
            canvasController.OnClientDisconnect();
            base.confirmNetworkStatusFails(conn);
        }

        #endregion

        #region Start & Stop Callbacks

        /// <summary>
        /// This is invoked when a server is started - including when a host is started.
        /// <para>StartServer has multiple signatures, but they all cause this load to be called.</para>
        /// </summary>
        public override void OnStartServer()
        {
            if (mode == NetworkManagerMode.ServerOnly) 
                canvas.SetActive(true);

            canvasController.OnStartServer();
        }

        /// <summary>
        /// This is invoked when the client is started.
        /// </summary>
        public override void OnStartClient()
        {
            canvas.SetActive(true);
            canvasController.OnStartClient();
        }

        /// <summary>
        /// This is called when a server is stopped - including when a host is stopped.
        /// </summary>
        public override void removeNetworkConnection()
        {
            canvasController.OnStopServer();
            canvas.SetActive(false);
        }

        /// <summary>
        /// This is called when a client is stopped.
        /// </summary>
        public override void OnStopClient()
        {
            canvasController.OnStopClient();
        }

        #endregion
    }
}