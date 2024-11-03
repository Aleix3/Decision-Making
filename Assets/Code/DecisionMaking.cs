using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    public Transform guard;
    public GameObject loot;
    public float closeDistance = 10f;
    Moves moves;  // Cambiado de Actions a Moves
    UnityEngine.AI.NavMeshAgent navAgent;

    private WaitForSeconds delay = new WaitForSeconds(0.05f);
    delegate IEnumerator Behavior();
    private Behavior currentBehavior;

    IEnumerator Start()
    {
        moves = gameObject.GetComponent<Moves>();
        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        yield return delay;

        currentBehavior = Patrol;

        while (enabled)
            yield return StartCoroutine(currentBehavior());
    }

    IEnumerator Patrol()
    {
        Debug.Log("Patrolling");

        while (Vector3.Distance(guard.position, loot.transform.position) < closeDistance)
        {
            moves.Wander();
            yield return delay;
        };

        currentBehavior = Approach;
    }

    IEnumerator Approach()
    {
        Debug.Log("Approaching");

        navAgent.speed = 8f;
        moves.Seek(loot.transform.position);

        bool acquired = false;
        while (Vector3.Distance(guard.position, loot.transform.position) > closeDistance)
        {
            if (Vector3.Distance(loot.transform.position, transform.position) < 2f)
            {
                acquired = true;
                break;
            };
            yield return delay;
        };

        if (acquired)
        {
            loot.GetComponent<Renderer>().enabled = false;
            Debug.Log("Acquired");
            currentBehavior = Hide;
        }
        else
        {
            navAgent.speed = 8f;
            currentBehavior = Patrol;
        }
    }

    IEnumerator Hide()
    {
        Debug.Log("Hiding");

        while (true)
        {
            moves.Hide();
            yield return delay;
        };
    }
}
