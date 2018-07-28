using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtilities {

	public bool isNegative(float num){
		if (num < 0) {
			return true;
		}

		return false;
	}

	public bool isNegative(int num){
		if (num < 0) {
			return true;
		}

		return false;
	}

	public bool isNegative(double num){
		if (num < 0) {
			return true;
		}

		return false;
	}
}
