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
        if(Player.shooting && Input.GetKey(GlobalVariables.P1Down) && (!Input.GetKey(GlobalVariables.P1Up)))
        {
            posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
            posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
            transform.position = new Vector2(posX, posY);
            angle = angle + Time.deltaTime * RotationSpeed;
            
            if (angle >= 360) { angle = 0; }
        }
        else if (Player.shooting && Input.GetKey(GlobalVariables.P1Up) && (!Input.GetKey(GlobalVariables.P1Down)))
        {
            posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
            posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
            transform.position = new Vector2(posX, posY);
            angle = angle - Time.deltaTime * RotationSpeed;
            

            if (angle <= 0) { angle = 360; }
        }
    }
}
