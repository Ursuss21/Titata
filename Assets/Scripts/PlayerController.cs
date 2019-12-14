using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player speed.
    public float speed;
    Rigidbody2D rigidbody;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    //Updates player position based on input.
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
    }
}
