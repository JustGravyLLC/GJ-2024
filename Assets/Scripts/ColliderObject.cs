using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderObject : MonoBehaviour
{
    Interactable parent;
    bool initialized = false;

    private void Start()
    {
        parent = this.transform.parent.GetComponent<Interactable>();
        initialized = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collision enter");
        if(initialized)
        {
            parent.OnInteract();
        }        
    }
}
