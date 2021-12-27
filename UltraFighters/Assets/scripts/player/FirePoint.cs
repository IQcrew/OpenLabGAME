using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{   

    [SerializeField] private Transform RotationCenter;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float RotationSpeed, RotationRadius;

    private float posX, posY, angle;

    // Update is called once per frame
    void Update()
    {
        if (Player.shooting) { spriteRenderer.enabled = true; }
        else { spriteRenderer.enabled = false; }
        var rotationVector = transform.rotation.eulerAngles;
        if (Player.PlayerRotation == "Right")
        {
            if (Player.shooting)
            {
                if (Input.GetKey(GlobalVariables.P1Up) && (!Input.GetKey(GlobalVariables.P1Down)))
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
                else if (Input.GetKey(GlobalVariables.P1Down) && (!Input.GetKey(GlobalVariables.P1Up)))
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
        else if (Player.PlayerRotation == "Left")
        {
            if (Player.shooting)
            {
                if (Input.GetKey(GlobalVariables.P1Up) && (!Input.GetKey(GlobalVariables.P1Down)))
                {
                    posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                    posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                    transform.position = new Vector2(posX, posY);

                    if (angle > 90f * Mathf.Deg2Rad) { angle -= Time.deltaTime * RotationSpeed; }
                }
                else if (Input.GetKey(GlobalVariables.P1Down) && (!Input.GetKey(GlobalVariables.P1Up)))
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
