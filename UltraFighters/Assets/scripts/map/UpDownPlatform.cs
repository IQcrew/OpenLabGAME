using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPlatform : MonoBehaviour
{
    [SerializeField] GameObject StartPoint;
    [SerializeField] GameObject EndPoint;
    [SerializeField] int timeForOneLoop = 10;

    private Vector3 StartPointPos;
    private Vector3 EndPointPos;
    private Vector3 oneHop;
    private float alphaTime = 0f; 
    void Start()
    {
        StartPointPos = StartPoint.transform.position;
        EndPointPos = EndPoint.transform.position;
        this.gameObject.transform.position = StartPointPos;
        oneHop = (EndPointPos - StartPointPos)/(timeForOneLoop/2);
    }
    private void Update()
    {
        alphaTime += Time.deltaTime;
        if(alphaTime < (timeForOneLoop/2)){ this.gameObject.transform.position += (oneHop * Time.deltaTime); }
        else if(alphaTime < timeForOneLoop) { this.gameObject.transform.position -= (oneHop * Time.deltaTime); }
        else { alphaTime = 0f; }
    }


}
