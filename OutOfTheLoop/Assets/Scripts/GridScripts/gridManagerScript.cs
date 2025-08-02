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
    [SerializeField] private Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    [SerializeField] private Color occupiedColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private Color hoverColor = new Color(0f, 1f, 0f, 0.5f);

    [Header("Placement Settings")]
    [SerializeField] private GameObject placementPreview;
    [SerializeField] private SpriteRenderer previewRenderer;
    private ObjectPlacerScript objectPlacerScript;


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
        objectPlacerScript = GameObject.FindGameObjectWithTag("ObjectPlacer").GetComponent<ObjectPlacerScript>();
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

    // The method for returning cell's middlepoints
	public Vector2 GetNearestCellMiddle(Vector2 worldPosition)
	{
		int x, y;
		if (GetGridPosition(worldPosition, out x, out y))
		{
			return grid[x, y].worldPosition;
		}
		return Vector2.zero;
	}

	// The method for placing all satellites
	public void PlaceObject(Vector2 worldPosition)
	{
		GridCell cell = GetNearestCell(worldPosition);

		if (objectPlacerScript.currentObjectIndex == 0 && objectPlacerScript.attractorAmount <= 0)
		{
            if (cell.isOccupied && cell.occupyingObject.CompareTag("Attractor"))
            {
                objectPlacerScript.attractorAmount++;
                RemoveObject(worldPosition);
            }
            else
                objectPlacerScript.attractorAmount++;
			return;
		}
		else if (objectPlacerScript.currentObjectIndex == 1 && objectPlacerScript.repulsorAmount <= 0)
		{
            if (cell.isOccupied && cell.occupyingObject.CompareTag("Repulser"))
            {
				objectPlacerScript.repulsorAmount++;
				RemoveObject(worldPosition);
            }
            else
                objectPlacerScript.repulsorAmount++;
			return;
		}

		if (cell == null)
			return;

		// If cell not occupied, place new thing
		if (!cell.isOccupied)
		{
			GameObject obj = Instantiate(objectPlacerScript.placeableObjects[objectPlacerScript.currentObjectIndex]);
			obj.transform.position = cell.worldPosition;
			cell.isOccupied = true;
			cell.occupyingObject = obj;
			return;
		}

		// If the cell is occupied
		if (cell.isOccupied)
		{
			// Remove if same 
			if (objectPlacerScript.placeableObjects[objectPlacerScript.currentObjectIndex].CompareTag(cell.occupyingObject.tag))
			{
				if (cell.occupyingObject.CompareTag("Attractor"))
					objectPlacerScript.attractorAmount++;
				else if (cell.occupyingObject.CompareTag("Repulser"))
					objectPlacerScript.repulsorAmount++;
				RemoveObject(worldPosition);
				return;
			}

			// If opposite, replace
			RemoveObject(worldPosition);
			PlaceObject(worldPosition); 
			return;
		}
	}

    public bool RemoveObject(Vector2 worldPosition)
    {
        GridCell cell = GetNearestCell(worldPosition);

        if (cell != null && cell.isOccupied)
        {
            if (cell.occupyingObject.CompareTag("Attractor"))
                objectPlacerScript.attractorAmount++;
            else if (cell.occupyingObject.CompareTag("Repulser"))
                objectPlacerScript.repulsorAmount++;

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

    void Update()
    {
        //HandleHoverPreview();
    }

    // Add this variable to track the last preview prefab
    private GameObject lastPlacementPreview;

    void HandleHoverPreview()
    {
        // Sets preview to be the active item being placed
        if (objectPlacerScript.currentObjectIndex >= 0)
            placementPreview = objectPlacerScript.placeableObjects[objectPlacerScript.currentObjectIndex];
        else
        {
            placementPreview = null;
            Destroy(currentPreview);
            currentPreview = null;
            previewRenderer = null;
        }

        if (placementPreview == null) return;

        // Check if the preview object has changed
        if (lastPlacementPreview != placementPreview)
        {
            // Destroy the old preview
            if (currentPreview != null)
            {
                Destroy(currentPreview);
                currentPreview = null;
                previewRenderer = null;
            }

            // Update the reference
            lastPlacementPreview = placementPreview;
        }

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
                if (currentPreview.TryGetComponent<SpriteRenderer>(out var sr))
                {
                    previewRenderer = sr;
                }

                // Disable any scripts and physics on the preview
                DisablePreviewComponents(currentPreview);
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

    // Helper method to disable components on the preview
    void DisablePreviewComponents(GameObject preview)
    {
        // Disable all MonoBehaviour scripts except SpriteRenderer
        MonoBehaviour[] scripts = preview.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }

        // Disable physics
        Rigidbody2D rb = preview.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }

        // Disable colliders
        Collider2D[] colliders = preview.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        // Also disable components in children
        foreach (Transform child in preview.transform)
        {
            DisablePreviewComponents(child.gameObject);
        }
    }



}