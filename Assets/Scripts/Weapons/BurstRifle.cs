using UnityEngine;

public class BurstRifle : Weapon {
	[SerializeField] protected LineRenderer _shotLine;
	[SerializeField] protected Animator _shotAnim;
	[SerializeField] protected AudioSource _shotSound;
	[Space]
	[SerializeField] protected int _roundsPerBurst = 3;
	[SerializeField] protected float _roundPeriodSec = .2f;
	[SerializeField] protected float _burstCooldownSec = .6f;
	[SerializeField] protected float _maxRange = 30f;
	[SerializeField] protected float _damagePerRound = 8f;

	protected float _lastShotTime = -1f;
	protected int _roundsRemaining = 0;

	protected override void StartFiring() {
		base.StartFiring();
		UpdateFiring();
	}

	protected override void StopFiring() {
		base.StopFiring();
	}

	protected override void UpdateFiring() {
		base.UpdateFiring();

		if (_roundsRemaining > 0) {
			if (Time.time - _lastShotTime >= _roundPeriodSec) {
				FireRound();
			}
		} else if (Time.time - _lastShotTime >= _burstCooldownSec) {
			StartNewBurst();
		}
	}

	protected override void UpdateIdle() {
		base.UpdateIdle();

		if (_roundsRemaining > 0 && Time.time - _lastShotTime >= _roundPeriodSec) {
			FireRound();
		}
	}

	protected virtual void StartNewBurst() {
		_roundsRemaining = _roundsPerBurst;
		FireRound();
	}

	protected virtual void FireRound() {
		Ray ray = new Ray(_firePoint.position, _firePoint.localToWorldMatrix * Vector3.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, _maxRange)) {
			_shotLine.SetPosition(1, hit.distance * _shotLine.transform.lossyScale.z * Vector3.forward);
			_shotAnim.SetTrigger("Fire");
			//Debug.DrawLine(ray.origin, hit.point, Color.green, _roundPeriodSec);

			Interactable target = hit.collider.GetComponentInParent<Interactable>();
            if (target) {
				target.OnShoot(_damagePerRound);
			}
		} else {
			_shotLine.SetPosition(1, _maxRange * Vector3.forward);
			_shotAnim.SetTrigger("Fire");
			//Debug.DrawLine(ray.origin, ray.origin + (ray.direction * _maxRange), Color.red, _roundPeriodSec);
		}

		_shotSound.Play();

		--_roundsRemaining;
		_lastShotTime = Time.time;
	}
}
