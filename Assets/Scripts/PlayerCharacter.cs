using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
	private static KeyCode leftKey => KeyCode.A;
	private static KeyCode rightKey => KeyCode.D;
	private static KeyCode backKey => KeyCode.S;
	private static KeyCode forwardKey => KeyCode.W;

	private static float _minX = -20f;
	private static float _maxX = 20f;

	private float _forwardVelocity = 2f;
	private float _horizontalVelocity = 0f;

	private float _forwardAcceleration = 2f;
	private float _brakeDecelerationBase = 0f;
	private float _brakeDecelerationFull = 8f;
	private float _horizontalAcceleration = 12f;
	private float _horizontalDeceleration = 8f;

	private float _forwardVelocityMin = .2f;
	private float _forwardVelocityMax = 20f;
	private float _horizontalVelocityMax = 8f;

	public float forwardVelocity => _forwardVelocity;
	public float horizontalVelocity => _horizontalVelocity;

	private void Update() {
		float sideInputFactor = 0f;

		if (Input.GetKey(leftKey)) {
			sideInputFactor -= 1f;
		}
		if (Input.GetKey(rightKey)) {
			sideInputFactor += 1f;
		}

		//Side acceleration
		float horizontalVelocityGoal = sideInputFactor * _horizontalVelocityMax;
		if (_horizontalVelocity > 0f) {
			if (horizontalVelocityGoal < 0f) {
				_horizontalVelocity = Mathf.Max(horizontalVelocityGoal, _horizontalVelocity + ((sideInputFactor * _horizontalAcceleration) - _horizontalDeceleration) * Time.deltaTime);
			} else if (horizontalVelocityGoal < _horizontalVelocity) {
				_horizontalVelocity = Mathf.Max(horizontalVelocityGoal, _horizontalVelocity - _horizontalDeceleration * Time.deltaTime);
			} else {
				_horizontalVelocity = Mathf.Min(horizontalVelocityGoal, _horizontalVelocity + sideInputFactor * _horizontalAcceleration * Time.deltaTime);
			}
		} else if (_horizontalVelocity < 0f) {
			if (horizontalVelocityGoal > 0f) {
				_horizontalVelocity = Mathf.Min(horizontalVelocityGoal, _horizontalVelocity + ((sideInputFactor * _horizontalAcceleration) + _horizontalDeceleration) * Time.deltaTime);
			} else if (horizontalVelocityGoal > _horizontalVelocity) {
				_horizontalVelocity = Mathf.Min(horizontalVelocityGoal, _horizontalVelocity + _horizontalDeceleration * Time.deltaTime);
			} else {
				_horizontalVelocity = Mathf.Max(horizontalVelocityGoal, _horizontalVelocity + sideInputFactor * _horizontalAcceleration * Time.deltaTime);
			}
		} else if (sideInputFactor != 0f) {
			_horizontalVelocity += sideInputFactor * _horizontalAcceleration * Time.deltaTime;
		}

		//Brake
		if (Input.GetKey(backKey)) {
			if (_forwardVelocity > _forwardVelocityMin) {
				_forwardVelocity = Mathf.Max(_forwardVelocity - Mathf.Lerp(_brakeDecelerationBase, _brakeDecelerationFull, Mathf.Abs(sideInputFactor)) * Time.deltaTime, _forwardVelocityMin);
			}
		} else {
			if (_forwardVelocity < _forwardVelocityMax) {
				_forwardVelocity = Mathf.Min(_forwardVelocity + _forwardAcceleration * Time.deltaTime, _forwardVelocityMax);
			}
		}

		//Side motion
		if (_horizontalVelocity != 0f) {
			Vector3 pos = transform.localPosition;
			pos.x = Mathf.Clamp(pos.x + _horizontalVelocity * Time.deltaTime, _minX, _maxX);
			transform.localPosition = pos;
		}
	}
}
