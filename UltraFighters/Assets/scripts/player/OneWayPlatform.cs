using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private BoxCollider2D PlayerHitBox;
    [SerializeField] private float DoubleTapTime = 0.5f;
    private float LastKeyDown;
    private GameObject currentPlatform;

    void Update()
    {
        if(Input.GetKeyDown(GlobalVariables.P1Down) && currentPlatform != null)
        {
            if((Time.time - LastKeyDown) <= DoubleTapTime)
            {
                StartCoroutine(DisableCollision());
            }
            LastKeyDown = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = collision.gameObject;
        }
            
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
            currentPlatform = null;
        
            
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformHitBox = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(PlayerHitBox, platformHitBox);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(PlayerHitBox, platformHitBox,false);
    }
}
