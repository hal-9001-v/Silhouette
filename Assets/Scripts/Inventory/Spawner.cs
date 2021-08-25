using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _prefab;

    [Header("Settings")]
    [SerializeField] [Range(1, 10)] int _spawnNumber;
    [SerializeField] [Range(0, 2)] float _radius;
    [SerializeField] [Range(0, 100)] float _explosionForce;

    public void Spawn(bool randomNumber)
    {
        int numInstances;

        if (randomNumber)
            numInstances = Random.Range(0, 10);
        else
            numInstances = _spawnNumber;

        for (int i = 0; i < numInstances; i++)
        {
            var instance = Instantiate(_prefab);
            instance.transform.position = GetRandomPosition();

            var rigidbody = instance.GetComponent<Rigidbody>();

            if (rigidbody)
            {
                var direction = rigidbody.transform.position - transform.position;
                direction.y = 0;

                rigidbody.AddForce(transform.up * _explosionForce + direction.normalized * _explosionForce * 0.2f, ForceMode.VelocityChange);

            }
        }
    }

    Vector3 GetRandomPosition()
    {
        if (_radius == 0)
        {
            return transform.position;
        }

        Vector3 position = transform.position;

        position += transform.up * Random.Range(0, _radius) + transform.right * Random.Range(0, _radius);

        return position;
    }

}
