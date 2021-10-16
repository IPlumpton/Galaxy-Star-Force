using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab = null;
    [SerializeField] private GameObject _enemyContainer = null;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private GameObject _ammoCollectablePrefab = null;
    [SerializeField] private float _spawnDelay = 5.0f;
    [SerializeField] private Vector2 _powerupSpawnRandomRange;
    [SerializeField] private float _ammoSpawnDelay = 15f;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnAmmoRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            GameObject _newEnemy = Instantiate(_enemyPrefab);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, _powerups.Length);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup]);
            yield return new WaitForSeconds(Random.Range(_powerupSpawnRandomRange.x, _powerupSpawnRandomRange.y));
        }
    }

    IEnumerator SpawnAmmoRoutine()
    {
        yield return new WaitForSeconds(_ammoSpawnDelay);

        while (_stopSpawning == false)
        {
            GameObject newAmmoCollectable = Instantiate(_ammoCollectablePrefab);
            yield return new WaitForSeconds(_ammoSpawnDelay);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
