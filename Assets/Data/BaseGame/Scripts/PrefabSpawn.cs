using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawn : MonoBehaviour
{
    public GameObject Prefab;
    
    public GameObject Instance { get; private set; }

    void Start()
    {
        Instance = Instantiate(Prefab, transform);
    }
}
