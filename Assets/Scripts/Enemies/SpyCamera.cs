using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Sighter))]
public class SpyCamera : Enemy
{

    [Header("References")]
    [SerializeField] Transform _cameraLen;
    [SerializeField] Transform[] _surveillancePoints;

    [Header("Settings")]
    [SerializeField]
    [Range(1f, 10)] float _staringTime = 2;

    [SerializeField]
    [Range(0.1f, 5)] float _rotationLerpFactor = 1;

    [SerializeField]
    Color _spottedColor = Color.red;
    Color _startingColor;

    [SerializeField] UnityEvent _spottedAction;
    [Header("Gizmos")]
    [SerializeField] bool _drawGizmos;

    Sighter _sighter;
    float _elapsedTime = 0;


    int _currentPoint = -1;

    //Vision Variables
    Vector3 _direction;
    float _range;
    float _angle;

    private void Awake()
    {

        if (_surveillancePoints != null && _surveillancePoints.Length > 0 && _cameraLen != null) {
            _cameraLen.transform.forward = (_surveillancePoints[0].position - _cameraLen.position).normalized;
        }
        SetNextPoint();
        _sighter = GetComponent<Sighter>();
        _startingColor = _sighter.SpotLight.color;

        /*
         if (_cameraLen == null)
        {
            Debug.LogWarning("No CameraLen reference in " + name + "'s SpyCamera Component!");
        }
        */
    }

    private void FixedUpdate()
    {
        if (_surveillancePoints != null && _cameraLen != null)
        {

            if (_elapsedTime < _staringTime)
            {

                _cameraLen.transform.forward = Vector3.Lerp(_cameraLen.transform.forward, _direction, Time.deltaTime * _rotationLerpFactor);

                _elapsedTime += Time.deltaTime;

            }
            else
            {
                SetNextPoint();

            }
        }

        if (_sighter.IsAnyTargetOnSight())
        {
            _sighter.SpotLight.color = _spottedColor;

            _spottedAction.Invoke();
        }

        else
        {
            _sighter.SpotLight.color = _startingColor;
        }

    }

    void SetNextPoint()
    {
        if (_surveillancePoints != null && _surveillancePoints.Length != 0)
        {
            _currentPoint++;

            if (_currentPoint == _surveillancePoints.Length)
            {
                _currentPoint = 0;
            }


            _direction = _surveillancePoints[_currentPoint].position - _cameraLen.position;

            _direction.Normalize();

            _elapsedTime = 0;
        }
    }


    [ContextMenu("Create Patrol Point")]
    void CreateSurveillancePoint()
    {
        GameObject go = new GameObject();
        go.name = name + "'s Surveillance Point";
        go.transform.position = transform.position;

        go.transform.parent = transform;
    }

    private void OnDrawGizmos()
    {

        if (_drawGizmos)
        {
            //Draw Surveillance Points
            if (_surveillancePoints != null)
            {
                for (int i = 0; i < _surveillancePoints.Length; i++)
                {
                    if (_surveillancePoints[i] != null)
                    {

                        if (i == 0)
                            Gizmos.color = Color.blue;
                        else
                            Gizmos.color = Color.yellow;

                        Gizmos.DrawCube(_surveillancePoints[i].position, new Vector3(0.5f, 0.5f, 0.5f));

                        if (i + 1 < _surveillancePoints.Length && _surveillancePoints[i + 1] != null)
                        {
                            Gizmos.DrawLine(_surveillancePoints[i].position, _surveillancePoints[i + 1].position);
                        }
                    }

                }


            }

        }


    }



}
