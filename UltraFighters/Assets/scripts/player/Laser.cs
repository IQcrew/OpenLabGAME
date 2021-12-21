using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject ThisGameObject;
    public Transform LaserPoint;
    public LineRenderer LineRender;
    Transform MyTransform;


    private void Awake()
    {
        MyTransform = GetComponent<Transform>();
    }
 
    public void ShootLaser(bool TurnOn)
    {
        if (TurnOn)
        {
            if (Physics2D.Raycast(MyTransform.position, transform.right))
            {
                ThisGameObject.SetActive(true);
                RaycastHit2D Rhit = Physics2D.Raycast(MyTransform.position, transform.right);
                DrawRay(LaserPoint.position, Rhit.point);
                
            }
            else{ ThisGameObject.SetActive(false); }
            }
        else
        {
            ThisGameObject.SetActive(false);
        }
    }
    void DrawRay(Vector2 StartPos, Vector2 EndPos)
    {
        LineRender.SetPosition(0, StartPos);
        LineRender.SetPosition(1, EndPos);
    }

}
