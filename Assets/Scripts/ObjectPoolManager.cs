using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public Transform prefab;
        public int size;
        public Transform objectParent;
    }
    public static ObjectPoolManager Instance { get; private set; }
    [SerializeField] private List<Pool> pools = new List<Pool>();
    [SerializeField] private Dictionary<string, Queue<Transform>> objectPoolDictionary = new Dictionary<string, Queue<Transform>>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        foreach (Pool pool in pools)
        {
            Queue<Transform> queue = new Queue<Transform>();
            for (int i = 0; i < pool.size; i++)
            {
                Transform obj = Instantiate(pool.prefab);
                obj.gameObject.SetActive(false);
                queue.Enqueue(obj);
                obj.SetParent(pool.objectParent);
            }
            if (!objectPoolDictionary.ContainsKey(pool.tag))
            {
                objectPoolDictionary.Add(pool.tag, queue);
            }
        }
    }
    public Transform SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!objectPoolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Don't have object with tag : " + tag);
            return null;
        }

        Queue<Transform> queue = objectPoolDictionary[tag];
        Transform obj = queue.Dequeue();

        if (obj.gameObject.activeInHierarchy)
        {
            Debug.LogWarning($"Pool for tag {tag} is empty. Expanding pool...");
            Pool pool = pools.Find(p => p.tag == tag);
            obj = Instantiate(pool.prefab, pool.objectParent);
            obj.gameObject.SetActive(false);
        }

        obj.gameObject.SetActive(true);
        obj.position = position;
        obj.rotation = rotation;

        queue.Enqueue(obj);
        return obj;
    }
    public void ReturnToPool(string tag, Transform obj)
    {
        if (!objectPoolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Don't have object with tag : " + tag);
            return;
        }

        obj.gameObject.SetActive(false);
        objectPoolDictionary[tag].Enqueue(obj);
    }
}
