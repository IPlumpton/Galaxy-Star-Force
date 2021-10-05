using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _horizontalPosition;
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    private Player _player = null;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _audioClip = null;

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
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if (transform.position.y <= -5.4f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_audioClip, new Vector3(transform.position.x, transform.position.y, -10f), 50.0f);

            if (_player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        _player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Unknown Powerup Collected");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
