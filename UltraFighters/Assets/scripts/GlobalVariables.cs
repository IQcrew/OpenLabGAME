using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float TimeInGame = 0;

    
    //player 1:
    public static int P1Health = 100;
    public static KeyCode P1Right = KeyCode.RightArrow;          
    public static KeyCode P1Left = KeyCode.LeftArrow;
    public static KeyCode P1Up = KeyCode.UpArrow;
    public static KeyCode P1Down = KeyCode.DownArrow;
    public static KeyCode P1hit = KeyCode.N;
    public static KeyCode P1fire = KeyCode.M;
    public static KeyCode P1slot = KeyCode.K;
    //player2:
    public static int P2Health = 100;
    public static KeyCode P2Right;
    public static KeyCode P2Left;
    public static KeyCode P2Up;
    public static KeyCode P2Down;
    public static KeyCode P2hit;
    public static KeyCode P2fire;
    public static KeyCode P2slot;


    // Update is called once per frame
    void Update()
    {
        TimeInGame = Time.time;
    }
}
