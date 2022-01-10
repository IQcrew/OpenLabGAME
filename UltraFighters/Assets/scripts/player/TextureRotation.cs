using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform firePoint;

    void Start() { spriteRenderer.enabled = false; }
    void Update()
    {
        if (Player.shooting)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = Player.PlayerGun.GunTexture;
            if (Player.PlayerRotation == "Right") { spriteRenderer.flipY = false; }
            else if (Player.PlayerRotation == "Left") { spriteRenderer.flipY = true; }
            float angle = Mathf.Atan2(firePoint.position.y - transform.position.y, firePoint.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Infinity);
        }
        else { spriteRenderer.enabled = false; }
    }
}
