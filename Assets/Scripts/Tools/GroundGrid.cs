using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class GroundGrid : MonoBehaviour
{

    [Header("References")]
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] MeshFilter _filter;

    [Header("Settings")]
    const int _maxSize = 1000;
    [Space(3)]
    [Range(0.1f, _maxSize)]
    [SerializeField] float _xGridSize = 1;

    [Space(3)]
    [Range(0.1f, _maxSize)]
    [SerializeField] float _zGridSize = 1;




    private void Update()
    {
        if (_renderer != null && _filter != null) {
            float xOffset = _renderer.bounds.size.x * 0.5f;
            float zOffset = _renderer.bounds.size.z * 0.5f;

            for (int i = 0; i < _xGridSize; i++) {
                for (int j = 0; j < _zGridSize; j++)
                {
                    var position = new Vector3();

                    position.x = i * xOffset;
                    position.y = transform.position.y;
                    position.x = j * zOffset;


                    Graphics.DrawMesh(_filter.sharedMesh, position, Quaternion.identity,_renderer.sharedMaterial, 0);
                }

            }


            
        }
        


    }

}
