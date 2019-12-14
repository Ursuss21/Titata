using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Room size and position in the level.
    public float width;
    public float height;
    public int x;
    public int y;
    
    //Bool for checking if unnecessary doors were removed.
    private bool updatedDoors = false;


    public Room(int x, int y){
        this.x = x;
        this.y = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    public List<Door> doors = new List<Door>();
    
    //Initialize room with its doors.
    void Start()
    {
        if(RoomController.instance == null){
            Debug.LogError("You pressed play in the wrong scene!");
            return;
        }
        Door[] ds = GetComponentsInChildren<Door>();
        foreach(Door d in ds){
            doors.Add(d);
            switch(d.doorType){
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }
        RoomController.instance.RegisterRoom(this);
    }

    //Waits for special rooms to spawn, to update their doors.
    void Update() {
        if((name.Contains("Boss") || name.Contains("Treasure") || name.Contains("Shop"))&& !updatedDoors){
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    //Removes unnecessary doors.
    public void RemoveUnconnectedDoors(){
        foreach(Door door in doors){
            switch(door.doorType){
                case Door.DoorType.right:
                    if(GetRight() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.left:
                    if(GetLeft() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.top:
                    if(GetTop() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.bottom:
                    if(GetBottom() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    //Gets the neighbouring rooms depending on the direction.
    public Room GetRight(){
        if(RoomController.instance.DoesRoomExist(x + 1, y)){
            return RoomController.instance.FindRoom(x + 1, y);
        }
        else{
            return null;
        }
    }

    public Room GetLeft(){
        if(RoomController.instance.DoesRoomExist(x - 1, y)){
            return RoomController.instance.FindRoom(x - 1, y);
        }
        else{
            return null;
        }
    }

    public Room GetTop(){
        if(RoomController.instance.DoesRoomExist(x, y + 1)){
            return RoomController.instance.FindRoom(x, y + 1);
        }
        else{
            return null;
        }
    }

    public Room GetBottom(){
        if(RoomController.instance.DoesRoomExist(x, y - 1)){
            return RoomController.instance.FindRoom(x, y - 1);
        }
        else{
            return null;
        }
    }

    //Draws gizmos.
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    //Returns the room center vector.
    public Vector3 GetRoomCenter(){
        return new Vector3(x * width, y * height);
    }
    
    //Checks if player entered the room.
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
