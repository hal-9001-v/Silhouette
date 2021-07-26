using System;
using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _entryA;
    [SerializeField] Transform _entryB;

    [Header("Gizmos")]
    [SerializeField] [Range(0.01f, 1)] float _entryRadius;

    CharacterMovement _player;

    private void Awake()
    {
        _player = FindObjectOfType<CharacterMovement>();
    }

    public void EnterPipeInA()
    {
        Debug.Log("Enter A");

        if (_player != null)
        {
            _player.MovePlayerToPositionInPipe(_entryB.position);
        }
    }

    public void EnterPipeInB()
    {
        Debug.Log("Enter B");

        if (_player != null)
        {
            _player.MovePlayerToPositionInPipe(_entryA.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (_entryA != null)
        {
            Gizmos.DrawSphere(_entryA.position, _entryRadius * 0.5f);
            Gizmos.DrawWireSphere(_entryA.position, _entryRadius);
        }

        if (_entryB != null)
        {
            Gizmos.DrawSphere(_entryB.position, _entryRadius * 0.5f);
            Gizmos.DrawWireSphere(_entryB.position, _entryRadius);
        }
    }

    IEnumerator CountdownToAction(float time, Action action) {

        yield return new WaitForSeconds(time);

        action.Invoke();
    }

}