using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorsTeleport : MonoBehaviour
{
    [SerializeField] GameObject SecondDoors;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>().readyToTeleport) 
        other.gameObject.transform.position = SecondDoors.transform.position;
        other.gameObject.GetComponent<Player>().readyToTeleport = false;

    }
}
