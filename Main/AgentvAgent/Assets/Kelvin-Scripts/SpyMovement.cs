using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam; //Takes camera looking direction

    public float speed = 2.5f;
    float turnSpeed = 5f;
    float turnSmoothVelocity;
    Node currentNode;
    Node oldNode;
    int weight = 4;

    void Start()
    {
        PathRequestManager.RequestWorldPosToNode(new WorldPosToNode(transform.position, OnNodeFound));
        if (currentNode != null)
        {
            oldNode = currentNode;
            PathRequestManager.UpdateGrid(new GridUpdate(currentNode, weight));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //If D/Right = 1, if A/Left = -1
        float vertical = Input.GetAxisRaw("Vertical"); //If W/Up = 1, if S/Down = -1
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //Finds the angle of movement

            if (targetAngle != transform.rotation.y) // Calculates the speed and direction of rotation for the player model
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, targetAngle, 0f), Time.deltaTime * turnSpeed); //Smooths the player rotation so it is not snappy
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime); //Moves the player

            PathRequestManager.RequestWorldPosToNode(new WorldPosToNode(transform.position, OnNodeFound));
            if (currentNode != oldNode)
            {
                PathRequestManager.UpdateGrid(new GridUpdate(oldNode, -weight));
                PathRequestManager.UpdateGrid(new GridUpdate(currentNode, weight));
                oldNode = currentNode;
            }
        }
        else
        {
            //PLAYER IS IDLE
        }

    }

    public void OnNodeFound(Node node, bool conversionSuccessful)
    {
        if (conversionSuccessful)
        {
            currentNode = node;
        }
    }
}
