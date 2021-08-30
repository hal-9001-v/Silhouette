using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketPicker : InputComponent
{
    [Header("References")]
    [SerializeField] Inventory _inventory;
    MobRegister _mobRegister;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5)] float _range;
    [SerializeField] [Range(0.1f, 5)] float _cooldown;
    [SerializeField] Color _gizmosColor = Color.blue;

    bool _readyToPoke = true;

    private void Awake()
    {
        _mobRegister = FindObjectOfType<MobRegister>();
    }

    void Poke()
    {
        if (_readyToPoke && _mobRegister && _inventory)
        {
            Pocket closestPocket = null;

            foreach (Pocket pocket in FindObjectsOfType<Pocket>())
            {
                if (pocket.isEmpty == false && pocket.canBePoked)
                {
                    if (Vector3.Distance(pocket.transform.position, transform.position) < _range)
                    {
                        if (closestPocket == null
                            || Vector3.Distance(closestPocket.transform.position, transform.position) > Vector3.Distance(pocket.transform.position, transform.position))
                        {
                            closestPocket = pocket;
                        }
                    }

                }
            }
            if (closestPocket)
            {

                StartCoroutine(PokeCooldown());
                closestPocket.Poke(_inventory);
            }
        }
    }

    IEnumerator PokeCooldown()
    {
        _readyToPoke = false;
        yield return new WaitForSeconds(_cooldown);
        _readyToPoke = true;
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.Interact.performed += ctx =>
        {
            Poke();
        };


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
