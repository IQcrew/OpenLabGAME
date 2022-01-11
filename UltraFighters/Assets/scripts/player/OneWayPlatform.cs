using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] public GameObject currectPlayer;
    [SerializeField] private BoxCollider2D PlayerHitBox;
    [SerializeField] private float DoubleTapTime = 0.5f;
    private float LastKeyDown;
    public static GameObject currentPlatform;
    private Player playerScript;
    private void Start()
    {
        playerScript = currectPlayer.GetComponent<Player>();
    }
    void Update()
    {
        if (!playerScript.shooting)
        {
            Debug.Log(currentPlatform);
            if (Input.GetKeyDown(playerScript.Down) && currentPlatform != null)
            {
                if ((Time.time - LastKeyDown) <= DoubleTapTime)
                {
                    Debug.Log("Disabled!");
                    StartCoroutine(DisableCollision());
                }
                LastKeyDown = Time.time;
            }
        }
    }
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformHitBox = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(PlayerHitBox, platformHitBox);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(PlayerHitBox, platformHitBox, false);
    }
}
