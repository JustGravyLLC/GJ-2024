using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected GameController _gameController;
    private Spawner _spawner;
    public GameObject renderRoot;
    public ColliderObject colliderRoot;
    public GameObject deathRoot;

    public void Initialize(Spawner s, GameController gc)
    {
        _gameController = gc;
        _spawner = s;
        renderRoot.SetActive(true);
        colliderRoot.gameObject.SetActive(true);
        colliderRoot.Initialize(this);
    }

    public virtual void OnInteract()
    {
        _gameController.playerCharacter.HitSlowdown();
        Despawn();
        Die();
    }

    public virtual void OnShoot(float damage)
    {
        Despawn();
        Die();
    }

    public virtual void OnLeave(){}

    public virtual void Die() {
		if (deathRoot) {
			deathRoot.SetActive(true);
		}
	}

	public void Despawn()
    {
        renderRoot.SetActive(false);
        colliderRoot.gameObject.SetActive(false);
    }
    
}

