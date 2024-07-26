using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableDictionary.Scripts;

public class Spawner : MonoBehaviour
{
    //Refs
    private GameController _gameController;
    public PlayerCharacter _playerCharacter;
    public List<SpawnParams> _spawnParams;

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
        _gameController = GameObject.FindFirstObjectByType<GameController>();

        _environmentSets = new List<EnvironmentSet>();
        InitialSpawn();
        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;
        if (_gameController.currentState != GameState.RUNNING) return;

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

        foreach(SpawnParams sp in _spawnParams)
        {
            //pick random interactable in list to spawn
            Interactable interactable = sp.interactables[Random.Range(0, sp.interactables.Count)];

            for (int i = 0; i < sp.count; i++)
            {
                x = Random.Range(sp.horizontalMin, sp.horizontalMax);
                x = x * (Random.Range(0, 2) * 2 - 1);
                z = Random.value * mountD * .9f;

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

[System.Serializable]
public class SpawnParams
{
    public List<Interactable> interactables;
    public float count;
    public float horizontalMin, horizontalMax;
}
