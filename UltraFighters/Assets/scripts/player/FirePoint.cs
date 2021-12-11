using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public Transform RotationCenter;
    [SerializeField]
    private float RotationSpeed, RotationRadius;

    private float posX, posY, angle;

    // Update is called once per frame
    void Update()
    {
        if (Player.PlayerRotation == "Right")
        {
            if (Player.shooting)
            {
                if (Input.GetKey(GlobalVariables.P1Up) && (!Input.GetKey(GlobalVariables.P1Down)))
                {
                    posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                    posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                    transform.position = new Vector2(posX, posY);

                    if (angle < 90f * Mathf.Deg2Rad || angle >= 269f * Mathf.Deg2Rad)
                    {
                        angle += Time.deltaTime * RotationSpeed;
                    }
                    if (angle >= 360f * Mathf.Deg2Rad) { angle = 0f; }
                }
                else if (Input.GetKey(GlobalVariables.P1Down) && (!Input.GetKey(GlobalVariables.P1Up)))
                {
                    posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                    posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                    transform.position = new Vector2(posX, posY);

                    if (angle <= 91f * Mathf.Deg2Rad || angle > 270f * Mathf.Deg2Rad)
                    {
                        angle -= Time.deltaTime * RotationSpeed;
                    }
                    if (angle <= 0f) { angle = 360f * Mathf.Deg2Rad; }
                }
            }
            else
            {
                angle = 0f;
                posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                transform.position = new Vector2(posX, posY);
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
            }
            else
            {
                angle = 180f * Mathf.Deg2Rad;
                posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
                posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
                transform.position = new Vector2(posX, posY);
            }    
        }
    }
}
