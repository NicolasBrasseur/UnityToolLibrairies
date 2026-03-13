using System.Collections.Generic;
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

        [SerializeField] private static GameObject _poolDefaultParent;
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
                DontDestroyOnLoad(gameObject);
            }
        }


        public static void CreatePool(GameObject prefab, int defaultPoolSize, int maxPoolSize)
        {
            if(_poolsList.ContainsKey(prefab))
            {
                Debug.LogWarning($"A pool already exist for the prefab : {prefab}");
                return;
            }

            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: () => CreateNewItem(prefab), // Lambda expression is used to workaround limitation of createFunc not being allowed to have parameters
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroy,
                collectionCheck: true,
                defaultCapacity: defaultPoolSize,
                maxSize: maxPoolSize);

            _poolsList.TryAdd(prefab, pool);
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

            GameObject spawnedObject = pool.Get();

            spawnedObject.transform.position = spawnPosition;
            spawnedObject.transform.rotation = spawnRotation;

            return spawnedObject;
        }



        private static GameObject CreateNewItem(GameObject prefab)
        {
            prefab.SetActive(false); // Ensure that the spawned object don't call the Awake or OnEnabled methods

            GameObject newItem = Instantiate(prefab);
            newItem.SetActive(false);

            prefab.SetActive(true);
            newItem.transform.SetParent(_poolDefaultParent.transform);

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
