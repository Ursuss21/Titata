using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Room currentRoom;
    public float moveSpeedWhenRoomChange;
    void Awake() {
        instance = this;
    }

    void Update()
    {
        UpdatePosition();
    }
    
    //Updates camera position after entering new room.
    void UpdatePosition(){
        if(currentRoom == null){
            return;
        }
        Vector3 targetPos = GetCameraTargetPostion();
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    //Returns room center, where the camera should be placed.
    Vector3 GetCameraTargetPostion(){
        if(currentRoom == null){
            return Vector3.zero;
        }
        Vector3 targetPos = currentRoom.GetRoomCenter();
        targetPos.z = transform.position.z;
        return targetPos;
    }
    
    public bool IsSwitchingScene(){
        return transform.position.Equals(GetCameraTargetPostion()) == false;
    }
}
