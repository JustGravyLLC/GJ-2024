using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteractable : Interactable
{
    override public void OnInteract()
    {
        _gameController.playerCharacter.HitSlowdown(10f);
        Despawn();
    }

    public override void OnShoot(float damage)
    {
    }
}
