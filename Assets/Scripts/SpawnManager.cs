using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab = null;
    [SerializeField]
    private GameObject _enemyContainer = null;
    [SerializeField]
    private float _spawnDelay = 5.0f;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject _newEnemy = Instantiate(_enemyPrefab);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
