using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectPlacerScript : MonoBehaviour
{
    [Header("Placement Objects")]
    [SerializeField] public GameObject[] placeableObjects;
    [SerializeField] public int currentObjectIndex = -1;

    [Header("References")]
    [SerializeField] private gridManagerScript gridSystem;

    [Header("Mouse Tracking")]
    [SerializeField] private GameObject mouseTracker;

    [Header("Variables")]
    [SerializeField] public int attractorAmount;
    [SerializeField] public int repulsorAmount;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI attractorText;
	[SerializeField] private TextMeshProUGUI repulsorText;

	private Camera mainCamera;

    void Start()
    {
        UpdateText();

		mainCamera = Camera.main;

        if (gridSystem == null)
        {
            gridSystem = FindObjectOfType<gridManagerScript>();
        }
    }

    void Update()
    {
        //HandleObjectSelection();
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
        if (Input.GetMouseButtonDown(0)) // Left click to place attractor
        {
            currentObjectIndex = 0;
            PlaceObject();
        }
        else if (Input.GetMouseButtonDown(1)) // Right click to place repulser
        {
            currentObjectIndex = 1;
			PlaceObject();
		}
	}

	void PlaceObject()
	{
		StartCoroutine(PlaceObjectCoroutine());
	}

    // Made into a coroutine to allow for time to create and remove the collision check for hazards
	IEnumerator PlaceObjectCoroutine()
	{
		Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

		if (currentObjectIndex < placeableObjects.Length)
		{
			Vector2 nearest = gridSystem.GetNearestCellMiddle(mouseWorldPos);
			GameObject colCheck = Instantiate(mouseTracker, nearest, Quaternion.identity);
			MouseTrackScript mouseTrackScript = colCheck.GetComponent<MouseTrackScript>();

			yield return new WaitForFixedUpdate(); 
			yield return new WaitForSeconds(0.05f); 

			if (!mouseTrackScript.isColliding)
			{
				gridSystem.PlaceObject(mouseWorldPos);

				if (currentObjectIndex == 0)
                    attractorAmount--;
                else if (currentObjectIndex == 1)
                    repulsorAmount--;
			}

			Destroy(colCheck);
			UpdateText();
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

    void UpdateText()
    {
		attractorText.text = attractorAmount.ToString();
		repulsorText.text = repulsorAmount.ToString();
	}
}
