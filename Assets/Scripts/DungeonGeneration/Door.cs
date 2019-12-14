using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Defines door positions.
    public enum DoorType{
        top, left, bottom, right
    }

    public DoorType doorType;
    public GameObject doorCollider;
    private GameObject player;

    //Defines offsets, which will be used when entering new room.
    private float heightOffset = 0.8f;
    private float widthOffset = 1.2f;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //Checks if player went through the door and updates his position, so he doesn't end up stuck in the wall.
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            switch(doorType){
                case DoorType.bottom:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y - heightOffset);
                    break;
                case DoorType.left:
                    player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
                    break;
                case DoorType.right:
                    player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
                    break;
                case DoorType.top:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y + heightOffset);
                    break;
            }
        }
    }
}
