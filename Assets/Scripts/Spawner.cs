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
    public List<List<Interactable>> interactablesDictionary;
    public List<Transform> spawnMounts;

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
    }

    private void InitialSpawn()
    {
        for(int i = 0; i < mountCount; i++)
        {
            GameObject mount = new GameObject();
            mount.transform.parent = this.transform;
            mount.transform.position = new Vector3(0, 0, mountD * i);
            spawnMounts.Add(mount.transform);
            SpawnOnMount(mount, toSpawn);
        }
    }

    private void SpawnOnMount(GameObject mount, int count)
    {
        //pick random number on x and z for position
        float x = 0;
        float z = 0;

        for (int i = 0; i < count; i++)
        {
            x = Random.Range(-mountW, mountW);
            z = Random.value * mountD;

            //spawn there
            Spawn(interactablePrefabs[0], mount, new Vector3(x, 0, z));
        }       
    }

    private Interactable Spawn(Interactable interactable, GameObject mount, Vector3 pos)
    {
        Interactable newInteractable = GameObject.Instantiate<Interactable>(interactable);
        newInteractable.transform.parent = mount.transform;
        newInteractable.transform.localPosition = pos;
        newInteractable.Initialize();

        return newInteractable;
    }
}

