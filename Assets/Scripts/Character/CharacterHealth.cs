using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CheckPointTracker))]
public class CharacterHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI _hpTextMesh;
    [SerializeField] [Range(1, 10)] float _maxHealth;
    [SerializeField] bool _canGetHurt;

    Rigidbody _rigidBody;
    CheckPointTracker _checkPointTracker;
    float _currentHealth;
    

        

    public bool IsAlive
    {
        get
        {
            return _currentHealth <= 0;
        }
        private set
        {

        }
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;


        _canGetHurt = true;
        _rigidBody = GetComponent<Rigidbody>();
        _checkPointTracker = GetComponent<CheckPointTracker>();

    }

    public void KillPlayer()
    {
        //Debug.Log("DeaD!");
        _checkPointTracker.SpawnAtCheckPoint();
        _currentHealth = 10;
        _hpTextMesh.text = _currentHealth.ToString();

    }

    public void HurtPlayer(float dmg)
    {
        if (_canGetHurt)
        {
            _currentHealth -= dmg;
            if (_hpTextMesh)
            {
                _hpTextMesh.text = _currentHealth.ToString();
            }

            //Debug.Log("Hurt");
            if (_currentHealth <= 0)
            {
                KillPlayer();

            }
        }

    }

    /// <summary>
    /// Hurt Player and push it adding velocity
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="push"></param>
    public void HurtPlayer(float dmg, Vector3 pushVelocity)
    {
        HurtPlayer(dmg);

        _rigidBody.velocity = pushVelocity;
    }
}
