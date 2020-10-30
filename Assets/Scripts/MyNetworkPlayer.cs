using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = new Color();

    #region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName; // this auto calls HandleDisplayNameUpdated
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor; // this auto calls HandleDisplayColorUpdated
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; }

        RpcLogNewName(newDisplayName);

        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client

    public void HandleDisplayNameUpdated(string oldColor, string newColor)
    {
        displayNameText.text = newColor;
    }
    
    public void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    [ContextMenu("Set My Name")]
    public void SetMyName()
    {
        CmdSetDisplayName("My New Name");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion

}
