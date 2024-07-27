using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
	private const float ANIM_DAMP_TIME_X = .2f;
	private const float ANIM_DAMP_TIME_Y = .2f;

	private const float MOVE_VOLUME_MIN = 0f;
	private const float MOVE_VOLUME_MAX = .5f;

	private const float BRAKE_VOLUME_MIN = .5f;
	private const float BRAKE_VOLUME_MAX = 1f;

	private const float MOVE_PARTICLE_RATE_MAX = 300f;
	private const float BRAKE_PARTICLE_RATE_MAX = 300f;

	public static KeyCode leftKey => KeyCode.A;
	public static KeyCode rightKey => KeyCode.D;
	public static KeyCode backKey => KeyCode.S;
	public static KeyCode forwardKey => KeyCode.W;
	public static KeyCode fireKey => KeyCode.Mouse0;

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
	[SerializeField]
	private List<ParticleSystem> _moveParticles;
	[SerializeField]
	private List<ParticleSystem> _brakeParticles;

	public float forwardVelocity => _forwardVelocity;
	public float horizontalVelocity => _horizontalVelocity;

	private bool _movementEnabled = true;
	public bool movementEnabled {
		get { return _movementEnabled; }
		set {
			if (_movementEnabled == value)
				return;

			_movementEnabled = value;
			_moveSound.volume = 0f;
			if (value) {
				_moveSound.Play();
				foreach (ParticleSystem emitter in _moveParticles) {
					emitter.Play();
				}
				foreach (ParticleSystem emitter in _brakeParticles) {
					emitter.Play();
				}
			} else {
				_moveSound.Stop();
				_brakeSound.Stop();
				foreach (ParticleSystem emitter in _moveParticles) {
					emitter.Pause();
				}
				foreach (ParticleSystem emitter in _brakeParticles) {
					emitter.Pause();
				}
			}
		}
	}

	private void Awake() {
		movementEnabled = false;
	}

	private void Update() {
		if (movementEnabled) {
			UpdateMovement();
		}
	}

	private void UpdateMovement() {
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
		if (Input.GetKeyUp(backKey)) {
			_brakeSound.Stop();
		}
		if (Input.GetKeyDown(backKey)) {
			_brakeSound.Play();
		}

		if (Input.GetKey(backKey)) {
			if (_forwardVelocity > _forwardVelocityMin) {
				float oldVelocity = _forwardVelocity;
				_forwardVelocity = Mathf.Max(_forwardVelocity - Mathf.Lerp(_brakeDecelerationBase, _brakeDecelerationFull, Mathf.Abs(sideInputFactor)) * Time.deltaTime, _forwardVelocityMin);
				_brakeSound.volume = Mathf.Lerp(BRAKE_VOLUME_MIN, BRAKE_VOLUME_MAX, (oldVelocity - _forwardVelocity) / _brakeDecelerationFull);

				foreach (ParticleSystem emitter in _brakeParticles) {
					var emissionModule = emitter.emission;
					emissionModule.rateOverTime = BRAKE_PARTICLE_RATE_MAX * Mathf.InverseLerp(_forwardVelocityMin, _forwardVelocityMax, forwardVelocity);
				}
			} else {
				foreach (ParticleSystem emitter in _brakeParticles) {
					var emissionModule = emitter.emission;
					emissionModule.rateOverTime = 0f;
				}
				_brakeSound.volume = BRAKE_VOLUME_MIN;
			}
			_anim.SetFloat("MoveY", -1f, ANIM_DAMP_TIME_Y, Time.deltaTime);
		} else {
			if (_forwardVelocity < _forwardVelocityMax) {
				_forwardVelocity = Mathf.Min(_forwardVelocity + _forwardAcceleration * Time.deltaTime, _forwardVelocityMax);
			}
			foreach (ParticleSystem emitter in _brakeParticles) {
				var emissionModule = emitter.emission;
				emissionModule.rateOverTime = 0f;
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
		foreach (ParticleSystem emitter in _moveParticles) {
			var emissionModule = emitter.emission;
			emissionModule.rateOverTime = MOVE_PARTICLE_RATE_MAX * Mathf.InverseLerp(_forwardVelocityMin, _forwardVelocityMax, forwardVelocity);

			var velocityModule = emitter.velocityOverLifetime;
			velocityModule.z = -forwardVelocity;
		}

		//Sound
		_moveSound.volume = Mathf.Lerp(MOVE_VOLUME_MIN, MOVE_VOLUME_MAX, Mathf.InverseLerp(_forwardVelocityMin, _forwardVelocityMax, _forwardVelocity));
	}

	public void HitSlowdown(float divisor = 2f)
    {
		_forwardVelocity = forwardVelocity / divisor;
    }
}
