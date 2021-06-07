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
    private GameObject[] _powerups;
    [SerializeField]
    private float _spawnDelay = 5.0f;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject _newEnemy = Instantiate(_enemyPrefab);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, _powerups.Length);
            GameObject _newPowerup = Instantiate(_powerups[randomPowerup]);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
