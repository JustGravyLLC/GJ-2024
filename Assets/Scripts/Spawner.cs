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
    public List<Transform> spawnMounts;
    public List<Interactable> interactablesPool;

    private void Start()
    {
        InitialSpawn();
    }

    private void Update()
    {
        float offset = speed * Time.deltaTime * _playerCharacter.forwardVelocity;
        Vector3 d = new Vector3(0, 0, offset);

        for (int i = 0; i < spawnMounts.Count; i++)
        {
            spawnMounts[i].Translate(-d);
        }

        Transform firstMount = spawnMounts[0];

        if (firstMount.position.z < mountD * -1.5f)
        {
            //uninitialize the first mount and put it at the back
            foreach(Interactable i in firstMount.GetComponentsInChildren<Interactable>())
            {
                PoolInteractable(i);
            }

            spawnMounts.RemoveAt(0);
            Vector3 v = new Vector3(0, 0, spawnMounts[spawnMounts.Count - 1].position.z + mountD);
            firstMount.position = v;
            spawnMounts.Add(firstMount);

            SpawnOnMount(firstMount, toSpawn);
        }
    }

    private void InitialSpawn()
    {
        for(int i = 0; i < mountCount; i++)
        {
            GameObject mount = new GameObject();
            mount.transform.parent = this.transform;
            mount.transform.position = new Vector3(0, 0, mountD * i);
            spawnMounts.Add(mount.transform);
            SpawnOnMount(mount.transform, toSpawn);
        }
    }

    private void SpawnOnMount(Transform mount, int count)
    {
        float x = 0;
        float z = 0;

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
        newInteractable.Initialize();

        return newInteractable;
    }

    private void PoolInteractable(Interactable i)
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

