using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchTracker : MonoBehaviour
{

    List<StretchZone> zones;

    CharacterAnimationCommand _player;
    // Start is called before the first frame update
    void Start()
    {
        zones = new List<StretchZone>();

        foreach (StretchZone zone in FindObjectsOfType<StretchZone>())
        {
            zones.Add(zone);
        }

        _player = FindObjectOfType<CharacterAnimationCommand>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_player != null)
        {
            foreach (StretchZone zone in zones)
            {
                if (other.gameObject == zone.gameObject)
                {
                    _player.Stretch(zone.direction);

                    return;
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player != null)
        {
            foreach (StretchZone zone in zones)
            {
                if (other.gameObject == zone.gameObject)
                {
                    _player.Unstretch();

                    return;
                }
            }

        }
    }
}
