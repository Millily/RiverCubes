using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner: MonoBehaviour
{
    [SerializeField] private Cube _prefab;

    private ObjectPool<Cube> _poolObjects;
    private int maxCount = 20;

    private Bounds _bounds;

    private Coroutine _coroutine;
    private float _spawnRepeatTime = 0.3f;

    private void Awake()
    {
        _bounds = new Bounds(transform.position, Vector3.one);

        _poolObjects = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            maxSize: maxCount
            );
    }

    private void Start()
    {
        _coroutine = StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_spawnRepeatTime);

        while (enabled)
        {
            GetCube();
            yield return wait;
        }
    }

    private void GetCube()
    {
        _poolObjects.Get();
    }

    private void ReturnToPool(Cube cube)
    {
        _poolObjects.Release(cube);
    }

    private void ActionOnGet(Cube cube)
    {
        float x = Random.Range(_bounds.min.x, _bounds.max.x);
        float y = transform.position.y;
        float z = Random.Range(_bounds.min.z, _bounds.max.z);
        Vector3 position = new Vector3(x, y, z);

        int minTime = 2;
        int maxTime = 5;
        int lifeTime = Random.Range(minTime, maxTime);

        cube.Initialize(lifeTime, position);
        cube.gameObject.SetActive(true);

        cube.LifeEnded += ReturnToPool;
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.LifeEnded -= ReturnToPool;
    }
}