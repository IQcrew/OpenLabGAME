using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class multipletargetscamera : MonoBehaviour         // TOBIAS - Program umoûnuje efektÌvne chodiù a zoomovaù na viac hr·Ëov
{
    public List<Transform> targets;

    public float smoothTime = .5f;
    public float minZoom = 40f;

    private Vector3 velocity;


    private void LateUpdate()           //update camery
    {
        if (targets.Count == 0) { Debug.Log("CAMERA-Nem· OBJEKTY NA SLEDOVANIE"); return; }
        move();
        zoom();
    }


    Vector3 GetCenterPoint()            // najde to stred medzi viacerimi objektmi
    {
        try
        {
            if (targets.Count == 1) { return targets[0].position; }
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }
        catch
        {
            if (targets[0] != null) { return targets[0].position; }
            else { return targets[1].position; }
        }
    }


    private void move()   // pohybuje camerou v horizontalnom a vertikalnom smere
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + zoom();

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        
    }


    private Vector3 zoom()     // returnuje z suradnicu pre akoby zoom
    {
        
        return new Vector3(0f,0f, -(GetGreatestDistance()/2+minZoom));
    }


    float GetGreatestDistance()  //returne najvacsiu vzialenost medzi hr·cmi
    {
        try
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.size.x > bounds.size.y ? bounds.size.x : (bounds.size.y * 1.7f);
        }
        catch { return 0f; }
    }
}
