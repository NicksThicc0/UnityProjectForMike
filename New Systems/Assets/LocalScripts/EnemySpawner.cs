using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    [SerializeField] bool spawnOnStart = true;

    [SerializeField] int amountOfEnemies;
    [SerializeField] GameObject[] Enemies;
    [SerializeField] GameObject[] Bosses;
    public Vector2 maxPos, minPos, spawnPos;
    [SerializeField] Transform Player;
    [SerializeField] List<GameObject> spawntEnemies;
    int amountOfEnemiesSpawnts;
    [Header("Current Round")]
    [SerializeField]int currentRound;
    [Header("UI")]
    public GameObject bossUi;
    public Image bossIcon;
    public Image bossFillBar;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!spawnOnStart) return;
        spawnMobs();
    }


    public void getRandArenaPos(Vector2 randPos)
    {
         randPos = new Vector2(Random.Range(maxPos.x, minPos.x), Random.Range(maxPos.y, minPos.y));
    }

    public void spawnMobs()
    {
        amountOfEnemiesSpawnts = 0;

        //Enemies

        for (int i = 0; i < amountOfEnemies; i++)
        {
            if (spawntEnemies.Count == amountOfEnemies) return;
            spawnPos = new Vector2(Random.Range(maxPos.x, minPos.x), Random.Range(maxPos.y, minPos.y));
            if (!Physics2D.OverlapCircle(spawnPos, .5f))
            {
                if (amountOfEnemiesSpawnts == amountOfEnemies) return;
                GameObject newEnemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)], spawnPos, Quaternion.Euler(0, 0, 0));
                spawntEnemies.Add(newEnemy);
                amountOfEnemiesSpawnts++;
            }
            if (amountOfEnemiesSpawnts != amountOfEnemies & i == amountOfEnemies - 1)
            {
                Start();
                Debug.Log("didnt spawn all trying again");
            }
            else if (amountOfEnemiesSpawnts == amountOfEnemies)
            {
                Debug.Log("all spawnt!");
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawntEnemies.Count; i++)
        {
            if (spawntEnemies[i] == null)
            {
                spawntEnemies.RemoveAt(i);
            }
        }
        if(spawntEnemies.Count <= 0)
        {
            Start();
            currentRound++;

        }
    }
}
