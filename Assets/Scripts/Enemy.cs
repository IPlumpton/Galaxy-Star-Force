using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _horizontalPosition;
    [SerializeField]
    private float _enemySpeed = 3.0f;
    private Player _player;

    void Start()
    {
        _horizontalPosition = Random.Range(-8.0f, 8.0f);
        transform.position = new Vector3(_horizontalPosition, 7.4f, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("Player is null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -5.4f)
        {
            _horizontalPosition = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(_horizontalPosition, 7.4f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (_player != null)
            {
                Debug.Log("Enemy damages player");
                _player.Damage();
            }

            Destroy(this.gameObject);
        }
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
