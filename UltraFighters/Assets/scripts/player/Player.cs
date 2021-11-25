using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float MovementX;
    Rigidbody2D myBody;
    [SerializeField]
    private float Speed = 5;
    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MovementX = Input.GetAxis("Horizontal");
        transform.position += new Vector3(MovementX, 0f, 0f) * Time.deltaTime * Speed;
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
            myBody.AddForce(new Vector2(0f, 10), ForceMode2D.Impulse);
    }

}
