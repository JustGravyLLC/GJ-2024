using UnityEngine;

public class BurstRifle : Weapon {
	[SerializeField] protected int _roundsPerBurst = 3;
	[SerializeField] protected float _roundPeriodSec = .2f;
	[SerializeField] protected float _burstCooldownSec = .6f;

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
		//TODO: Do the actual firing
		Debug.Log($"Fire {_roundsPerBurst - _roundsRemaining}");

		--_roundsRemaining;
		_lastShotTime = Time.time;
	}
}
