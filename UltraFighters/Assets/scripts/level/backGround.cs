using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backGround : MonoBehaviour
{
    [SerializeField] GameObject BackGround;
    [SerializeField] GameObject BackGroundShift;
    Transform transformBG;
    Transform transformBGS;
    Vector3 centerPosition;
    float shiftPosX;
    float shiftPosY;
    void Start()
    {
        transformBG = BackGround.GetComponent<Transform>();
        transformBGS = BackGroundShift.GetComponent<Transform>();
        centerPosition = transformBG.position;
    }

    void Update()
    {
        if(Input.mousePosition.x >= 0 && Input.mousePosition.y >= 0 && Input.mousePosition.x <= Screen.width && Input.mousePosition.y <= Screen.height)
        shiftPosX = Input.mousePosition.x/ Screen.width - 0.5f;
        shiftPosY = Input.mousePosition.y/ Screen.height - 0.5f;
        transformBG.position = centerPosition+ new Vector3(Screen.width*(shiftPosX*0.02f), Screen.height*(shiftPosY*0.02f), 0);
        transformBGS.position = centerPosition + new Vector3(Screen.width * (shiftPosX * 0.015f), Screen.height * (shiftPosY * 0.015f), 0);
        Debug.Log(Input.mousePosition);


    }
}
