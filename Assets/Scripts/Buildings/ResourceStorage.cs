using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] int resourceCapacity = 50;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public int ResourceCapacity
    {
        get
        {
            return resourceCapacity;
        }
    }

    #endregion
}
