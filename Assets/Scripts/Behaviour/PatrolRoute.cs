using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [Header("References")]
    [SerializeField] HeavyMob _mob;
    public Transform[] patrolPoints;

    private void Start()
    {
        //RequestMob();

        CreateMob();
    }

    public void CreateMob() {

        var mob = Instantiate(_mob.gameObject).GetComponent<HeavyMob>();
        mob.transform.position = patrolPoints[0].position;
        mob.GetComponent<Navigator>().WarpNavMesh();
        
        mob.SetPatrolRoute(this);
        
        
    }

    public void RequestMob()
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

    [ContextMenu("Update Patrol Points")]
    private void UpdatePatrolPoints()
    {
        List<Transform> points = new List<Transform>();

        int counter = 1;
        foreach (Transform point in GetComponentsInChildren<Transform>()) {
            if (point != transform) {
                points.Add(point);
                
                point.name = "Patrol Point " + counter.ToString();
                counter++;
            }
        }


        patrolPoints = points.ToArray();
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
