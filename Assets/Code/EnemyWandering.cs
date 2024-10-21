using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWandering : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform centrePoint;
    public Transform player;
    public float range;
    public float detectionRadius = 10f;

    // A�adir referencia al LineRenderer
    private LineRenderer lineRenderer;
    public int segments = 50; // Cuantos puntos tendr� el c�rculo

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Obtener el LineRenderer del objeto
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;

        // Configurar propiedades del LineRenderer
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        // Dibujar el c�rculo inicial
        DrawDetectionRadius();
    }

    void Update()
    {
        float distancePlayer = Vector3.Distance(player.position, transform.position);

        if (distancePlayer <= detectionRadius)
        {
            FollowPlayer();
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 punto;
            if (TryGetRandomPoint(centrePoint.position, range, out punto))
            {
                agent.SetDestination(punto);
            }
        }

        // Actualizar el c�rculo en caso de que detectionRadius cambie
        DrawDetectionRadius();
    }

    bool TryGetRandomPoint(Vector3 center, float radius, out Vector3 point)
    {
        Vector3 potentialPoint = center + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
        {
            point = navHit.position;
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    void FollowPlayer()
    {
        agent.destination = player.position;
    }

    // M�todo para dibujar el c�rculo del radio de detecci�n
    void DrawDetectionRadius()
    {
        float angle = 360f / segments;
        Vector3[] positions = new Vector3[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * (i * angle);
            float x = Mathf.Sin(rad) * detectionRadius;
            float z = Mathf.Cos(rad) * detectionRadius;
            positions[i] = new Vector3(x, 0, z);
        }

        lineRenderer.SetPositions(positions);
    }

    // M�todo para dibujar el Gizmo en la escena (Scene View)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (centrePoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(centrePoint.position, range);
        }
    }
}
