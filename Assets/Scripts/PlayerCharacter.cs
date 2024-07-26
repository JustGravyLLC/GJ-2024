using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
	private const float ANIM_DAMP_TIME_X = .2f;
	private const float ANIM_DAMP_TIME_Y = .2f;

	private static KeyCode leftKey => KeyCode.A;
	private static KeyCode rightKey => KeyCode.D;
	private static KeyCode backKey => KeyCode.S;
	private static KeyCode forwardKey => KeyCode.W;

	private static float _minX = -20f;
	private static float _maxX = 20f;

	private float _forwardVelocity = 2f;
	private float _horizontalVelocity = 0f;

	private float _forwardAcceleration = 4f;
	private float _brakeDecelerationBase = 8f;
	private float _brakeDecelerationFull = 10f;
	private float _horizontalAcceleration = 12f;
	private float _horizontalDeceleration = 8f;

	private float _forwardVelocityMin = .2f;
	private float _forwardVelocityMax = 24f;
	private float _horizontalVelocityMax = 8f;

	[SerializeField]
	private Animator _anim;
	[SerializeField]
	private AudioSource _moveSound;
	[SerializeField]
	private AudioSource _brakeSound;

	public float forwardVelocity => _forwardVelocity;
	public float horizontalVelocity => _horizontalVelocity;

	private bool _movementEnabled = false;
	public bool movementEnabled {
		get { return _movementEnabled; }
		set {
			if (_movementEnabled == value)
				return;

			_movementEnabled = value;
			_moveSound.volume = 0f;
			if (value) {
				_moveSound.Play();
			} else {
				_moveSound.Stop();
				_brakeSound.Stop();
			}
		}
	}

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
				if (!_brakeSound.isPlaying) {
					_brakeSound.Play();
				}
			}
			_anim.SetFloat("MoveY", -1f, ANIM_DAMP_TIME_Y, Time.deltaTime);
		} else {
			if (_forwardVelocity < _forwardVelocityMax) {
				_forwardVelocity = Mathf.Min(_forwardVelocity + _forwardAcceleration * Time.deltaTime, _forwardVelocityMax);
			}
			_anim.SetFloat("MoveY", 0f, ANIM_DAMP_TIME_Y, Time.deltaTime);
		}

		//Side motion
		if (_horizontalVelocity != 0f) {
			Vector3 pos = transform.localPosition;
			pos.x = Mathf.Clamp(pos.x + _horizontalVelocity * Time.deltaTime, _minX, _maxX);
			transform.localPosition = pos;
		}

		//Animation
		_anim.SetFloat("MoveX", sideInputFactor, ANIM_DAMP_TIME_X, Time.deltaTime);

		//Sound
		_moveSound.volume = Mathf.InverseLerp(_forwardVelocityMin, _forwardVelocityMax, _forwardVelocity);
	}
}
