using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public NavMeshAgent navigator;
    public Transform patrolCenter;
    public Transform target;
    public float patrolRange;
    public float alertRadius = 10f;

    private LineRenderer radiusIndicator;
    public int circlePoints = 50;

    void Start()
    {
        navigator = GetComponent<NavMeshAgent>();
        radiusIndicator = GetComponent<LineRenderer>();
        radiusIndicator.positionCount = circlePoints + 1;
        radiusIndicator.useWorldSpace = false;
        radiusIndicator.startWidth = 0.1f;
        radiusIndicator.endWidth = 0.1f;
        radiusIndicator.startColor = Color.green;
        radiusIndicator.endColor = Color.green;

        RenderAlertRadius();
    }

    void Update()
    {
        float targetDistance = Vector3.Distance(target.position, transform.position);

        if (targetDistance <= alertRadius)
        {
            PursueTarget();
        }
        else if (navigator.remainingDistance <= navigator.stoppingDistance)
        {
            Vector3 randomPosition;
            if (GetRandomPoint(patrolCenter.position, patrolRange, out randomPosition))
            {
                navigator.SetDestination(randomPosition);
            }
        }

        RenderAlertRadius();
    }

    bool GetRandomPoint(Vector3 center, float radius, out Vector3 position)
    {
        Vector3 candidatePoint = center + Random.insideUnitSphere * patrolRange;
        if (NavMesh.SamplePosition(candidatePoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            position = hit.position;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    void PursueTarget()
    {
        navigator.destination = target.position;
    }

    void RenderAlertRadius()
    {
        float angleStep = 360f / circlePoints;
        Vector3[] circlePositions = new Vector3[circlePoints + 1];

        for (int i = 0; i <= circlePoints; i++)
        {
            float radian = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Sin(radian) * alertRadius;
            float z = Mathf.Cos(radian) * alertRadius;
            circlePositions[i] = new Vector3(x, 0, z);
        }

        radiusIndicator.SetPositions(circlePositions);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        if (patrolCenter != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(patrolCenter.position, patrolRange);
        }
    }
}
