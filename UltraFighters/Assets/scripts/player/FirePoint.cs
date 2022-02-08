using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    [SerializeField] private GameObject currectPlayer;
    [SerializeField] private Transform RotationCenter;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float RotationSpeed, RotationRadius;
    [SerializeField] private float RotationSmooth;
    private bool PlayerLastRotationRight;
    private float posX, posY, angle;
    private Player playerScript;
    
    // Update is called once per frame
    private void Start()
    {
        playerScript = currectPlayer.GetComponent<Player>();
    }
    public void FUpdate()
    {
        var rotationVector = transform.rotation.eulerAngles;
        if (playerScript.PlayerRotationRight)
        {
            if (PlayerLastRotationRight != playerScript.PlayerRotationRight) { angle = 0f; UpdatePosition(posX, posY, angle); }
            PlayerLastRotationRight = playerScript.PlayerRotationRight;
            if (Input.GetKey(playerScript.Up) && (!Input.GetKey(playerScript.Down)))
            {
                UpdatePosition(posX, posY, angle);
                if (angle < 90f * Mathf.Deg2Rad || angle >= 265f * Mathf.Deg2Rad) { angle += Time.deltaTime * RotationSpeed; RotationSpeed += RotationSpeed < 5 ? Time.deltaTime * RotationSmooth : 0; }
                else { angle = 90f * Mathf.Deg2Rad; RotationSpeed = 1; }
                if (angle >= 360f * Mathf.Deg2Rad) { angle = 0f; }
            }
            else if (Input.GetKey(playerScript.Down) && (!Input.GetKey(playerScript.Up)))
            {
                UpdatePosition(posX, posY, angle);
                if (angle <= 95f * Mathf.Deg2Rad || angle > 270f * Mathf.Deg2Rad) { angle -= Time.deltaTime * RotationSpeed; RotationSpeed += RotationSpeed < 5 ? Time.deltaTime * RotationSmooth : 0; }
                else { angle = 270f * Mathf.Deg2Rad;  }
                if (angle <= 0f) { angle = 360f * Mathf.Deg2Rad; }
            }
            else { RotationSpeed = 1; }
            UpdateRotation(rotationVector, angle * Mathf.Rad2Deg);
        }
        else
        {
            if (playerScript.shooting)
            {
                if (PlayerLastRotationRight != playerScript.PlayerRotationRight) { angle = 180f * Mathf.Deg2Rad; UpdatePosition(posX, posY, angle); }
                PlayerLastRotationRight = playerScript.PlayerRotationRight;
                if (Input.GetKey(playerScript.Up) && (!Input.GetKey(playerScript.Down)))
                {
                    UpdatePosition(posX, posY, angle);
                    if (angle > 90f * Mathf.Deg2Rad) { angle -= Time.deltaTime * RotationSpeed; RotationSpeed += RotationSpeed < 5 ? Time.deltaTime * RotationSmooth : 0; }
                }
                else if (Input.GetKey(playerScript.Down) && (!Input.GetKey(playerScript.Up)))
                {
                    UpdatePosition(posX, posY, angle);
                    if (angle < 270f * Mathf.Deg2Rad) { angle += Time.deltaTime * RotationSpeed; RotationSpeed += RotationSpeed < 5 ? Time.deltaTime * RotationSmooth : 0; }
                }
                else { RotationSpeed = 1; }
                UpdateRotation(rotationVector, 180 - (angle * Mathf.Rad2Deg));
            }
            else
            {
                angle = 180f * Mathf.Deg2Rad;
                UpdatePosition(posX, posY, angle);
                UpdateRotation(rotationVector, 0f);
            }
        }
    }
    private void UpdatePosition(float posX, float posY, float angle)
    {
        posX = RotationCenter.position.x + Mathf.Cos(angle) * RotationRadius;
        posY = RotationCenter.position.y + Mathf.Sin(angle) * RotationRadius;
        transform.position = new Vector2(posX, posY);
    }
    private void UpdateRotation(Vector3 rotationVector, float angle)
    {
        rotationVector.z = angle;
        transform.rotation = Quaternion.Euler(rotationVector);
    }
    public void exitFP()
    {
        playerScript = currectPlayer.GetComponent<Player>();
        var rotationVector = transform.rotation.eulerAngles;
        if (playerScript.PlayerRotationRight)
        {
            angle = 0f;
            UpdatePosition(posX, posY, angle);
            UpdateRotation(rotationVector, 180f);
        }
        else
        {
            angle = 180f * Mathf.Deg2Rad;
            UpdatePosition(posX, posY, angle);
            UpdateRotation(rotationVector, 0f);
        }
    }
        
}
