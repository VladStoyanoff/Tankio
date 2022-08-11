using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Mirror;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] NavMeshAgent agent = null;

    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;
        agent.SetDestination(hit.position);
    }

    #endregion
}
