using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform FirePoint;
    public GameObject BulletPrefab;



    private bool ReadyToFire = false;


    void Update()
    {
        if (Player.shooting)
        {
            if (Input.GetKey(GlobalVariables.P1fire))
            {
                ReadyToFire = true;
            }
            else if (ReadyToFire){
                ReadyToFire = false;
                shoot();
            }
        }
        
    }
    void shoot()
    {
        Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
    }
}
