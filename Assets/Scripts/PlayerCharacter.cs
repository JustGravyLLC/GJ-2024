using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
	private static KeyCode leftKey => KeyCode.A;
	private static KeyCode rightKey => KeyCode.D;
	private static KeyCode backKey => KeyCode.S;
	private static KeyCode forwardKey => KeyCode.W;

	private float _forwardVelocity = 0f;
	private float _horizontalVelocity = 0f;

	private float _maxForwardVelocity = 8f;
	private float _maxHorizontalVelocity = 2f;

	private void Update() {
		float horizontalFactor = 0f;

		if (Input.GetKey(leftKey)) {
			horizontalFactor -= 1f;
		}
		if (Input.GetKey(rightKey)) {
			horizontalFactor += 1f;
		}


		if (Input.GetKey(backKey)) {
			//Brake
		} else {

		}
	}
}
