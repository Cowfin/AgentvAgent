using UnityEngine;

namespace Mirror.Cloud.Example
{
    public sealed class NetworkManagerListServerPong : NetworkManagerListServer
    {
        public override void lobbyNetworkCount(NetworkConnection conn)
        {
            Debug.Assert(startPositions.Count == 2, "Pong Scene should have 2 start Positions");
            // add player at correct spawn position
            Transform startPos = userLobbyCount == 0 ? startPositions[0] : startPositions[1];

            GameObject player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);

            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}
