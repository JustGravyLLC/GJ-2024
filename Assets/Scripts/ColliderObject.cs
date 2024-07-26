using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderObject : MonoBehaviour
{
    Interactable parent;
    bool initialized = false;


    public void Initialize(Interactable i)
    {
        parent = i;
        initialized = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(initialized)
        {
            parent.OnInteract();
        }        
    }


}
