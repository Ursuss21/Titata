using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float lifeTime;

    void Start()
    {
        StartCoroutine(DeathDelay());
        transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
    }

    IEnumerator DeathDelay(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyController>().Death();
        }
    }
}
