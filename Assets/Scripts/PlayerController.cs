using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player speed.
    public float speed;
    Rigidbody2D rigidbody;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    //Updates player position based on input.
    void Update()
    {
        float horizontal = Input.GetAxis("MoveHorizontal");
        float vertical = Input.GetAxis("MoveVertical");

        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        if((shootHorizontal != 0 || shootVertical != 0) && Time.time > lastFire + fireDelay){
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }

        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
    }

    void Shoot(float x, float y){
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
    }
}
