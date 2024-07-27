using UnityEngine;

public abstract class Weapon : MonoBehaviour {
	protected KeyCode fireKey => PlayerCharacter.fireKey;

	[SerializeField]
	protected Transform _firePoint;

	protected virtual bool isFiring { get; private set; } = false;

	protected virtual void Update() {
		bool wasFiring = isFiring;

		if (Input.GetKeyDown(fireKey)) {
			isFiring = true;
		}
		if (Input.GetKeyUp(fireKey)) {
			isFiring = false;
		}

		if (isFiring) {
			if (wasFiring) {
				UpdateFiring();
			} else {
				StartFiring();
			}
		} else if (wasFiring) {
			StopFiring();
		} else {
			UpdateIdle();
		}
	}

	protected virtual void StartFiring() {

	}

	protected virtual void StopFiring() {

	}

	protected virtual void UpdateFiring() {

	}

	protected virtual void UpdateIdle() {

	}
}
