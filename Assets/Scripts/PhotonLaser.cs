using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLaser : MonoBehaviour
{
    private float _photonSpeed = 15f;
    private Transform _target;
    private GameObject[] photonTargets;
    private Transform[] _currentEnemies;

    private void Start()
    {
        photonTargets = GameObject.FindGameObjectsWithTag("Enemy");

        _currentEnemies = new Transform[photonTargets.Length];

        for (int i = 0; i < photonTargets.Length; i++)
        {
            _currentEnemies[i] = photonTargets[i].transform;
        }

        GetClosestEnemy(_currentEnemies);
    }

    private void Update()
    {
        if(_target == null)
        {
            MoveUp();
        }
        else
        {
            MoveToTarget();
        }

    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _photonSpeed * Time.deltaTime);

        if (transform.position.y > 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    void MoveToTarget()
    {
        float step = _photonSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
        if(Vector3.Distance(transform.position, _target.position) == 0f)
        {
            Destroy(gameObject);
        }
    }

    Transform GetClosestEnemy(Transform[] enemies)
    {
        _target = null;
        float smallestDistanceSqd = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float distanceSqdToTarget = directionToTarget.sqrMagnitude;
            if(distanceSqdToTarget < smallestDistanceSqd)
            {
                smallestDistanceSqd = distanceSqdToTarget;
                _target = potentialTarget;
            }
        }

        return _target;
    }
}
