using UnityEngine;

public struct InputAxis {
		// The axis needs both a negative and positive key to denote what key creates what value in this axis
		private KeyCode positiveKey, negativeKey;

		// Sets the positive key of the axis
		public void setPositiveKey(KeyCode key) {
			// Assure that the positive and negative keys never are the same key
			if(this.negativeKey != key) {
				this.positiveKey = key;
			}
		}

		// Sets the negative key of the axis
		public void setNegativeKey(KeyCode key) {
			// Assure that the positive and negative keys never are the same key
			if(this.positiveKey != key) {
				this.negativeKey = key;
			}
		}

		// Returns a whole integer value between the two keys
		public float GetRawAxisInput() {
			if(Input.GetKey(positiveKey) && Input.GetKey(negativeKey)) {
				// If both keys are pressed then we return 0
				return 0f;
			} else if(Input.GetKey(positiveKey) && !Input.GetKey(negativeKey)) {
				// If only the positive key is pressed, then we return 1
				return 1f;
			} else if(Input.GetKey(negativeKey) && !Input.GetKey(positiveKey)) {
				// If only the negative key is pressed, then we return -1
				return -1f;
			}
			// If neither keys are pressed then we return 0;
			return 0f;
		}
	}