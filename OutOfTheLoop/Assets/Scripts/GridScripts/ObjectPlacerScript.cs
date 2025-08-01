using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacerScript : MonoBehaviour
{
    [Header("Placement Objects")]
    [SerializeField] public GameObject[] placeableObjects;
    [SerializeField] public int currentObjectIndex = -1;

    [Header("References")]
    [SerializeField] private gridManagerScript gridSystem;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (gridSystem == null)
        {
            gridSystem = FindObjectOfType<gridManagerScript>();
        }
    }

    void Update()
    {
        HandleObjectSelection();
        HandleObjectPlacement();
    }

    void HandleObjectSelection()
    {
        // Number keys to select objects
        for (int i = 0; i < placeableObjects.Length && i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (currentObjectIndex == i)
                {
                    currentObjectIndex = -1; // Deselect if already selected
                }
                else
                    currentObjectIndex = i;
                Debug.Log($"Selected object: {placeableObjects[i].name}");
            }
        }

		// Mouse wheel to cycle through objects
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0)
		{
			currentObjectIndex += scroll > 0 ? -1 : 1; 
			currentObjectIndex = Mathf.Clamp(currentObjectIndex, 0, placeableObjects.Length - 1);
			Debug.Log($"Selected object: {placeableObjects[currentObjectIndex].name}");
		}
	}

	void HandleObjectPlacement()
    {
        if (Input.GetMouseButtonDown(0) && currentObjectIndex != -1) // Left click to place
        {
            PlaceObject();
        }
        else if (Input.GetMouseButtonDown(1) && currentObjectIndex != -1) // Right click to remove
        {
            RemoveObject();
        }
    }

    void PlaceObject()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (currentObjectIndex < placeableObjects.Length)
        {
            GameObject newObject = Instantiate(placeableObjects[currentObjectIndex]);

            if (!gridSystem.PlaceObject(newObject, mouseWorldPos))
            {
                Destroy(newObject);
                Debug.Log("Cannot place object here - cell is occupied!");
            }
        }
    }

    void RemoveObject()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        gridSystem.RemoveObject(mouseWorldPos);
    }

    public void setItem1()
    {
        if (currentObjectIndex == 0)
        {
            currentObjectIndex = -1; // Deselect if already selected
        }
        else
            currentObjectIndex = 0;
    }

    public void setItem2()
    {
        if (currentObjectIndex == 1)
        {
            currentObjectIndex = -1; // Deselect if already selected
        }
        else
            currentObjectIndex = 1;
    }

    public void setItem3()
    {
        if (currentObjectIndex == 2)
        {
            currentObjectIndex = -1; // Deselect if already selected
        }
        else
            currentObjectIndex = 2;
    }

    public void setItem4()
    {
        if (currentObjectIndex == 3)
        {
            currentObjectIndex = -1; // Deselect if already selected
        }
        else
            currentObjectIndex = 3;
    }
}
