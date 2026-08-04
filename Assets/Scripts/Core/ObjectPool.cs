using UnityEngine;

/// <summary>
/// Object Pool for efficient instantiation and reuse of game objects
/// Implements the object pool pattern to avoid runtime instantiation overhead
/// </summary>
/// <typeparam name="T">Type of MonoBehaviour to pool</typeparam>
public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private T prefab;
    [SerializeField] private int initialSize = 10;
    [SerializeField] private bool autoExpand = true;
    
    private Queue<T> _pool = new Queue<T>();
    private Transform _container;
    private static ObjectPool<T> _instance;

    public static ObjectPool<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject(typeof(T).Name + "Pool");
                _instance = go.AddComponent<ObjectPool<T>>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        _container = new GameObject("Container").transform;
        _container.SetParent(transform);
        
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNew();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    private T CreateNew()
    {
        T obj = Instantiate(prefab, _container);
        obj.gameObject.SetActive(false);
        return obj;
    }

    /// <summary>
    /// Get an object from the pool
    /// </summary>
    /// <param name="position">Position to place the object</param>
    /// <param name="rotation">Rotation to apply to the object</param>
    /// <returns>Pooled object instance</returns>
    public T Get(Vector3 position, Quaternion rotation)
    {
        T obj;
        
        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else if (autoExpand)
        {
            obj = CreateNew();
        }
        else
        {
            Debug.LogWarning($"Pool exhausted for {typeof(T).Name}. Consider increasing initial size.");
            return null;
        }
        
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);
        
        return obj;
    }

    /// <summary>
    /// Get an object from the pool at default position
    /// </summary>
    public T Get()
    {
        return Get(Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Return an object to the pool
    /// </summary>
    /// <param name="obj">Object to return</param>
    public void Return(T obj)
    {
        if (obj == null) return;
        
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    /// <summary>
    /// Return all active objects to the pool
    /// </summary>
    public void ReturnAll()
    {
        foreach (Transform child in _container)
        {
            if (child.gameObject.activeSelf)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                {
                    Return(component);
                }
            }
        }
    }

    /// <summary>
    /// Pre-warm the pool with additional objects
    /// </summary>
    /// <param name="count">Number of additional objects to create</param>
    public void Expand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T obj = CreateNew();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (initialSize < 1) initialSize = 1;
    }
#endif
}
