using UnityEngine;
using NB_ToolLibrary;

public class SpawnPooledObjects : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private bool _spawn = false;
    [SerializeField] private bool _playSound = false;
    [SerializeField] private AudioRandomizer _randomizer;
    [Space(5)]
    [Header("Pool")]
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private Vector3 _spawnPosition = Vector3.zero;


    private void Awake()
    {
        _spawn = false;
    }

    private void Start()
    {
        ObjectPoolManager.CreatePool(_spawnObject, 10, 15 , true, transform);
    }

    private void Update()
    {
        if(_spawn)
        {
            GameObject spawnedObject = ObjectPoolManager.SpawnObject(_spawnObject, _spawnPosition, Quaternion.identity);
            if(spawnedObject.TryGetComponent<SpawnedObject>(out SpawnedObject spawnedObjectComponent)) { spawnedObjectComponent.SetDeleteTimer(); }

            _spawn = false;
        }

        if(_playSound)
        {

            _randomizer.PlayAudio();

            _playSound = false;
        }
    }
}
