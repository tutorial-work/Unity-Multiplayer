using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeamColorSetter : NetworkBehaviour
{
    [SerializeField] Renderer[] colorRenderers = new Renderer[0];

    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    private Color teamColor = new Color();

    #region Server Functions

    public override void OnStartServer()
    {
        RTSPlayer player = connectionToClient.identity.GetComponent<RTSPlayer>();

        teamColor = player.TeamColor;
    }

    #endregion

    #region Client Functions

    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        foreach(Renderer renderer in colorRenderers)
        {
            renderer.material.SetColor("_BaseColor", newColor);
        }
    }

    #endregion

}
