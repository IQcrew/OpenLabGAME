using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTexture : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start() { spriteRenderer.enabled = false; }
    void Update()
    {
        if (Player.shooting == true) { spriteRenderer.enabled = true; }
        else { spriteRenderer.enabled = false; }
    }
}
