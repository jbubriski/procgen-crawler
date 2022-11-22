using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDirector : MonoBehaviour
{
    private static LevelDirector _this;

    private Transform _floor;
    private Transform _traps;
    private Transform _decorations;
    private Transform _enemiesParent;
    private Transform _extras;
    private Transform _playerSpawnPoint;

    private FollowPlayer _followPlayer;

    private AstarPath _astarPath;

    public Transform Exit;

    public Transform DefaultTile;
    public Transform[] Tiles;
    public Transform[] Traps;
    public Transform[] Decorations;
    public Transform[] EnemyTransforms;
    public Transform PlayerSpawnPointPrefab;
    public Transform PlayerPrefab;

    public List<Transform> Enemies;

    public Text FloorValueUiText;

    public int GridSize = 20;
    public int TileSize = 4;

    public int Floor = 1;

    public static LevelDirector Instance
    {
        get
        {
            if (_this == null)
            {
                _this = FindObjectOfType<LevelDirector>();
            }

            return _this;
        }
    }

    public void Start()
    {
        Destroy(transform.Find("Placeholder").gameObject);

        _floor = transform.Find("Floor");
        _traps = transform.Find("Traps");
        _decorations = transform.Find("Decorations");
        _enemiesParent = transform.Find("Enemies");
        _extras = transform.Find("Extras");

        _followPlayer = FindObjectOfType<FollowPlayer>();

        _astarPath = FindObjectOfType<AstarPath>();

        ResetAndSetup();
    }

    public void Update()
    {

    }

    public void ResetAndSetup()
    {
        ResetMap();
        SetupMap();

        SpawnPlayer();
        _followPlayer.Reset();
    }

    public void ResetMap()
    {
        ClearObjects();

        Floor = 1;
        UpdateFloor();
    }

    public void NextFloor()
    {
        ClearObjects();
        SetupMap();

        Floor++;
        UpdateFloor();
    }

    private void ClearObjects()
    {
        ClearChildren(_floor);
        ClearChildren(_traps);
        ClearChildren(_decorations);
        ClearChildren(_enemiesParent);
        ClearChildren(_extras);
    }

    public void SetupMap()
    {
        transform.Rotate(0, -45, 0);

        var gridWidth = GridSize * TileSize;
        var spawnedExit = false;

        for (var i = 0; i < GridSize; i++)
        {
            for (var k = 0; k < GridSize; k++)
            {
                var newTilePosition = new Vector3(i * TileSize - gridWidth / 2, 0, k * TileSize - gridWidth / 2);

                if (_playerSpawnPoint == null && Random.value * 100 > 90 && i > 2) // Player Spawn - 10%
                {
                    Debug.Log("spawn");
                    _playerSpawnPoint = Instantiate(PlayerSpawnPointPrefab, transform);
                    _playerSpawnPoint.position = newTilePosition + new Vector3(0, 1, 0);

                    var newTile = Instantiate(DefaultTile, _floor);
                    newTile.position = newTilePosition;
                }
                else if (!spawnedExit && Random.value * 100 > 90) // Exit - 10%
                {
                    var exit = Instantiate(Exit, _floor);
                    exit.position = newTilePosition;
                    exit.Rotate(new Vector3(0, 180, 0));
                    spawnedExit = true;
                }
                else
                {
                    var tileType = Tiles.RandomElement();
                    var newTile = Instantiate(tileType, _floor);

                    newTile.position = newTilePosition;

                    if (tileType.name != "FloorWater")
                    {
                        if (Random.value * 100 > 95) // Traps - 5%
                        {
                            var newTrap = Instantiate(Traps.RandomElement(), _traps);
                            newTrap.position = newTilePosition;
                        }
                        else if (Random.value * 100 > 90) // Decorations - 10% 
                        {
                            var newDecoration = Instantiate(Decorations.RandomElement(), _decorations);
                            newDecoration.position = newTilePosition;
                        }
                        else if (Random.value * 100 > 95) // Enemy ~ 5% 
                        {
                            var newEnemy = Instantiate(EnemyTransforms.RandomElement(), _enemiesParent);
                            newEnemy.position = newTilePosition + new Vector3(2, 2, 2);
                            Enemies.Add(newEnemy.transform);
                        }
                    }
                }
            }
        }

        transform.Rotate(0, 45, 0);

        _astarPath.Scan();
    }

    public Transform SpawnPlayer()
    {
        var player = Instantiate(PlayerPrefab, transform);

        if (player != null)
        {
            player.position = _playerSpawnPoint.position;
        }

        return player;
    }

    private void UpdateFloor()
    {
        FloorValueUiText.text = Floor.ToString();
    }

    private void ClearChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var childTransform = parent.GetChild(i);

            Destroy(childTransform.gameObject);
        }
    }
}
