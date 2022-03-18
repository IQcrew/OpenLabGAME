using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorsTeleport : MonoBehaviour
{
    [SerializeField] GameObject SecondDoors;

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Player>().readyToTeleport = false;
        other.gameObject.transform.position = SecondDoors.transform.position;

    }
}
