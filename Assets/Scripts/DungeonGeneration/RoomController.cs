using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

//Useful info about room.
public class RoomInfo {
    public string name;
    public int x;
    public int y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    //String responsible for recognizing levels.
    string currentWorldName = "Basement";
    RoomInfo currentLoadRoomData;
    Room currentRoom;

    //Queue for rooms waiting to be loaded.
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    //List of loaded rooms.
    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;

    //Bools responsible for making sure, that special rooms have spawned.
    bool spawnedBossRoom = false;
    bool spawnedTreasureRoom = false;
    bool spawnedShopRoom = false;
    bool updatedRooms = false;

    void Awake() {
        instance = this;
    }

    void Update() {
        UpdateRoomQueue();
    }

    //Updates room queue and calls a special rooms spawn functions.
    void UpdateRoomQueue(){
        if(isLoadingRoom){
            return;
        }
        if(loadRoomQueue.Count == 0){
            if(!spawnedBossRoom){
                SpawnBossRoom();
            }
            else if(!spawnedTreasureRoom && spawnedBossRoom){
                SpawnTreasureRoom();
            }
            else if(!spawnedShopRoom && spawnedTreasureRoom && spawnedBossRoom){
                SpawnShopRoom();
            }
            else if(spawnedBossRoom && spawnedTreasureRoom && spawnedShopRoom && !updatedRooms){
                foreach(Room room in loadedRooms){
                    room.RemoveUnconnectedDoors();
                }
                updatedRooms = true;
            }
            return;
        }
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    //Responsible for spawning special rooms.
    void SpawnBossRoom(){
        spawnedBossRoom = true;
        Room tempRoom = SpecialRoomPosition();
        LoadRoom("Boss", tempRoom.x, tempRoom.y);
    }

    void SpawnTreasureRoom(){
        spawnedTreasureRoom = true;
        Room tempRoom = SpecialRoomPosition();
        LoadRoom("Treasure", tempRoom.x, tempRoom.y);
    }

    void SpawnShopRoom(){
        spawnedShopRoom = true;
        Room tempRoom = SpecialRoomPosition();
        LoadRoom("Shop", tempRoom.x, tempRoom.y);
    }

    //Randomly generated positions for special rooms. Special room can neighbour only one normal room.
    Room SpecialRoomPosition(){
        Room tempRoom = null;
        GameObject tempGameObject = new GameObject();
        int variant;
        while(tempRoom == null){
            variant = Random.Range(0, 4);
            foreach(Room room in loadedRooms){
                if(variant == 0 && room.GetTop()== null && room.name.Contains("Empty")){
                    if(FindRoom(room.x, room.y + 2) == null && FindRoom(room.x - 1, room.y + 1) == null && FindRoom(room.x + 1, room.y + 1) == null){
                        tempRoom = tempGameObject.AddComponent<Room>();
                        tempRoom.x = room.x;
                        tempRoom.y = room.y + 1;
                        return tempRoom;
                    }
                }
                else if(variant == 1 && room.GetRight()== null && room.name.Contains("Empty")){
                    if(FindRoom(room.x + 2, room.y) == null && FindRoom(room.x + 1, room.y + 1) == null && FindRoom(room.x + 1, room.y - 1) == null){
                        tempRoom = tempGameObject.AddComponent<Room>();
                        tempRoom.x = room.x + 1;
                        tempRoom.y = room.y;
                        return tempRoom;
                    }
                }
                else if(variant == 2 && room.GetBottom()== null && room.name.Contains("Empty")){
                    if(FindRoom(room.x, room.y - 2) == null && FindRoom(room.x - 1, room.y - 1) == null && FindRoom(room.x + 1, room.y - 1) == null){
                        tempRoom = tempGameObject.AddComponent<Room>();
                        tempRoom.x = room.x;
                        tempRoom.y = room.y - 1;
                        return tempRoom;
                    }
                }
                if(variant == 3 && room.GetLeft()== null && room.name.Contains("Empty")){
                    if(FindRoom(room.x - 2, room.y) == null && FindRoom(room.x - 1, room.y + 1) == null && FindRoom(room.x - 1, room.y - 1) == null){
                        tempRoom = tempGameObject.AddComponent<Room>();
                        tempRoom.x = room.x - 1;
                        tempRoom.y = room.y;
                        return tempRoom;
                    }
                }
            }
        }
        return null;
    }

    //Responsible for enqueueing new rooms to spawn.
    public void LoadRoom(string name, int x, int y){
        if(DoesRoomExist(x, y)){
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.x = x;
        newRoomData.y = y;
        loadRoomQueue.Enqueue(newRoomData);
    }

    //Responsible for creating new scene in which new room is stored.
    IEnumerator LoadRoomRoutine(RoomInfo info){
        string roomName = currentWorldName + info.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        while(loadRoom.isDone == false){
            yield return null;
        }
    }

    //Setting room to spawn or ignoring the spawn if room already exists in this position.
    public void RegisterRoom(Room room){
        if(!DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y)){
            room.transform.position = new Vector3(
                currentLoadRoomData.x * room.width,
                currentLoadRoomData.y * room.height,
                0
            );
            room.x = currentLoadRoomData.x;
            room.y = currentLoadRoomData.y;
            room.name = currentWorldName + "_" + currentLoadRoomData.name + " " + room.x + ", " + room.y;
            room.transform.parent = transform;
            isLoadingRoom = false;
            if(loadedRooms.Count == 0){
                CameraController.instance.currentRoom = room;
            }
            loadedRooms.Add(room);
        }
        else{
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    
    //Checking if room exists in x and y position.
    public bool DoesRoomExist(int x, int y){
        return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    }

    //Finding room in x and y position.
    public Room FindRoom(int x, int y){
        return loadedRooms.Find(item => item.x == x && item.y == y);
    }
    
    //Updating current room for player.
    public void OnPlayerEnterRoom(Room room){
        CameraController.instance.currentRoom = room;
        currentRoom = room;
    }
}
