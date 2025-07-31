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

    [Header("Cell Indicators")]
    [SerializeField] private bool showCellStates = true;
    [SerializeField] private Color occupiedCellColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private GameObject cellIndicatorPrefab; // Optional: custom cell indicator

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

        // Create cell indicators if needed
        if (showCellStates)
        {
            CreateCellIndicators();
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

    void CreateCellIndicators()
    {
        int width = gridSystem.GridWidth;
        int height = gridSystem.GridHeight;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateCellIndicator(x, y);
            }
        }
    }

    void CreateCellIndicator(int x, int y)
    {
        GameObject indicator;

        if (cellIndicatorPrefab != null)
        {
            indicator = Instantiate(cellIndicatorPrefab, transform);
        }
        else
        {
            // Create default indicator
            indicator = new GameObject($"Cell Indicator [{x},{y}]");
            indicator.transform.parent = transform;

            SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
            sr.sprite = CreateSquareSprite();
            sr.color = new Color(1, 1, 1, 0);
            sr.sortingOrder = sortingOrder + 1;
        }

        indicator.transform.position = gridSystem.GetWorldPosition(x, y);
        indicator.transform.localScale = Vector3.one * gridSystem.CellSize * 0.9f;
        indicator.SetActive(false);

        cellIndicators[new Vector2Int(x, y)] = indicator;
    }

    void Update()
    {
        if (!showCellStates) return;

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

                    if (isOccupied && cellIndicators[key].TryGetComponent<SpriteRenderer>(out var sr))
                    {
                        sr.color = occupiedCellColor;
                    }
                }
            }
        }
    }

    Sprite CreateSquareSprite()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
    }

    public void ToggleGrid()
    {
        showGridInGame = !showGridInGame;
        gridLinesContainer?.SetActive(showGridInGame);
    }

    public void SetGridColor(Color newColor)
    {
        gridLineColor = newColor;
        foreach (var line in gridLines)
        {
            line.startColor = gridLineColor;
            line.endColor = gridLineColor;
        }
    }
}
