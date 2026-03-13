using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using NB_ToolLibrary;

public class SpawnedObject : MonoBehaviour, IKeepPoolReference
{
    private ObjectPool<GameObject> _pool;

    public ObjectPool<GameObject> GetPool()
    {
        return _pool;
    }

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }


    private void Update()
    {
        transform.Translate(0, 4f * Time.deltaTime, 0);
    }

    public void SetDeleteTimer()
    {
        StartCoroutine(DeleteObject());
    }

    private IEnumerator DeleteObject()
    {
        yield return new WaitForSeconds(5);

        ObjectPoolManager.ReleaseObject(gameObject, _pool);
    }
}
