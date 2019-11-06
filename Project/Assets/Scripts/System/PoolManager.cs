using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{
    public static Transform trans;
    public static Dictionary<string, Pool> dicPools = new Dictionary<string, Pool>();
    [SerializeField]
    private List<Pool> pools = new List<Pool>();
    // Use this for initialization
    void Start()
    {
        // DontDestroyOnLoad(gameObject);
        dicPools.Clear();
        trans = transform;
        foreach (Pool e in pools)
        {
            e.Setup();
            dicPools.Add(e.namePool, e);
        }
    }
    public static void AddNewPool(Pool newPool)
    {
        if (!dicPools.ContainsKey(newPool.namePool))
        {
            newPool.Setup();
            dicPools.Add(newPool.namePool, newPool);
        }

    }

    public static Transform CreateNewpreFab(Transform prefab)
    {
        return Instantiate(prefab);
    }
}
[Serializable]
public class Pool
{
    public string namePool;
    private List<Transform> gameObjects = new List<Transform>();
    public Transform prefab;
    public int maxObject;
    public bool hideHierarchy;
    public Transform parent;

    private int indexBulle = -1;
    public void Setup()
    {
        for (int i = 0; i < maxObject; i++)
        {
            Transform bl = PoolManager.CreateNewpreFab(prefab);
            bl.SetParent(parent, false);
            bl.gameObject.SetActive(false);
            if(hideHierarchy)
                bl.hideFlags = HideFlags.HideInHierarchy;
            gameObjects.Add(bl);
        }
    }
    // Use this for initialization
    public Transform GetObjectInstance()
    {
        indexBulle++;
        if (indexBulle >= maxObject)
        {
            indexBulle = 0;
        }
        Transform bullet = gameObjects[indexBulle];
        bullet.gameObject.SetActive(true);
        bullet.SendMessage("OnSpawned", null, SendMessageOptions.DontRequireReceiver);
        return bullet;
    }
    public void OnInactive(GameObject bl)
    {
        bl.SendMessage("OnDeSpawned", null, SendMessageOptions.DontRequireReceiver);
        bl.SetActive(false);
    }
}