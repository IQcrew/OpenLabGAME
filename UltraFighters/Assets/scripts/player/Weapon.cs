using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform FirePoint;
    public GameObject StandardBulletPrefab;
    private bool ReadyToFire = false;


    public Gun[] AllGuns = new Gun[2];

    
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
                if (AllGuns[1].fire())
                {
                    Instantiate(AllGuns[1].Bullet, FirePoint.position, FirePoint.rotation);
                }
                
            }
        }
        
    }
}


