using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHolderScript : MonoBehaviour
{
	// Bools
	private bool hasMoved = false;

	// Transforms
	private Transform downButton;
	private RectTransform rectTransform;	

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		downButton = transform.GetChild(1);
	}

	public void MoveUI()
	{
		Vector3 pos = rectTransform.localPosition;

		if (!hasMoved)
		{
			rectTransform.localPosition = new Vector3(pos.x, pos.y - 345, pos.z);
			hasMoved = true;
		}
		else
		{
			rectTransform.localPosition = new Vector3(pos.x, pos.y + 345, pos.z);
			hasMoved = false;
		}
		downButton.localEulerAngles += new Vector3(0, 0, 180f);
	}
}
