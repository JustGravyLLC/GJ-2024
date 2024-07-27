using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteractable : Interactable
{
    [SerializeField]
    private float hp = 20;
    override public void OnInteract()
    {
        _gameController.playerCharacter.HitSlowdown(10f);
        Despawn();
        Die();
    }

    public override void OnShoot(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Despawn();
            Die();
        }
    }
}
