using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Range(0, 10)] float _maxHealth;

    public bool canGetHurt = true;
    public float currentHealth;
    //{ get; private set; }

    public TypeOfCharacter typeOfCharacter;

    public enum TypeOfCharacter
    {
        Player,
        Enemy,
        Breakable
    }

    /// <summary>
    /// Every time character is hurt. NOTE: This is not invoked when it is killed.
    /// Vector3 is the point of hit and float is the push velocity
    /// Transform is the hitting GameObject
    /// </summary>
    public Action<Vector3, float, Transform> hurtAction;
    public Action<Vector3, float, Transform> deadAction;

    public bool isAlive
    {
        get
        {
            return currentHealth <= 0;
        }
        private set
        {

        }
    }

    private void Awake()
    {
        currentHealth = _maxHealth;

        canGetHurt = true;
    }

    public void Hurt(float dmg, Vector3 source, float push, Transform hitter)
    {
        if (canGetHurt)
        {
            currentHealth -= Mathf.Abs(dmg);

            //Debug.Log("Hurt");
            if (currentHealth > 0)
            {
                if (hurtAction != null)
                    hurtAction.Invoke(source, push, hitter);

            }
            else
            {
                //Make sure it doesn't get a below zero value just in case
                currentHealth = 0;

                if (deadAction != null)
                    deadAction.Invoke(source, push, hitter);

            }
        }

    }


}
