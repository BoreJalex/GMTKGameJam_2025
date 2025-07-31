using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridCell
{
    public Vector2 worldPosition;
    public bool isOccupied;
    public GameObject occupyingObject;

    public GridCell(Vector2 pos)
    {
        worldPosition = pos;
        isOccupied = false;
        occupyingObject = null;
    }
}
public class gridManagerScript : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 gridOrigin = Vector2.zero;

    [Header("Visual Settings")]
    [SerializeField] private bool showGrid = true;
    [SerializeField] private Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    [SerializeField] private Color occupiedColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private Color hoverColor = new Color(0f, 1f, 0f, 0.5f);

    [Header("Placement Settings")]
    [SerializeField] private GameObject placementPreview;
    [SerializeField] private SpriteRenderer previewRenderer;

    private GridCell[,] grid;
    private GridCell currentHoveredCell;
    private GameObject currentPreview;
    private Camera mainCamera;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public float CellSize => cellSize;
    public Vector2 GridOrigin => gridOrigin;

    public bool IsCellOccupied(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return grid[x, y].isOccupied;
        }
        return false;
    }
    void Start()
    {
        mainCamera = Camera.main;
        InitializeGrid();

        // Create a collider for mouse detection if needed
        CreateGridCollider();
    }

    void InitializeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 worldPos = GetWorldPosition(x, y);
                grid[x, y] = new GridCell(worldPos);
            }
        }
    }

    void CreateGridCollider()
    {
        // Add a box collider that covers the entire grid for mouse detection
        BoxCollider2D gridCollider = gameObject.AddComponent<BoxCollider2D>();
        gridCollider.size = new Vector2(gridWidth * cellSize, gridHeight * cellSize);
        gridCollider.offset = new Vector2((gridWidth * cellSize) / 2f,
                                         (gridHeight * cellSize) / 2f) + gridOrigin;
        gridCollider.isTrigger = true;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        // Add half cell size to get the center of the cell
        return new Vector2(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f) + gridOrigin;
    }

    public bool GetGridPosition(Vector2 worldPosition, out int x, out int y)
    {
        Vector2 gridPos = worldPosition - gridOrigin;
        x = Mathf.FloorToInt(gridPos.x / cellSize);
        y = Mathf.FloorToInt(gridPos.y / cellSize);

        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    public GridCell GetNearestCell(Vector2 worldPosition)
    {
        int x, y;
        if (GetGridPosition(worldPosition, out x, out y))
        {
            return grid[x, y];
        }
        return null;
    }

    public bool PlaceObject(GameObject obj, Vector2 worldPosition)
    {
        GridCell cell = GetNearestCell(worldPosition);

        if (cell != null && !cell.isOccupied)
        {
            obj.transform.position = cell.worldPosition;
            cell.isOccupied = true;
            cell.occupyingObject = obj;
            return true;
        }

        return false;
    }

    public bool RemoveObject(Vector2 worldPosition)
    {
        GridCell cell = GetNearestCell(worldPosition);

        if (cell != null && cell.isOccupied)
        {
            if (cell.occupyingObject != null)
            {
                Destroy(cell.occupyingObject);
            }
            cell.isOccupied = false;
            cell.occupyingObject = null;
            return true;
        }

        return false;
    }

    public void ClearGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].isOccupied && grid[x, y].occupyingObject != null)
                {
                    Destroy(grid[x, y].occupyingObject);
                }
                grid[x, y].isOccupied = false;
                grid[x, y].occupyingObject = null;
            }
        }
    }

    void Update()
    {
        HandleHoverPreview();
    }

    void HandleHoverPreview()
    {
        if (placementPreview == null) return;

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        GridCell cell = GetNearestCell(mouseWorldPos);

        if (cell != null)
        {
            currentHoveredCell = cell;

            if (currentPreview == null)
            {
                currentPreview = Instantiate(placementPreview);
                currentPreview.SetActive(false);

                // Make preview semi-transparent
                if (previewRenderer == null && currentPreview.TryGetComponent<SpriteRenderer>(out var sr))
                {
                    previewRenderer = sr;
                }
            }

            if (!cell.isOccupied)
            {
                currentPreview.SetActive(true);
                currentPreview.transform.position = cell.worldPosition;

                // Set preview transparency
                if (previewRenderer != null)
                {
                    Color c = previewRenderer.color;
                    c.a = 0.5f;
                    previewRenderer.color = c;
                }
            }
            else
            {
                currentPreview.SetActive(false);
            }
        }
        else
        {
            currentHoveredCell = null;
            if (currentPreview != null)
            {
                currentPreview.SetActive(false);
            }
        }
    }

    public List<GameObject> GetAllPlacedObjects()
    {
        List<GameObject> objects = new List<GameObject>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].isOccupied && grid[x, y].occupyingObject != null)
                {
                    objects.Add(grid[x, y].occupyingObject);
                }
            }
        }

        return objects;
    }

    void OnDrawGizmos()
    {
        if (!showGrid) return;

        // Draw grid lines
        Gizmos.color = gridColor;

        // Vertical lines
        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(x * cellSize + gridOrigin.x, gridOrigin.y, 0);
            Vector3 end = new Vector3(x * cellSize + gridOrigin.x, gridHeight * cellSize + gridOrigin.y, 0);
            Gizmos.DrawLine(start, end);
        }

        // Horizontal lines
        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = new Vector3(gridOrigin.x, y * cellSize + gridOrigin.y, 0);
            Vector3 end = new Vector3(gridWidth * cellSize + gridOrigin.x, y * cellSize + gridOrigin.y, 0);
            Gizmos.DrawLine(start, end);
        }

        // Draw cell states
        if (grid != null)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 cellCenter = grid[x, y].worldPosition;

                    if (grid[x, y].isOccupied)
                    {
                        Gizmos.color = occupiedColor;
                        Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize * 0.8f, cellSize * 0.8f, 0.1f));
                    }
                    else if (currentHoveredCell == grid[x, y])
                    {
                        Gizmos.color = hoverColor;
                        Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize * 0.9f, cellSize * 0.9f, 0.1f));
                    }
                }
            }
        }
    }
}