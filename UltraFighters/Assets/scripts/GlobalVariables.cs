using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float TimeInGame = 0;



    // Update is called once per frame
    void Update()
    {
        TimeInGame = Time.time;
    }
}
