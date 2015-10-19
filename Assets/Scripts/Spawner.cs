using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy enemy;

    public event System.Action<int> OnNewWave;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    Transform playerT;
    LivingEntity player;

    Vector3 oldCampingPosition;
    float campingThreshold = 5;
    float timeBetweenCampingCheck = 2;
    float nextCampingCheckTime;
    bool isCamping;

    MapGenerator map;

    bool isDisable;

    void Start(){
        map = FindObjectOfType<MapGenerator>();

        player = FindObjectOfType<Player>();
        playerT = player.transform;
        player.OnDeath += OnPlayerDeath;

        nextCampingCheckTime = Time.time + timeBetweenCampingCheck;
        isCamping = false;

        isDisable = false;

        NextWave();
    }

    void Update(){
        if (!isDisable)
        {
            if (Time.time > nextCampingCheckTime)
            {
                nextCampingCheckTime = Time.time + timeBetweenCampingCheck;
                isCamping = Vector3.Distance(playerT.position, oldCampingPosition) < campingThreshold;
                oldCampingPosition = playerT.position;
            }


            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy(){
        float spawnDelay = 1;
        float spawnTimer = 0;
        float flashSpeed = 4;

        Transform tile = isCamping ? map.GetTileFromPosition(playerT.position) : map.GerRandomOpenTile();
        Material tileMat = tile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;

        while (spawnTimer <= spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColor,flashColor, Mathf.PingPong(spawnTimer * flashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        tileMat.color = initialColor;

        Enemy spawnedEnemy = Instantiate(enemy, tile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;

    }

    void OnPlayerDeath(){
        isDisable = true;
    }

    void OnEnemyDeath(){
        enemiesRemainingAlive--;
        print(enemiesRemainingAlive);

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void ResetPlayerPosition(){
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up;
    }

    void NextWave(){
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves [currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if(OnNewWave != null){
                OnNewWave(currentWaveNumber);
                ResetPlayerPosition();
            }
        }
    }

    [System.Serializable]
    public class Wave{
        public int enemyCount;
        public float timeBetweenSpawns;
    }

}
