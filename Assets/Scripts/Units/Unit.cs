using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] UnityEvent onSelected = null;
    [SerializeField] UnityEvent onDeselected = null;

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions
    
    public void Select()
    {
        if (!hasAuthority) return;

        onSelected?.Invoke(); // ? mark checks if event is null "it's a safety check"
    }

    public void Deselect()
    {
        if (!hasAuthority) return;

        onDeselected?.Invoke(); // ? mark checks if event is null "it's a safety check"
    }

    #endregion

}
