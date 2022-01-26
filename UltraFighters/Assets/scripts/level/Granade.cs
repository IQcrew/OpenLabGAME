using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{

    [SerializeField] GameObject explosion;
    [SerializeField] float waitTime;

    void Start()
    {
        StartCoroutine(tickOff());
    }
    private IEnumerator tickOff()
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}


