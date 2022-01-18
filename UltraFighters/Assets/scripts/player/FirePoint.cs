using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    [SerializeField] private GameObject currectPlayer;
    [SerializeField] private Transform RotationCenter;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float RotationSpeed, RotationRadius;
    private float posX, posY, angle;
    private Player playerScript;
    // Update is called once per frame
    private void Start()
    {
        playerScript = currectPlayer.GetComponent<Player>();
    }
    void Update()
    {
        if (playerScript.shooting) { spriteRenderer.enabled = true; }
        else { spriteRenderer.enabled = false; }
        var rotationVector = transform.rotation.eulerAngles;
        if (playerScript.PlayerRotationRight)
        {
            if (playerScript.shooting)
            {
                if (Input.GetKey(playerScript.Up) && (!Input.GetKey(playerScript.Down)))
                {
                    posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                    posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                    transform.position = new Vector2(posX, posY);

                    if (angle < 90f * Mathf.Deg2Rad || angle >= 265f * Mathf.Deg2Rad)
                    {
                        angle += Time.deltaTime * RotationSpeed;
                    }
                    else { angle = 90f * Mathf.Deg2Rad; }

                    if (angle >= 360f * Mathf.Deg2Rad) { angle = 0f; }
                }
                else if (Input.GetKey(playerScript.Down) && (!Input.GetKey(playerScript.Up)))
                {
                    posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                    posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                    transform.position = new Vector2(posX, posY);

                    if (angle <= 95f * Mathf.Deg2Rad || angle > 270f * Mathf.Deg2Rad)
                    {
                        angle -= Time.deltaTime * RotationSpeed;
                    }
                    else { angle = 270f * Mathf.Deg2Rad; }

                    if (angle <= 0f) { angle = 360f * Mathf.Deg2Rad; }
                }
                rotationVector.z = angle * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(rotationVector);
            }
            else
            {
                angle = 0f;
                posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                transform.position = new Vector2(posX, posY);
                rotationVector.z = 180;
                transform.rotation = Quaternion.Euler(rotationVector);
            }
            
        }
        else if (!playerScript.PlayerRotationRight)
        {
            if (playerScript.shooting)
            {
                if (Input.GetKey(playerScript.Up) && (!Input.GetKey(playerScript.Down)))
                {
                    posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                    posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                    transform.position = new Vector2(posX, posY);

                    if (angle > 90f * Mathf.Deg2Rad) { angle -= Time.deltaTime * RotationSpeed; }
                }
                else if (Input.GetKey(playerScript.Down) && (!Input.GetKey(playerScript.Up)))
                {
                    posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                    posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                    transform.position = new Vector2(posX, posY);

                    if (angle < 270f * Mathf.Deg2Rad) { angle += Time.deltaTime * RotationSpeed; }
                }
                rotationVector.z = 180 - (angle * Mathf.Rad2Deg);
                transform.rotation = Quaternion.Euler(rotationVector);
            }
            else
            {
                angle = 180f * Mathf.Deg2Rad;
                posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                transform.position = new Vector2(posX, posY);
                rotationVector.z = 0;
                transform.rotation = Quaternion.Euler(rotationVector);
            }
            
        }
    }
        
}
