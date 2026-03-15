using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Pool;


namespace NB_ToolLibrary
{
    // based on https://www.youtube.com/watch?v=Ah3epb2HGCw and https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Pool.ObjectPool_1.html
    public class ObjectPoolManager : MonoBehaviour
    {
        #region Variables declaration

        private static ObjectPoolManager _instance;
        public static ObjectPoolManager Instance => _instance;

        [SerializeField] private bool _dontDestroyOnLoad = false;
        private static GameObject _poolDefaultParent;
        private static Dictionary<GameObject, ObjectPool<GameObject>> _poolsList = new Dictionary<GameObject, ObjectPool<GameObject>>();

        private const int DEFAULT_POOL_SIZE = 100;
        private const int DEFAULT_POOL_MAX_SIZE = 100;

        #endregion


        private void Awake()
        {
            // Singleton
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                if(_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }

            SetupPoolParents();

            void SetupPoolParents()
            {
                _poolDefaultParent = new GameObject("Default Parent");
                _poolDefaultParent.transform.parent = transform;

            }
        }


        public static void CreatePool(GameObject prefab, int defaultPoolSize, int maxPoolSize, bool preWarmPool = false, Transform overwrittenParent = null)
        {
            if(_poolsList.ContainsKey(prefab))
            {
                Debug.LogWarning($"A pool already exist for the prefab : {prefab}");
                return;
            }

            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: () => CreateNewItem(prefab, GetParent(overwrittenParent)), // Lambda expression is used to workaround limitation of createFunc not being allowed to have parameters
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroy,
                collectionCheck: true,
                defaultCapacity: defaultPoolSize,
                maxSize: maxPoolSize);

            _poolsList.TryAdd(prefab, pool);

            if(preWarmPool)
            {
                Queue<GameObject> preWarmObjects = new Queue<GameObject>();

                for(int i = 0; i < defaultPoolSize; i++)
                {
                    GameObject spawnedObject = SpawnObject(prefab, Vector3.zero, Quaternion.identity);
                    preWarmObjects.Enqueue(spawnedObject);
                }

                for (int i = 0; i < defaultPoolSize; i++)
                {
                    GameObject spawnedObject = preWarmObjects.Dequeue();
                    ReleaseObject(spawnedObject, pool);
                }
            }

            Transform GetParent(Transform overwrittenParent)
            {
                if(overwrittenParent != null)
                {
                    return overwrittenParent;
                }

                return _poolDefaultParent.transform;
            }
        }

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            ObjectPool<GameObject> pool = null;

            // Test if objectToSpawn is linked to a pool, if not, create a new pool
            if(!_poolsList.TryGetValue(objectToSpawn, out pool))
            {
                Debug.LogWarning($"No pool associated with {objectToSpawn} thus a new pool with default values has been created");
                CreatePool(objectToSpawn, DEFAULT_POOL_SIZE, DEFAULT_POOL_MAX_SIZE);
                _poolsList.TryGetValue(objectToSpawn, out pool);
            }

            // Safety test of pool data, should never be null
            if(pool == null)
            {
                Debug.LogError($"No pool data for {objectToSpawn}, something wrong happened and no object has been spawned");
                return null;
            }

            // Retreive object from pool or instantiate it
            GameObject spawnedObject = pool.Get();

            // Save pool reference if spawned object implement special interface
            if (spawnedObject.TryGetComponent<IKeepPoolReference>(out IKeepPoolReference itemComponent))
            {
                itemComponent.SetPool(pool);
            }

            spawnedObject.transform.position = spawnPosition;
            spawnedObject.transform.rotation = spawnRotation;

            return spawnedObject;
        }

        public static void ReleaseObject(GameObject objectToRelease, ObjectPool<GameObject> pool)
        {
            pool.Release(objectToRelease);
        }

        private static GameObject CreateNewItem(GameObject prefab, Transform parent)
        {
            prefab.SetActive(false); // Ensure that the spawned object don't call the Awake or OnEnabled methods

            GameObject newItem = Instantiate(prefab);
            newItem.SetActive(false);

            prefab.SetActive(true);
            newItem.transform.SetParent(parent);

            return newItem;
        }

        private static void OnGet(GameObject item)
        {
            item.SetActive(true);
        }

        private static void OnRelease(GameObject item)
        {
            item.SetActive(false);
        }

        private static void OnDestroy(GameObject item)
        {
            Destroy(item);
        }
    }
}
