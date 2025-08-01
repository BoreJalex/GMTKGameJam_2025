using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemUI_Script : MonoBehaviour
{
    // Variables
    private int UISelected = 0;
    private Vector3 baseImageSize = new Vector3(2,2,2);

    // GameObjects
    private GameObject objectPlacer;

    // Scripts
    ObjectPlacerScript ObjectPlacerScript;

    // Images
    [SerializeField] private GameObject[] images;

	private void Start()
	{
        objectPlacer = GameObject.Find("ObjectPlacer");
		ObjectPlacerScript = objectPlacer.GetComponent<ObjectPlacerScript>();

		images[UISelected].transform.localScale *= 1.25f;
	}

	// Update is called once per frame
	void Update()
    {
        if (UISelected != ObjectPlacerScript.currentObjectIndex)
        {
			switch (UISelected)
			{
				case 0:
					images[UISelected].transform.localScale = baseImageSize;
					break;
				case 1:
					images[UISelected].transform.localScale = baseImageSize;
					break;
				case 2:
					images[UISelected].transform.localScale = baseImageSize;
					break;
				case 3:
					images[UISelected].transform.localScale = baseImageSize;
					break;
			}
			UISelected = ObjectPlacerScript.currentObjectIndex;
            switch (UISelected)
            {
                case 0:
                    images[UISelected].transform.localScale *= 1.25f;
                    break;
                case 1:
                    images[UISelected].transform.localScale *= 1.25f;
                    break;
                case 2:
                    images[UISelected].transform.localScale *= 1.25f;
                    break;
                case 3:
                    images[UISelected].transform.localScale *= 1.25f;
                    break;
            }
        }
    }
}
