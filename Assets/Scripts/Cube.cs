using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer), typeof(ChangerColor))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private ChangerColor _changerColor;
    private Color _startColor;
    private Color _colorAfterCollision = Color.blue;

    private Coroutine _coroutine;
    private int _lifeTime;
    private bool _platformCollision = false;

    public Rigidbody Rigidbody { get; private set; }

    public event Action<Cube> LifeEnded;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
        _changerColor = GetComponent<ChangerColor>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize(int lifeTime, Vector3 position)
    {
        _lifeTime = lifeTime;
        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out _) && _platformCollision == false)
        {
            _platformCollision = true;
            _changerColor.ChangeColor(_renderer, _colorAfterCollision);
            _coroutine = StartCoroutine(CountLifeTime(_lifeTime));
        }
    }

    private IEnumerator CountLifeTime(int end)
    {
        float delay = 1f;
        var wait = new WaitForSeconds(delay);

        for (int i = 0; i < end; i++)
        {
            yield return wait;
        }

        LifeEnded?.Invoke(this);
        _changerColor?.ChangeColor(_renderer, _startColor);
        _platformCollision = false;
    }
}