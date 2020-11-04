using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{
    /********** MARK: Class Variables **********/

    private Targetable target;

    /********** MARK: Properties **********/
    #region Properties

    public Targetable Target
    {
        get
        {
            return target;
        }
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget)) return;

        target = newTarget;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    #endregion
}
