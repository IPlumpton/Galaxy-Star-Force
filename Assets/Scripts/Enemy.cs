using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 3.0f;
    [SerializeField] private float _fireRate = 3.0f;
    [SerializeField] private GameObject _laserPrefab;

    private Player _player;
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _horizontalPosition;
    private float _canFire = -1;


    void Start()
    {
        _horizontalPosition = Random.Range(-8.0f, 8.0f);
        transform.position = new Vector3(_horizontalPosition, 7.4f, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.Log("Player is null");
        }

        if (_animator == null)
        {
            Debug.Log("Animator is NULL");
        }

        if (_audioSource == null)
        {
            Debug.Log("Audio Source is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -5.4f)
        {
            _horizontalPosition = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(_horizontalPosition, 7.4f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (_player != null)
            {
                Debug.Log("Enemy damages player");
                _player.Damage();
            }

            _animator.SetTrigger("OnEnemyDeath");

            _audioSource.Play();

            gameObject.GetComponent<Collider2D>().enabled = false;

            _enemySpeed = 1.7f;

            Destroy(this.gameObject, 2.8f);
        }
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _animator.SetTrigger("OnEnemyDeath");

            _audioSource.Play();

            gameObject.GetComponent<Collider2D>().enabled = false;

            _enemySpeed = 1.7f;

            Destroy(this.gameObject, 2.8f);
        }
    }


}
