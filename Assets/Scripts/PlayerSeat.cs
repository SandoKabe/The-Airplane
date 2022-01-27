using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeat : MonoBehaviour
{
    // Used to place the player at the good higher in the plane
    public Transform airPlane;

    bool playerIsAlreadySeat;
    Transform rig;
    Vector3 playerPosition;
    float seatPosition = 1.18f;
    float originalPosition = 1.8f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Used to place the camera offset at the good place when the player seat in the airplane.
    private void OnTriggerEnter(Collider other)
    {
        if (!playerIsAlreadySeat && other.gameObject.CompareTag("MainCamera"))
        {
            rig = other.gameObject.transform.root.GetComponent<MasterController>().Rig.transform;
            rig.SetParent(airPlane);
            playerPosition = rig.transform.position;
            playerPosition.y = seatPosition;
            rig.transform.position = playerPosition;
            rig.transform.Rotate(0, 90f, 0);
            playerIsAlreadySeat = true;
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") && rb.velocity == Vector3.zero)
        {
            //rig = other.gameObject.transform.root.GetComponent<MasterController>().Rig.transform;
            rig.SetParent(null);
            playerPosition = rig.transform.position;
            playerPosition.y = originalPosition;
            rig.transform.position = playerPosition;

            playerIsAlreadySeat = false;
        }
    }
}
