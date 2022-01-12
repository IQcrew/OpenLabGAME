using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stats : MonoBehaviour
{
    [SerializeField] Transform ThisTransform;
    [SerializeField] GameObject FollowedObject;
    [SerializeField] GameObject HealthBar;
    [SerializeField] Vector3 offset;
    private Player followedplayer;
    private SpriteRenderer Bar;
    void Start()
    {
        followedplayer = FollowedObject.GetComponent<Player>();
        Bar = HealthBar.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ThisTransform.transform.position = FollowedObject.transform.position+offset;
        if (followedplayer.getHealt > followedplayer.MaxHealth / 1.5) { Bar.color = Color.green; }
        else if (followedplayer.getHealt > followedplayer.MaxHealth / 2.5) { Bar.color = Color.yellow; }
        else { Bar.color = Color.red;}
        HealthBar.transform.localScale = new Vector3 (1.7f*(followedplayer.getHealt/followedplayer.MaxHealth),0.25f,1);
    }
}
