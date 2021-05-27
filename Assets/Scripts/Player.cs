using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 5.5f;
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private float _laserCooldown = 1.0f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager = null;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }

    void Update()
    {
        PlayerMovement();

        PlayerShooting();
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _playerSpeed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.8f, 8.8f),
                                         Mathf.Clamp(transform.position.y, -3.5f, 4.0f), 0);
    }

    void PlayerShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _laserCooldown;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives -= 1;
        Debug.Log("Lives: " + _lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
