using System;
using UnityEngine;

[RequireComponent(typeof(CheckPointTracker))]
public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Range(1, 10)] float _maxHealth;
    public bool CanGetHurt = true;
    public float CurrentHealth;
    //{ get; private set; }

    public TypeOfCharacter CharacterType;

    public enum TypeOfCharacter
    {
        Player,
        Enemy
    }

    /// <summary>
    /// Every time character is hurt. NOTE: This is not invoked when it is killed.
    /// Vector3 is the point of hit and float is the push velocity
    /// Transform is the hitting GameObject
    /// </summary>
    public Action<Vector3, float, Transform> HurtAction;
    public Action<Vector3, float, Transform> DeadAction;

    public bool IsAlive
    {
        get
        {
            return CurrentHealth <= 0;
        }
        private set
        {

        }
    }

    private void Awake()
    {
        CurrentHealth = _maxHealth;

        CanGetHurt = true;
    }

    public void Hurt(float dmg, Vector3 source, float push, Transform hitter)
    {
        if (CanGetHurt)
        {
            CurrentHealth -= dmg;

            //Debug.Log("Hurt");
            if (CurrentHealth > 0)
            {
                if (HurtAction != null)
                    HurtAction.Invoke(source, push, hitter);

            }
            else
            {
                //Make sure it doesn't get a below zero value just in case
                CurrentHealth = 0;

                if (DeadAction != null)
                    DeadAction.Invoke(source, push, hitter);

            }
        }

    }
}
