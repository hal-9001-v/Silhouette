using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public Transform[] patrolPoints;

    private void Start()
    {
        GetMob();
    }

    public void GetMob()
    {
        foreach (HeavyMob mob in FindObjectsOfType<HeavyMob>())
        {
            if (mob.avaliableForPatrol)
            {
                mob.SetPatrolRoute(this);

                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints != null)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {

                    if (i == 0)
                        Gizmos.color = Color.blue;
                    else
                        Gizmos.color = Color.green;

                    Gizmos.DrawCube(patrolPoints[i].position, new Vector3(1f, 1f, 1f));

                    if (i + 1 < patrolPoints.Length && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                    else
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
                    }
                }

            }


        }

    }

}
