using System.Collections;
using UnityEngine;

public class Pidgeon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator _animator;
    [SerializeField] Transform _flyingDestination;
    [SerializeField] Renderer _renderer;


    [Header("Settings")]
    [Range(1, 20)]
    [SerializeField] float _flyingAwayTime = 5;

    [Range(1, 20)]
    [SerializeField] float _flyingSpeed = 20;

    //Animator Variables
    const string _flyTrigger = "Fly";


    public void StartFlying() {
        if (_animator != null) {
            _animator.SetTrigger(_flyTrigger);
        }

        StartCoroutine(FlyAway());
            
    }

    IEnumerator FlyAway() {

        Vector3 destination = Vector3.zero;
        if (_flyingDestination != null)
        {
            destination = _flyingDestination.position;

        }
        else {


            var pidgeonDestinations = FindObjectsOfType<PidgeonDestination>();

            if (pidgeonDestinations != null)
            {
                destination = pidgeonDestinations[Random.Range(0, pidgeonDestinations.Length)].transform.position;
            }
            else {

                Debug.Log("No destination in pidgeon " + name + "!");

                StopCoroutine(FlyAway());

            }

        }

        float elapsedTime = 0;
        Vector3 startPosition = transform.position;

        transform.LookAt(destination);

        float lerpDuration = Vector3.Distance(transform.position, destination) /_flyingSpeed;

        while (elapsedTime < _flyingAwayTime) {

            transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / lerpDuration);

            elapsedTime += Time.deltaTime;

            yield return null;

        }

        DisablePidgeon();
        
        yield return null;
    }

    void DisablePidgeon() {
        enabled = false;

        if (_animator != null)
        {
            _animator.enabled = false;
        }

        if (_renderer != null)
        {
            _renderer.enabled = false;
        }

    }


}
