using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] GameObject unitPrefab = null;
    [SerializeField] Transform spawnPoint = null;

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject instance = Instantiate(unitPrefab, spawnPoint.position, spawnPoint.rotation);

        // spawns the instance across the network and gives authority to the client that spawned it
        NetworkServer.Spawn(instance, connectionToClient); 
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Client Functions

    public void OnPointerClick(PointerEventData eventData)
    {
        DrawLine();

        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (!hasAuthority) return;

        CmdSpawnUnit();
    }

    #endregion

    /********** MARK: Debug **********/
    #region Debug

    private void DrawLine()
    {
        Camera mainCamera = FindObjectOfType<Camera>(); // "there can be only one"
        Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.white, 3f);
        }
    }

    #endregion
}
