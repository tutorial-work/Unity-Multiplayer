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

    public bool HasWaypoint { get; [Server] set; }

    public Vector3 MyWaypoint { get; [Server] set; }

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

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
                SetMyWaypoint(target.transform.position);
            }
            else if(agent.hasPath)
            {
                // stop
                ClearMyWaypoint();
            }

            return;
        }

        if (!agent.hasPath) return;

        if (agent.remainingDistance > agent.stoppingDistance) return;

        ClearMyWaypoint();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        ServerMove(position);
    }

    [Server]
    public void ServerMove(Vector3 position)
    {
        targeter.ClearTarget();

        float leeway = 1f; // distance away from navmesh that still counts

        // exit if invalid position
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, leeway, NavMesh.AllAreas))
            return;

        SetMyWaypoint(position);
    }

    [Server]
    public void SetMyWaypoint(Vector3 position)
    {
        HasWaypoint = true;
        MyWaypoint = position;
        agent.SetDestination(position);
    }

    [Server]
    public void ClearMyWaypoint()
    {
        HasWaypoint = false;
        agent.ResetPath();
    }

    [Server]
    private void ServerHandleGameOver()
    {
        agent.ResetPath();
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    #endregion
}
