using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTextureManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform hand;
    void Start()
    {
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (Player.shooting)
        {
            spriteRenderer.enabled = true;
            if (spriteRenderer.sprite.name == "UF-shoot_1")
            {
                transform.rotation = hand.transform.rotation; 
                if (Player.PlayerRotation == "Right") { spriteRenderer.flipY = false; }
                else if (Player.PlayerRotation == "Left") { spriteRenderer.flipY = true; }
            }
            else if (spriteRenderer.sprite.name == "UF-shoot_2")
            {
                if (Player.PlayerRotation == "Right") { spriteRenderer.flipY = false; }
                else if (Player.PlayerRotation == "Left") { spriteRenderer.flipY = true; }
                float angle = Mathf.Atan2(firePoint.position.y - transform.position.y, firePoint.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Infinity);
            }
        }
        else
            spriteRenderer.enabled = false;
    }
}
