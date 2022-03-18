using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCharge : MonoBehaviour
{
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Player>().readyToTeleport = true;
    }
}
