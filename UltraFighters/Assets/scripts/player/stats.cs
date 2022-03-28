using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stats : MonoBehaviour
{
    [SerializeField] Transform ThisTransform;
    [SerializeField] GameObject FollowedObject;
    [SerializeField] GameObject HealthBar;
    [SerializeField] Vector3 offset;
    //render layer sorting
    [SerializeField] Renderer MyRenderer;
    public string sortingLayerName = "Default"; //initialization before the methods
    public int orderInLayer = 0;

    private Player followedplayer;
    private SpriteRenderer Bar;
    void Start()
    {
        if (sortingLayerName != string.Empty)
        {
            MyRenderer.sortingLayerName = sortingLayerName;
            MyRenderer.sortingOrder = orderInLayer;
        }
        followedplayer = FollowedObject.GetComponent<Player>();
        Bar = HealthBar.GetComponent<SpriteRenderer>();
        if(FollowedObject.name == "Player_1")
            gameObject.GetComponent<TextMesh>().text = dataManager.gameData.NicknameP1;
        else
            gameObject.GetComponent<TextMesh>().text = dataManager.gameData.NicknameP2;
    }

    void Update()
    {
        try
        {
            ThisTransform.transform.position = FollowedObject.transform.position + offset;
            if (followedplayer.getHealt > followedplayer.MaxHealth / 1.5) { Bar.color = Color.green; }
            else if (followedplayer.getHealt > followedplayer.MaxHealth / 2.5) { Bar.color = Color.yellow; }
            else { Bar.color = Color.red; }
            HealthBar.transform.localScale = new Vector3(1.7f * (followedplayer.getHealt / followedplayer.MaxHealth), 0.25f, 1);
        }
        catch { Destroy(gameObject); }
    }
}
