using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isForward;

    void Start()
    {
        // Aseg�rate de que haya al menos dos waypoints
        if (waypoints.Length < 2)
        {
            Debug.LogError("Se necesitan al menos 2 waypoints para patrullar.");
            return;
        }

        agent = GetComponent<NavMeshAgent>();

        // Seleccionar aleatoriamente el punto de inicio
        currentWaypointIndex = Random.Range(0, waypoints.Length);

        // Establecer la direcci�n de patrullaje de forma aleatoria
        isForward = Random.value > 0.5f;

        // Establecer el destino inicial
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        // Si el agente ha llegado al waypoint actual
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Actualizar el waypoint seg�n la direcci�n
            UpdateWaypoint();

            // Establecer la nueva posici�n de destino
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Actualizar el �ndice del waypoint seg�n la direcci�n
    void UpdateWaypoint()
    {
        if (isForward)
        {
            currentWaypointIndex++;
            // Si llegamos al final de los waypoints, invertir la direcci�n
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = waypoints.Length - 1;
                isForward = false;
            }
        }
        else
        {
            currentWaypointIndex--;
            // Si llegamos al primer waypoint, invertir la direcci�n
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 0;
                isForward = true;
            }
        }
    }
}
