using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : InputComponent
{
    [Header("Settings")]

    [SerializeField]
    [Range(0, 1)] float _defaultVolume;

    AudioSource _source;

    public override void SetInput(PlatformMap input)
    {
        input.Character.Music.performed += ctx =>
        {
            if (ctx.ReadValue<float>() < 0)
            {
                DownVolume();
            }
            else if (ctx.ReadValue<float>() > 0)
            {
                UpVolume();
            }

        };

        input.Character.Exit.performed +=ctx => {
            Application.Quit();
        
        };

    }

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();

        if (_source != null)
        {
            _source.volume = _defaultVolume;
        }
    }

    void UpVolume()
    {
        if (_source != null && _source.volume < 1)
        {
            _source.volume += 0.1f;
        }
    }

    void DownVolume()
    {
        if (_source != null && _source.volume > 0)
        {
            _source.volume -= 0.1f;
        }
    }


}
