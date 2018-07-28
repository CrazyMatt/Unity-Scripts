// REQUIRES MathUtilities.cs TO EXIST IN UNITY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamControl : MonoBehaviour {

	public float panSpeedX = 1;
	public float panSpeedZ = 1;
	public float zoomSpeed = 1;

	public float minZoom = 10;
	public float maxZoom = 100;

	public bool isBound = false;
	public float xBoundLeft = -500;
	public float xBoundRight = 500;
	public float zBoundForward = 500;
	public float zBoundBackward = -500;

	public bool allowPlayerToCenterCamera = true;
	public Vector3 defaultCamCenter = new Vector3 (0, 60, -60);

	public enum Direction {
		x,
		y,
		z
	};

	public Vector3 panCamInDirection(Direction direction, float amount, bool obeyBounding = true){
		MathUtilities mu = new MathUtilities ();

		Vector3 initialLocation = gameObject.transform.position;
		Vector3 movementToAdd = new Vector3(0f,0f,0f);

		if (obeyBounding && isBound) {
			switch (direction) {
				case Direction.x:
					if (((initialLocation.x <= xBoundLeft) && mu.isNegative(amount)) || ((initialLocation.x >= xBoundRight) && !mu.isNegative(amount))) {
						gameObject.transform.position = initialLocation;
						return initialLocation;
					}
					break;
				case Direction.z:
					if (((initialLocation.z <= zBoundBackward) && mu.isNegative(amount)) || ((initialLocation.z >= zBoundForward) && !mu.isNegative(amount))) {
						gameObject.transform.position = initialLocation;
						return initialLocation;
					}
					break;
			}
		}

		switch (direction) {
			case Direction.x:
				movementToAdd = new Vector3 (amount, 0f, 0f);
				break;
			case Direction.y:
				movementToAdd = new Vector3 (0f, amount, 0f);
				break;
			case Direction.z:
				movementToAdd = new Vector3 (0f, 0f, amount);
				break;
		}

		Vector3 newLocation = initialLocation + movementToAdd;
		gameObject.transform.position = newLocation;
		return newLocation;
	}
		
	/* Return values:
	 * bool[0]: wasPlaced?
	 * bool[1]: wasInBounds? */
	public bool[] putCamAt(Vector3 location, bool obeyBounding = false){
		bool inBounds;
		bool placed;
		bool[] outputValues = new bool[2];

		if (((location.x >= xBoundLeft) &&
		    (location.x <= xBoundRight)) &&
		    ((location.z >= zBoundBackward) &&
		    (location.z <= zBoundForward))) {

			inBounds = true;
		} else {
			inBounds = false;
		}

		if(obeyBounding && !inBounds) {
			placed = false;
			outputValues = new bool[2] {placed, inBounds};
			return outputValues;
		}

		gameObject.transform.position = location;
		placed = true;
		outputValues = new bool[2] {placed, inBounds};
		return outputValues;
	}

	public float zoomInCam(float amount){

		float initialZoom = gameObject.GetComponent<Camera> ().fieldOfView;
		float newZoom = initialZoom;

		if (amount > 0) { // Zooming In
			if(!(initialZoom < minZoom)){
				newZoom = initialZoom - amount;
			}
		} else { // Zooming Out
			if(!(initialZoom > maxZoom)){
				newZoom = initialZoom - amount;
			}
		}

		gameObject.GetComponent<Camera> ().fieldOfView = newZoom;
		return newZoom;
	}

	public bool setZoomToPercent(float percent, bool obeyBounding = false){
		if(obeyBounding && !((percent >= 0) && (percent <= 100))){
			return false;
		}

		float zoomRange = maxZoom - minZoom;
		float newZoom = ((percent / 100) * zoomRange) + minZoom;

		gameObject.GetComponent<Camera> ().fieldOfView = newZoom;
		return true;
	}

	public bool setZoom(int povZoomAmount, bool obeyBounding = false){
		if(obeyBounding && !((povZoomAmount < Mathf.CeilToInt(minZoom)) && (povZoomAmount > Mathf.FloorToInt(maxZoom)))){
			return false;
		}

		gameObject.GetComponent<Camera> ().fieldOfView = povZoomAmount;
		return true;
	}

	public float getZoom(){
		return gameObject.GetComponent<Camera> ().fieldOfView;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// WASD Movement keys
		if(Input.GetKey(KeyCode.A)){
			panCamInDirection (Direction.x, -panSpeedX);
		}
		if(Input.GetKey(KeyCode.D)){
			panCamInDirection (Direction.x, panSpeedX);
		}

		if(Input.GetKey(KeyCode.W)){
			panCamInDirection (Direction.z, panSpeedZ);
		}
		if(Input.GetKey(KeyCode.S)){
			panCamInDirection (Direction.z, -panSpeedZ);
		}


		// RF Zoom keys
		if(Input.GetKey(KeyCode.R)){
			zoomInCam (zoomSpeed);
		}
		if(Input.GetKey(KeyCode.F)){
			zoomInCam (-zoomSpeed);
		}

		// Center
		if (Input.GetKey (KeyCode.C)) {
			if (allowPlayerToCenterCamera) {
				putCamAt (defaultCamCenter);
			}
		}

	}
}
