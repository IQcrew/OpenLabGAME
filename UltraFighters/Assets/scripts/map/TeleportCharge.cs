using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCharge : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().readyToTeleport = true;
    }
}
