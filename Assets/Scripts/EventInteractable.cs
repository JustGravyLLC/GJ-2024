using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteractable : Interactable
{
    private bool interacted = false;
    private bool inside = false;
    private float _minSpeed = 5f;

    public override void OnInteract()
    {
        inside = true;
    }

    public override void OnLeave()
    {
        inside = false;
    }

    private void Update()
    {
        if (interacted) return;

        if (!inside) return;

        if(_gameController.playerCharacter.forwardVelocity < _minSpeed)
        {
            interacted = true;
            _gameController.EnterEvent();
        }
    }
}
