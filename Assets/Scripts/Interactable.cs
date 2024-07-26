using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject renderRoot;
    public GameObject colliderRoot;

    public void Initialize()
    {
        renderRoot.SetActive(true);
        colliderRoot.SetActive(true);
    }

    public void Despawn()
    {
        renderRoot.SetActive(false);
        colliderRoot.SetActive(false);
    }
    
}

