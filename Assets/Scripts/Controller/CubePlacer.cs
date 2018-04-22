using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubePlacer : MonoBehaviour {

    public Transform spawnTransform;
    public Transform goalTransform;

    public GameObject cubePrefab;
    public EnemyController enemyPrefab;

    private Grid grid;
    private EnemyController testPath;

    NavMeshPath path;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();

        path = new NavMeshPath();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && (testPath == null || !testPath.gameObject.activeSelf))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                PlaceCubeNear(hitInfo.point);
            }
        }
    }

    private void PlaceCubeNear(Vector3 clickPoint)
    {
        int colPos = Mathf.FloorToInt(clickPoint.x / grid.cellSize.x / 1.6f), rowPos = Mathf.FloorToInt(clickPoint.z / grid.cellSize.z / 1.6f);

        Vector3Int cell = new Vector3Int(colPos, rowPos, 0);

        //Debug.Log("<" + colPos + ", " + rowPos + ", 0>");

        if (cell != new Vector3Int(-16, 15, 0) && cell != new Vector3Int(-16, 14, 0) && cell != new Vector3Int(-15, 15, 0) && cell != new Vector3Int(-15, 14, 0) &&
            cell != new Vector3Int(15, -16, 0) && cell != new Vector3Int(15, -15, 0) && cell != new Vector3Int(14, -16, 0) && cell != new Vector3Int(14, -15, 0))
        {
            Vector3 finalPosition = grid.GetCellCenterWorld(cell);

            GameObject cube = Instantiate(cubePrefab, finalPosition, Quaternion.Euler(Vector3.zero));
            cube.transform.position = finalPosition;
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

            StartCoroutine(CheckIfBlockGoal(testPath, cube, Time.deltaTime));
        }
    }

    IEnumerator CheckIfBlockGoal(EnemyController testPath, GameObject cube, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


        testPath.agent.CalculatePath(goalTransform.position, path);

        if (testPath.CheckPath(path))
        {

        }
        else
        {
            Destroy(cube);
            Debug.Log("Would block goal");
        }

        testPath.gameObject.SetActive(false);

        //SpawnController.spawnController.PathEnemiesToGoal();
    }
}