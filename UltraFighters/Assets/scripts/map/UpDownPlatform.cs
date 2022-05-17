using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPlatform : MonoBehaviour
{
    [SerializeField] GameObject StartPoint;
    [SerializeField] GameObject EndPoint;
    [SerializeField] float timeForOneLoop = 10;

    private Vector3 StartPointPos;
    private Vector3 EndPointPos;
    private Vector3 oneHop;
    private float alphaTime = 0f; 
    bool velUP = true;
    bool velDOWN = true;
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
        if(alphaTime < (timeForOneLoop/2)){ 
            velUP = true;
            if (velDOWN){
                velDOWN = false;
                 this.gameObject.GetComponent<Rigidbody2D>().velocity = oneHop;
            }
        }
        else if(alphaTime < timeForOneLoop) { 
            velDOWN = true;
            if (velUP){
                velUP = false;
                this.gameObject.GetComponent<Rigidbody2D>().velocity = -oneHop;
            }
            }
        else { alphaTime = 0f;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.gameObject.transform.position = StartPointPos;
        }
    }
}
