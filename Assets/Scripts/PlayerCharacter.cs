using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
	private static KeyCode leftKey => KeyCode.A;
	private static KeyCode rightKey => KeyCode.D;
	private static KeyCode backKey => KeyCode.S;
	private static KeyCode forwardKey => KeyCode.W;

	private float _forwardVelocity = .2f;
	private float _horizontalVelocity = 0f;

	private float _forwardAcceleration = .4f;

	private float _forwardVelocityMin = .2f;
	private float _forwardVelocityMax = 12f;
	private float _horizontalVelocityMax = 2f;

	public float forwardVelocity => _forwardVelocity;
	public float horizontalVelocity => _horizontalVelocity;

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
			if (_forwardVelocity < _forwardVelocityMax) {
				_forwardVelocity = Mathf.Max(_forwardVelocity + _forwardAcceleration * Time.deltaTime, _forwardVelocityMax);
			}

		}

	}
}
