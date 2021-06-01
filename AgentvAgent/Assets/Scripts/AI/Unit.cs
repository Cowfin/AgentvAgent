using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    List<GatheringSpot> gatheringSpots;
    GatheringSpot spot;
    Spot target;
    bool drawPathGizmos;
    float speed = 2.5f;
    float turnSpeed = 5f;
    int weight = 4;
    Node[] path;
    int[] weights;
    int targetIndex;
    Node currentNode;
    bool changingPath;
    bool walking;
    float waitTime;

    void Start()
    {
        drawPathGizmos = PathRequestManager.DrawPathGizmos();
        waitTime = Random.Range(5f, 20f);
        PathRequestManager.AvailableSpot(new SpotRequest(null, OnSpotFound));

        PathRequestManager.RequestWorldPosToNode(new WorldPosToNode(transform.position, OnNodeFound));
        if (currentNode != null)
        {
            PathRequestManager.UpdateGrid(new GridUpdate(currentNode, weight));
        }
        if (target != null)
        {
            walking = true;
            PathRequestManager.UpdateGrid(new GridUpdate(currentNode, -weight));
            PathRequestManager.RequestPath(new PathRequest(transform.position, target.location, OnPathFound));
        }
    }

    // If the unit is waiting, decrease its wait time.
    // if it should no longer be waiting then request a new
    // path.
    void Update()
    {
        if (walking == false)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                if (target != null)
                {
                    FreeSpot(target);
                }
                PathRequestManager.AvailableSpot(new SpotRequest(target, OnSpotFound));
                if (target != null)
                {
                    walking = true;
                    waitTime = Random.Range(1f, 2f);
                    PathRequestManager.UpdateGrid(new GridUpdate(currentNode, -weight));
                    PathRequestManager.RequestPath(new PathRequest(transform.position, target.location, OnPathFound));
                }
            }
        }
    }

    // If a spot was found, set the target to the found spot.
    public void OnSpotFound(Spot outTarget, bool spotSearchSuccessful)
    {
        if (spotSearchSuccessful)
        {
            target = outTarget;
        }
    }

    // Release the spot that was being stood on once
    // it has been left.
    void FreeSpot(Spot spot)
    {
        PathRequestManager.releaseSpot(spot);
    }

    // If a node was successfully found, set the currentNode
    // to the node found.
    public void OnNodeFound(Node node, bool conversionSuccessful)
    {
        if (conversionSuccessful)
        {
            currentNode = node;
        }
    }

    // Once a path has been found, set the units path, weights and
    // being the FollowPath coroutine.
    public void OnPathFound(Node[] newPath, int[] pathWeights, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            weights = pathWeights;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    // Have the unit follow its path, if it reaches the end,
    // it will wait at the gathering spot and remove its path.
    // Also handles unit rotation to face walking direction.
    IEnumerator FollowPath()
    {
        Node oldNode = currentNode;
        currentNode = path[0];
        targetIndex = 0;
        changingPath = false;
        Vector3 currentWaypoint = currentNode.worldPos;
        PathRequestManager.UpdateGrid(new GridUpdate(currentNode, weight));

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    walking = false;
                    yield break;
                }
                oldNode = currentNode;
                currentNode = path[targetIndex];
                currentWaypoint = currentNode.worldPos;
                
                for (int i = 0; i < 3; i++)
                {
                    if (targetIndex + i >= path.Length)
                    {
                        break;
                    }
                    if (path[targetIndex + i].movementWeight > weights[targetIndex + i] + weight)
                    {
                        changingPath = true;
                        
                        PathRequestManager.UpdateGrid(new GridUpdate(oldNode, -weight));
                        PathRequestManager.RequestPath(new PathRequest(transform.position, target.location, OnPathFound));
                        break;
                    }
                }

                if (!changingPath)
                {

                    PathRequestManager.UpdateGrid(new GridUpdate(oldNode, -weight));
                    PathRequestManager.UpdateGrid(new GridUpdate(currentNode, weight));
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            Quaternion targetRotation = transform.rotation;
            if (currentWaypoint - transform.position != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(currentWaypoint - transform.position);
            }
            if (targetRotation != transform.rotation)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null && drawPathGizmos)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i].worldPos + new Vector3(0, 1, 0), new Vector3(0.2f, 0.2f, 0.2f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i].worldPos);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1].worldPos, path[i].worldPos);
                }
            }
        }
    }
}
