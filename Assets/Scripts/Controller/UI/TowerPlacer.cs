using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowerPlacer : MonoBehaviour {

    public Transform spawnTransform;
    public Transform goalTransform;

    public GameObject cubePrefab;
    public EnemyController enemyPrefab;
    public Transform gridHighlight;
    public MeshRenderer gridMeshRenderer;

    public Material allowedMat;
    public Material invalidMat;

    public int activeTowerIndex = -1;

    public int failCount = 0;

    private Grid grid;
    private EnemyController testPath;

    NavMeshPath path;
    RaycastHit hitInfo;

    public Dictionary<Vector3Int, TowerController> towers = new Dictionary<Vector3Int, TowerController>();

    private static TowerPlacer _towerPlacer;

    public static TowerPlacer towerPlacer
    {
        get
        {
            if (_towerPlacer == null)
                _towerPlacer = FindObjectOfType<TowerPlacer>();

            return _towerPlacer;
        }
    }

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();

        path = new NavMeshPath();

        gridMeshRenderer = gridHighlight.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hit = false;

        if (activeTowerIndex < 0 || activeTowerIndex >= TowerFactory.towerFactory.baseTowerControllers.Length)
        {
            gridHighlight.gameObject.SetActive(false);
        }
        else
        {
            gridHighlight.gameObject.SetActive(true);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hit = Physics.Raycast(ray, out hitInfo, 100f, (1 << 11));

            Vector3Int cell = GetCell(hitInfo.point);

            Vector3 pos = grid.GetCellCenterWorld(cell);
            pos.y = .25f;

            gridHighlight.position = pos;

            if (GameManager.money >= TowerFactory.towerFactory.baseTowerControllers[activeTowerIndex].tower.buildCost && !towers.ContainsKey(cell))
            {
                gridMeshRenderer.material = allowedMat;

                if (Input.GetMouseButtonDown(0) && (testPath == null || !testPath.gameObject.activeSelf))
                {

                    if (hit)
                    {
                        PlaceCubeNear(hitInfo.point, activeTowerIndex);
                    }
                }

                if (Input.GetMouseButtonDown(1) && (testPath == null || !testPath.gameObject.activeSelf))
                {
                    activeTowerIndex = -1;
                }
            }
            else
            {
                gridMeshRenderer.material = invalidMat;
            }
        }


    }

    private void PlaceCubeNear(Vector3 clickPoint, int index)
    {
        Vector3Int cell = GetCell(clickPoint);

        //Debug.Log("<" + colPos + ", " + rowPos + ", 0>");

        if (cell != new Vector3Int(-16, 15, 0) && cell != new Vector3Int(-16, 14, 0) && cell != new Vector3Int(-15, 15, 0) && cell != new Vector3Int(-15, 14, 0) &&
            cell != new Vector3Int(15, -16, 0) && cell != new Vector3Int(15, -15, 0) && cell != new Vector3Int(14, -16, 0) && cell != new Vector3Int(14, -15, 0) &&
            !towers.ContainsKey(cell))
        {
            Vector3 finalPosition = grid.GetCellCenterWorld(cell);
            finalPosition.y = 0.05f;

            TowerController tower = TowerFactory.towerFactory.BuildBaseTower(index, finalPosition, Quaternion.Euler(Vector3.zero));

            towers.Add(cell, tower);

            if (tower != null)
            {
                tower.transform.position = finalPosition;
                //cube.transform.localScale = Vector3.one * 0.5f;

                if (testPath == null)
                    testPath = Instantiate(enemyPrefab, spawnTransform.position, Quaternion.Euler(0, 0, 0));
                else
                {
                    testPath.transform.position = spawnTransform.position;
                    testPath.transform.rotation = Quaternion.Euler(0, 0, 0);
                    testPath.gameObject.SetActive(true);
                }

                SpawnController.spawnController.PathEnemiesToGoal();

                StartCoroutine(CheckIfBlockGoal(testPath, tower, cell, Time.deltaTime));
            }
        }
    }

    private Vector3Int GetCell(Vector3 position)
    {
        int colPos = Mathf.FloorToInt(position.x / grid.cellSize.x / 1.6f), rowPos = Mathf.FloorToInt(position.z / grid.cellSize.z / 1.6f);

        return new Vector3Int(colPos, rowPos, 0);
    }

    IEnumerator CheckIfBlockGoal(EnemyController testPath, TowerController tower, Vector3Int cell, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


        testPath.agent.CalculatePath(goalTransform.position, path);

        if (testPath.CheckPath(path))
        {
            GameManager.AddMoney(-tower.tower.buildCost);
        }
        else
        {
            Destroy(tower.gameObject);

            failCount++;

            GameManager.AddMoney(Mathf.Min(-10 * failCount, 0));

            towers.Remove(cell);

            Debug.Log("Would block goal");
        }

        testPath.gameObject.SetActive(false);

        //SpawnController.spawnController.PathEnemiesToGoal();
    }
}