using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Possible directions for dungeon crawler.
public enum Direction {
    up = 0,
    left = 1,
    down = 2,
    right = 3
};

public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();

    //Dictionary, in which we store vectors for the directions chosen by dungeon crawler.
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>{
        {Direction.up, Vector2Int.up},
        {Direction.left, Vector2Int.left},
        {Direction.down, Vector2Int.down},
        {Direction.right, Vector2Int.right}
    };

    //Main function to generate dungeon. Every dungeon crawler goes into a random direction. It adds to positions list. The number of positions depends on a random number between minimal and maximal number of iterations (rooms).
    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData){
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();
        for(int i = 0; i < dungeonData.numberOfCrawlers; ++i){
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }
        int iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);
        for(int i = 0; i < iterations; ++i){
            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers){
                Vector2Int newPosition = dungeonCrawler.Move(directionMovementMap);
                while(positionsVisited.Contains(newPosition)){
                    newPosition = dungeonCrawler.Move(directionMovementMap);
                }
                positionsVisited.Add(newPosition);
            }
        }
        return positionsVisited;
    }
}
