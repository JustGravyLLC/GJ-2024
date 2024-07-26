using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Refs
    public PlayerCharacter _playerCharacter;
    public List<Interactable> interactablePrefabs;

    //Fields
    public int toSpawn = 8;
    public float mountW, mountD = 100f;
    public int mountCount = 5;
    public float speed = 1f;

    //Data
    private List<Interactable> interactablesPool;
    private List<EnvironmentSet> environmentSets;

    private struct EnvironmentSet
    {
        public Transform mount;
        public List<Interactable> interactables;
    }

    private void Start()
    {
        environmentSets = new List<EnvironmentSet>();
        interactablesPool = new List<Interactable>();
        InitialSpawn();
    }

    private void Update()
    {
        float offset = speed * Time.deltaTime * _playerCharacter.forwardVelocity;
        Vector3 d = new Vector3(0, 0, offset);

        foreach(EnvironmentSet e in environmentSets)
        {
            e.mount.Translate(-d);
        }
    

        Transform firstMount = environmentSets[0].mount;

        if (firstMount.position.z < mountD * -1.5f)
        {
            //uninitialize the first mount and put it at the back
            foreach(Interactable i in firstMount.GetComponentsInChildren<Interactable>())
            {
                PoolInteractable(i);
            }

            EnvironmentSet es = environmentSets[0];
            environmentSets.RemoveAt(0);
            Vector3 v = new Vector3(0, 0, environmentSets[environmentSets.Count - 1].mount.position.z + mountD);
            firstMount.position = v;
            environmentSets.Add(es);

            SpawnOnMount(firstMount, toSpawn);
        }
    }

    private void InitialSpawn()
    {
        for(int i = 0; i < mountCount; i++)
        {
            Transform mount = new GameObject().transform;

            mount.parent = this.transform;
            mount.position = new Vector3(0, 0, mountD * i);
            environmentSets.Add(new EnvironmentSet { mount = mount, interactables = new List<Interactable>()});

            SpawnOnMount(mount, toSpawn);
        }
    }

    private void SpawnOnMount(Transform mount, int count)
    {
        float x, z;

        for (int i = 0; i < count; i++)
        {
            x = Random.Range(-mountW, mountW);
            z = Random.value * mountD;

            Spawn(interactablePrefabs[0], mount, new Vector3(x, 0, z));
        }       
    }

    private Interactable Spawn(Interactable interactable, Transform mount, Vector3 pos)
    {
        Interactable newInteractable = GameObject.Instantiate<Interactable>(interactable);
        newInteractable.transform.parent = mount;
        newInteractable.transform.localPosition = pos;
        newInteractable.Initialize(this);

        return newInteractable;
    }

    public void PoolInteractable(Interactable i)
    {
        interactablesPool.Add(i);
        i.Despawn();
    }

    private Interactable GetInteractable()
    {
        Interactable i;
        if (interactablesPool.Count > 0)
        {
            i = interactablesPool[0];
            interactablesPool.RemoveAt(0);
        }else
        {
            i = GameObject.Instantiate<Interactable>(interactablePrefabs[0]);            
        }
        return i;
    }
}

