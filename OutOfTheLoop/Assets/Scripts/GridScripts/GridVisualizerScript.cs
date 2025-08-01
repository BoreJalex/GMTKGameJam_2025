using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizerScript : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private bool showGridInGame = true;
    [SerializeField] private Color gridLineColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private int sortingOrder = -10;

    private gridManagerScript gridSystem;
    private GameObject gridLinesContainer;
    private List<LineRenderer> gridLines = new List<LineRenderer>();
    private Dictionary<Vector2Int, GameObject> cellIndicators = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        gridSystem = GetComponent<gridManagerScript>();
        if (showGridInGame)
        {
            CreateGridVisuals();
        }
    }

    void CreateGridVisuals()
    {
        // Create container for organization
        gridLinesContainer = new GameObject("Grid Lines");
        gridLinesContainer.transform.parent = transform;

        // Get grid dimensions from GridSystem2D (you'll need to add public getters)
        int width = gridSystem.GridWidth;
        int height = gridSystem.GridHeight;
        float cellSize = gridSystem.CellSize;
        Vector2 origin = gridSystem.GridOrigin;

        // Create vertical lines
        for (int x = 0; x <= width; x++)
        {
            GameObject lineObj = new GameObject($"Vertical Line {x}");
            lineObj.transform.parent = gridLinesContainer.transform;

            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            ConfigureLineRenderer(line);

            Vector3 start = new Vector3(x * cellSize + origin.x, origin.y, 0);
            Vector3 end = new Vector3(x * cellSize + origin.x, height * cellSize + origin.y, 0);

            line.SetPosition(0, start);
            line.SetPosition(1, end);

            gridLines.Add(line);
        }

        // Create horizontal lines
        for (int y = 0; y <= height; y++)
        {
            GameObject lineObj = new GameObject($"Horizontal Line {y}");
            lineObj.transform.parent = gridLinesContainer.transform;

            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            ConfigureLineRenderer(line);

            Vector3 start = new Vector3(origin.x, y * cellSize + origin.y, 0);
            Vector3 end = new Vector3(width * cellSize + origin.x, y * cellSize + origin.y, 0);

            line.SetPosition(0, start);
            line.SetPosition(1, end);

            gridLines.Add(line);
        }        
    }

    void ConfigureLineRenderer(LineRenderer line)
    {
        line.positionCount = 2;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = gridLineColor;
        line.endColor = gridLineColor;
        line.sortingOrder = sortingOrder;
        line.useWorldSpace = true;
    }

  


    void Update()
    {

        // Update cell indicators based on occupancy
        for (int x = 0; x < gridSystem.GridWidth; x++)
        {
            for (int y = 0; y < gridSystem.GridHeight; y++)
            {
                Vector2Int key = new Vector2Int(x, y);
                if (cellIndicators.ContainsKey(key))
                {
                    bool isOccupied = gridSystem.IsCellOccupied(x, y);
                    cellIndicators[key].SetActive(isOccupied);
                }
            }
        }
    }

  

}
