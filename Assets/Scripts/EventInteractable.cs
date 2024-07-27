using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteractable : Interactable
{
    private bool interacted = false;
    private float timer = 2f;
    private bool runTime = false;

    public override void OnInteract()
    {
        //Count time to interact
        runTime = true;
    }

    public override void OnLeave()
    {
        if (runTime) runTime = false;
    }

    private void Update()
    {
        if (interacted) return;

        //if timer is running, decrement
        if (runTime){timer -= Time.deltaTime;}

        if(timer < 0)
        {
            interacted = true;
            _gameController.EnterEvent();
        }
    }
}
