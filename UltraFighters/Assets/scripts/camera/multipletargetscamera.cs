using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class multipletargetscamera : MonoBehaviour         // TOBIAS - Program umoûnuje efektÌvne chodiù a zoomovaù na viac hr·Ëov
{
    public List<Transform> targets;

    public Vector3 offset;
    public float smoothTime = .5f;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()           //update camery
    {
        if (targets.Count == 0) { Debug.Log("CAMERA-Nem· OBJEKTY NA SLEDOVANIE"); return; }
        move();
        zoom();
        Debug.Log(GetGreatestDistance());
    }


    Vector3 GetCenterPoint()            // najde to stred medzi viacerimi objektmi
    {
        if (targets.Count == 1) { return targets[0].position; }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }


    private void move()   // pohybuje camerou v horizontalnom a vertikalnom smere
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }


    private void zoom()     // podla najvecsej vzdialenosti hraca urËÌ zoom kamery.
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }


    float GetGreatestDistance()  //returne najvacsiu vzialenost medzi hr·cmi
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
}
