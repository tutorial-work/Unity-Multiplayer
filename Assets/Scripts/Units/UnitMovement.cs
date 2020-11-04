using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class UnitMovement : NetworkBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Targeter targeter = null;
    [SerializeField] float chaseRange = 10f;

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.Target;

        if (target != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude
                > chaseRange * chaseRange)
            {
                // chase
                agent.SetDestination(target.transform.position);
            }
            else if(agent.hasPath)
            {
                // stop
                agent.ResetPath();
            }

            return;
        }

        if (!agent.hasPath) return;

        if (agent.remainingDistance > agent.stoppingDistance) return;

        agent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        targeter.ClearTarget();

        float leeway = 1f; // distance away from navmesh that still counts

        // exit if invalid position
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, leeway, NavMesh.AllAreas))
            return;

        agent.SetDestination(position);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions
        
    #endregion
}
