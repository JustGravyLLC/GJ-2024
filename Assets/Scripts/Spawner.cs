using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableDictionary.Scripts;

public class Spawner : MonoBehaviour
{
    //Refs
    public PlayerCharacter _playerCharacter;
    public SerializableDictionary<List<Interactable>, int> _interactableSpawnDictionary;

    //Fields
    public float mountW, mountD = 100f;
    public int mountCount = 5;
    public float speed = 1f;
    private bool initialized = false;

    //Data
    private List<EnvironmentSet> _environmentSets;

    private struct EnvironmentSet
    {
        public Transform mount;
        public List<Interactable> interactables;
    }

    public void Initialize()
    {
        _environmentSets = new List<EnvironmentSet>();
        InitialSpawn();
        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        float offset = speed * Time.deltaTime * _playerCharacter.forwardVelocity;
        Vector3 d = new Vector3(0, 0, offset);

        foreach(EnvironmentSet e in _environmentSets)
        {
            e.mount.Translate(-d);
        }
    

        Transform firstMount = _environmentSets[0].mount;

        if (firstMount.position.z < mountD * -1.5f)
        {
            //uninitialize the first mount and put it at the back
            foreach(Interactable i in firstMount.GetComponentsInChildren<Interactable>())
            {
                i.Despawn();
            }

            EnvironmentSet es = _environmentSets[0];
            _environmentSets.RemoveAt(0);
            Vector3 v = new Vector3(0, 0, _environmentSets[_environmentSets.Count - 1].mount.position.z + mountD);
            firstMount.position = v;
            _environmentSets.Add(es);

            SpawnOnMount(firstMount);
        }
    }

    private void InitialSpawn()
    {
        for(int i = 0; i < mountCount; i++)
        {
            Transform mount = new GameObject().transform;

            mount.parent = this.transform;
            mount.position = new Vector3(0, 0, mountD * i);
            _environmentSets.Add(new EnvironmentSet { mount = mount, interactables = new List<Interactable>()});

            SpawnOnMount(mount);
        }
    }

    private void SpawnOnMount(Transform mount)
    {
        float x, z;

        foreach(List<Interactable> interactables in _interactableSpawnDictionary.Dictionary.Keys)
        {
            //pick random interactable in list to spawn
            Interactable interactable = interactables[Random.Range(0, interactables.Count)];

            for (int i = 0; i < _interactableSpawnDictionary.Get(interactables); i++)
            {
                x = Random.Range(-mountW, mountW);
                z = Random.value * mountD;

                Spawn(interactable, mount, new Vector3(x, 0, z));
            }    
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
}

