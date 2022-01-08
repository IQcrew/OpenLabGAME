using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTextureManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform hand;
    private static Transform currentHand;

    void Start()
    {
        spriteRenderer.enabled = false;
    }
    void Update()
    {
        FirePoint.RotationCenter = currentHand;
        if (Player.shooting)
        {
            if (spriteRenderer.sprite.name == "PlayerShootHand1" && Player.PlayerGun.ShootingTexture.name == "PlayerShootHand1")
            {
                spriteRenderer.enabled = true;
                currentHand = hand;
                if (Player.PlayerRotation == "Right") { spriteRenderer.flipY = false; }
                else if (Player.PlayerRotation == "Left") { spriteRenderer.flipY = true; }
                float angle = Mathf.Atan2(firePoint.position.y - transform.position.y, firePoint.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Infinity);
            }
            else if ((spriteRenderer.sprite.name == "PlayerShootHand2.1" || spriteRenderer.sprite.name == "PlayerShootHand2.2") && Player.PlayerGun.ShootingTexture.name == "PlayerShootHand2.1")
            {
                spriteRenderer.enabled = true; // toto cele neber vazne to je len aby tu neco bolo xdddd
                currentHand = hand;
                if (Player.PlayerRotation == "Right") { spriteRenderer.flipY = false; }
                else if (Player.PlayerRotation == "Left") { spriteRenderer.flipY = true; }
                float angle = Mathf.Atan2(firePoint.position.y - transform.position.y, firePoint.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Infinity);
            }
            else if (spriteRenderer.sprite.name == "PlayerShootHead")
            {
                spriteRenderer.enabled = true;
                if (Player.PlayerRotation == "Right") { spriteRenderer.flipY = false; }
                else if (Player.PlayerRotation == "Left") { spriteRenderer.flipY = true; }
                transform.rotation = currentHand.transform.rotation;
            }
            else if (spriteRenderer.sprite.name == "PlayerShootBody")
                spriteRenderer.enabled = true;
        }
        else
            spriteRenderer.enabled = false;
    }
}
