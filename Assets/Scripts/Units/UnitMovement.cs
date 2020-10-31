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

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    [Command]
    public void CmdMove(Vector3 position)
    {
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
