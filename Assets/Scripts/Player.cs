using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _basePlayerSpeed = 5.5f;
    [SerializeField] private GameObject _laserPrefab = null;
    [SerializeField] private GameObject _tripleShotPrefab = null;
    [SerializeField] private GameObject _photonPrefab = null;
    [SerializeField] private float _laserCooldown = 1f;
    [SerializeField] private float _photonCooldown = 1f;
    [SerializeField] private float _photonPowerUpDuration = 10f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _playerStartingAmmo = 15;
    [SerializeField] private float _speedBoostMultiplier = 2.0f;
    [SerializeField] private float _powerUpDuration = 5.0f;
    [SerializeField] private GameObject _shieldVisualiser = null;
    [SerializeField] private GameObject _leftEngineVisualiser, _rightEngineVisualiser;
    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private AudioClip _photonAudioClip;
    [SerializeField] private AudioClip _outOfAmmoClip;

    private float _canFire = -1.0f;
    private float _playerDamagedTime;
    private float _playerSafePeriod = 0.2f;
    private float _playerSpeed;
    private int _playerScore = 0;
    private int _shieldActiveLevel = 0;
    private int _playerCurrentAmmo;
    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerUpActive = false;
    private bool _isPhotonActive = false;

    private SpawnManager _spawnManager = null;
    private UIManager _UIManager = null;
    private AudioSource _audioSource;
    private Shield _playerShield;

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

        _playerSpeed = _basePlayerSpeed;

        _playerShield = _shieldVisualiser.GetComponent<Shield>();

        _shieldVisualiser.SetActive(false);

        UpdateAmmo(_playerStartingAmmo);
    }

    void Update()
    {
        PlayerMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _isPhotonActive)
        {
            FirePhoton();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _playerSpeed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.8f, 8.8f),
                                         Mathf.Clamp(transform.position.y, -3.5f, 4.0f), 0);

        if (Input.GetKey(KeyCode.LeftShift) && _isSpeedPowerUpActive == false)
        {
            _playerSpeed = _basePlayerSpeed * _speedBoostMultiplier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _isSpeedPowerUpActive == false)
        {
            _playerSpeed = _basePlayerSpeed;
        }
    }

    void FireLaser()
    {
        if(_playerCurrentAmmo <= 0)
        {
            _audioSource.clip = _outOfAmmoClip;
            _audioSource.Play();
            return;
        }

        _canFire = Time.time + _laserCooldown;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (!_isTripleShotActive)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.clip = _laserAudioClip;
        _audioSource.Play();
        UpdateAmmo(-1);
    }

    void FirePhoton()
    {
        _canFire = Time.time + _photonCooldown;
        Instantiate(_photonPrefab, transform.position, Quaternion.identity);
        _audioSource.Play();
    }

    public void Damage()
    {
        if(_playerDamagedTime > Time.time)
        {
            return;
        }

        switch(_shieldActiveLevel)
        {
            case 3:
                _playerShield.SetShieldColor(2);
                _shieldActiveLevel--;
                _playerDamagedTime = Time.time + _playerSafePeriod;
                return;
            case 2:
                _playerShield.SetShieldColor(1);
                _shieldActiveLevel--;
                _playerDamagedTime = Time.time + _playerSafePeriod;
                return;
            case 1:
                _playerShield.SetShieldColor(0);
                _shieldActiveLevel--;
                _playerDamagedTime = Time.time + _playerSafePeriod;
                return;
            default:
                break;
        }

        _lives--;
        _playerDamagedTime = Time.time + _playerSafePeriod;

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
            StopCoroutine("SpeedBoostPowerDown");
            StartCoroutine("SpeedBoostPowerDown");
            return;
        }
        else
        {
            StopCoroutine("SpeedBoostPowerDown");

            _playerSpeed = _basePlayerSpeed;

            _playerSpeed *= _speedBoostMultiplier;

            _isSpeedPowerUpActive = true;

            StartCoroutine("SpeedBoostPowerDown");
        }
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(_powerUpDuration);
        _playerSpeed = _basePlayerSpeed;
        _isSpeedPowerUpActive = false;
    }

    public void ShieldsActive()
    {
        _shieldVisualiser.SetActive(true);
        _shieldActiveLevel = 3;
        _playerShield.SetShieldColor(3);
    }

    public void PhotonLaserActive()
    {
        _isPhotonActive = true;
        _audioSource.clip = _photonAudioClip;
        StartCoroutine(PhotonLaserPowerDown());
    }

    IEnumerator PhotonLaserPowerDown()
    {
        yield return new WaitForSeconds(_photonPowerUpDuration);
        _isPhotonActive = false;
    }

    public void UpdateAmmo(int ammoAdjustment)
    {
        _playerCurrentAmmo += ammoAdjustment;
        _UIManager.UpdateAmmoText(_playerCurrentAmmo);
    }

    public void RestoreHealth(int healthRestored)
    {
        _lives += healthRestored;

        if(_lives >= 3)
        {
            _lives = 3;
            _rightEngineVisualiser.SetActive(false);
            _leftEngineVisualiser.SetActive(false);
        }
        else if(_lives == 2)
        {
            _leftEngineVisualiser.SetActive(false);
        }

        _UIManager.UpdateLives(_lives);
    }

    public void AddScore(int points)
    {
        _playerScore += points;
        _UIManager.UpdateScoreText(_playerScore);
    }
}
