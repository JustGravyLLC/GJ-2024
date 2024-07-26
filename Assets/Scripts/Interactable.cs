using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Spawner _spawner;
    public GameObject renderRoot;
    public ColliderObject colliderRoot;

    public void Initialize(Spawner s)
    {
        _spawner = s;
        renderRoot.SetActive(true);
        colliderRoot.gameObject.SetActive(true);
    }

    public void OnInteract()
    {
        _spawner.PoolInteractable(this);
    }



    public void Despawn()
    {
        renderRoot.SetActive(false);
        colliderRoot.gameObject.SetActive(false);
    }
    
}

