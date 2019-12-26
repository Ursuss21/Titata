using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float lifeTime;
    public bool isEnemyBullet = false;
    public float bulletSpeed;
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private Vector2 playerPosition;

    private void Start()
    {
        StartCoroutine(DeathDelay());
        if(!isEnemyBullet){
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    private void Update() {
        if(isEnemyBullet){
            currentPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, bulletSpeed * Time.deltaTime);
            if(currentPosition == lastPosition){
                Destroy(gameObject);
            }
            lastPosition = currentPosition;
        }
    }

    public void GetPlayer(Transform player){
        playerPosition = player.position;
    }

    IEnumerator DeathDelay(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy" && !isEnemyBullet){
            other.gameObject.GetComponent<EnemyController>().Death();
        }
        if(other.tag == "Player" && isEnemyBullet){
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
    }
}
