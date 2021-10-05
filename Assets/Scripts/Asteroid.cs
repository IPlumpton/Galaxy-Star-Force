using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 10.0f;
    [SerializeField]
    private GameObject _explosionPrefab = null;
    private SpawnManager _spawnManager = null;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            transform.GetComponent<SpriteRenderer>().enabled = false;
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.2f);
        }
    }
}
