using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexplacer : MonoBehaviour {

	public float boundWidth = 100;
	public float boundHeight = 100;

	public GameObject reproductionObject;
	public float hexagonRadius = 20;
	public bool allowOverflow = true;

	public int hexGridWidth = 10;
	public int hexGridHeight = 5;

	// Use this for initialization
	void Start () {
		if ((boundWidth < 2) ||
		    (boundHeight < 2) ||
		    (hexagonRadius < 1)) {
			Debug.LogWarning ("boundWidth, boundHeight, or hexagonRadius too low.");
			return;
		}

		bool yInBound = true;
		bool offsetRow = false;

		float currentY = hexagonRadius;
		for(int yIndex = 0; yIndex < hexGridHeight; yIndex++) {
			int allowedRepetitionsInThisRow = hexGridWidth; // TO CHANGE

			for (int i = 0; i < allowedRepetitionsInThisRow; i++) {
				float offset = 0;

				if (offsetRow) {
					offset = hexagonRadius * 0.75f;
				}

				GameObject newObject = GameObject.Instantiate (reproductionObject);
				newObject.transform.parent = gameObject.transform;
				newObject.GetComponent<RectTransform>().transform.position = new Vector3 ((i * (hexagonRadius + hexagonRadius/2)) + offset, 0, currentY);
				newObject.GetComponent<RectTransform>().rotation = new Quaternion (0, 0, 0, 0);
				newObject.name = "x" + i + "y" + yIndex;
			}

			currentY = currentY + hexagonRadius/2;
			offsetRow = !offsetRow;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
