using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobShaderModifyer
{
    MaterialPropertyBlock _block;
    Renderer[] _renderers;

    public const string AmmountProperty = "_DeformationAmmount";
    public const string DirectionProperty = "_DeformationDirection";

    public float deformationAmmount
    {
        get
        {
            return _deformationAmmount;
        }

        set
        {
            _deformationAmmount = value;

            _block.SetFloat(AmmountProperty, value);

            UpdateRenderers();

        }
    }

    float _deformationAmmount;

    public Vector3 deformationDirection
    {
        get
        {
            return _deformationDirection;
        }

        set
        {
            _deformationDirection = value;

            _block.SetVector(DirectionProperty, value);
            //_block.SetVector(DirectionProperty, _renderers[0].transform.InverseTransformDirection(direction));

            UpdateRenderers();
        }

    }

    Vector3 _deformationDirection;

    public MobShaderModifyer(Renderer renderer)
    {
        _renderers = new Renderer[1];
        _renderers[0] = renderer;

        _block = new MaterialPropertyBlock();

    }

    public MobShaderModifyer(Renderer[] renderers)
    {
        _renderers = renderers;

        _block = new MaterialPropertyBlock();
    }

    void UpdateRenderers()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].SetPropertyBlock(_block);
        }
    }
}
