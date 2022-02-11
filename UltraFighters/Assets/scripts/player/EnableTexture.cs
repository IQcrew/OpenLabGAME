using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTexture : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public GameObject currectPlayer;
    Player playerScript;
    void Start() { spriteRenderer.enabled = false; playerScript = currectPlayer.GetComponent<Player>(); }
    void Update()
    {
        if (playerScript.shooting == true || playerScript.granadePos) { spriteRenderer.enabled = true; }
        else { spriteRenderer.enabled = false; }
    }
}
