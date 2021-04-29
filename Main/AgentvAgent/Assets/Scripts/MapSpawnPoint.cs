using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Map Set", menuName = "Rounds/Map Set")]
public class MapSpawnPoint : ScriptableObject
{
    [Scene]
    [SerializeField]
    private List<string> agentGameScenes = new List<string>();

    public NetworkIdentity prizePrefab;

    public void mainSpawn()
    {
        for (int i = 0; i < 10; i++)
            playerInitialTrial();
    }
    public void playerInitialTrial()
    {
        Vector3 placementPlayer = new Vector3(Random.Range(-14, 20), 1, Random.Range(-14, 20));

        GameObject agentObject = Instantiate(prizePrefab.gameObject, placementPlayer, Quaternion.identity);
        Reward opponent = agentObject.gameObject.GetComponent<Reward>();


        NetworkServer.Spawn(agentObject);
    }

    public IReadOnlyCollection<string> agentPlayableScenes => agentGameScenes.AsReadOnly();
}
