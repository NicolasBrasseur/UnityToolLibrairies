using UnityEngine;
using UnityEngine.Pool;

public interface IKeepPoolReference
{
    public void SetPool(ObjectPool<GameObject> pool);
    public ObjectPool<GameObject> GetPool();
}
