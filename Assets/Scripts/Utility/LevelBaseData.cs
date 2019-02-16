using UnityEngine;

[System.Serializable]
public class LevelBaseData
{
    [Tooltip("A new enemy is spawned when the player advances this distance. \n" +
            "The distance reduces after each spawn (not below the minimum value however) resulting in increased difficulty.")]
    public float SpawnStartInterval;
    public float MinSpawnInterval;
    public float IntervalMultiplier;

    [Tooltip("When an enemy is spawned, the corresponding enemy size number is substracted from the reserve. \n" +
        "The initial start reserve is multiplied with the level number. \n" +
        "Enemies are spawned until the reserve runs empty. \n")]
    public int EnemyStartReserve;

    public Enemy[] Enemies;
}
