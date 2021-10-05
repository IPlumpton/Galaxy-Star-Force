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
    private GameObject _tripleShotPrefab = null;
    [SerializeField]
    private float _laserCooldown = 1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;
    [SerializeField]
    private float _powerUpDuration = 5.0f;
    [SerializeField]
    private GameObject _shieldVisualiser = null;
    [SerializeField]
    private GameObject _leftEngineVisualiser, _rightEngineVisualiser;
    [SerializeField]
    private AudioClip _laserAudioClip;

    private float _canFire = -1.0f;
    private SpawnManager _spawnManager = null;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    private bool _isSpeedPowerUpActive = false;
    private int _playerScore = 0;
    private UIManager _UIManager = null;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        if (_UIManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null");
        }

        _shieldVisualiser.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _isTripleShotActive)
        {
            _canFire = Time.time + _laserCooldown;
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _audioSource.clip = _laserAudioClip;
            _audioSource.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _laserCooldown;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            _audioSource.clip = _laserAudioClip;
            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualiser.SetActive(false);
            return;
        }

        _lives--;
        Debug.Log("Lives: " + _lives);

        _UIManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngineVisualiser.SetActive(true);
        }
        
        if (_lives == 1)
        {
            _rightEngineVisualiser.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _UIManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(_powerUpDuration);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        if (_isSpeedPowerUpActive)
        {
            StopCoroutine(SpeedBoostPowerDown());
            StartCoroutine(SpeedBoostPowerDown());
            return;
        }
        else
        {
            _playerSpeed *= _speedBoostMultiplier;

            _isSpeedPowerUpActive = true;

            StopCoroutine(SpeedBoostPowerDown());
            StartCoroutine(SpeedBoostPowerDown());
        }
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(_powerUpDuration);
        _playerSpeed /= _speedBoostMultiplier;
        _isSpeedPowerUpActive = false;
    }

    public void ShieldsActive()
    {
        _isShieldActive = true;
        _shieldVisualiser.SetActive(true);
    }

    public void AddScore(int points)
    {
        _playerScore += points;
        _UIManager.UpdateScoreText(_playerScore);
    }
}
