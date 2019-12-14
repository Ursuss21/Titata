using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    public Vector2Int Position {get; set; }

    //Sets dungeon crawler starting position, from which the dungeon will be generated.
    public DungeonCrawler(Vector2Int startPos){
        Position = startPos;
    }

    //Returns dungeon crawler new position after moving.
    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap){
        Direction toMove = (Direction)Random.Range(0, directionMovementMap.Count);
        Position += directionMovementMap[toMove];
        return Position;
    }
}
