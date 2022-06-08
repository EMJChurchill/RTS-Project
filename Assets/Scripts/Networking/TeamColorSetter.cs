using Mirror;
using UnityEngine;

public class TeamColorSetter : NetworkBehaviour
{
    [Header("Object Colour")]
    [SerializeField] private Renderer[] colorRenderers = new Renderer[0];

    [Header("Minimap Icon")]
    [SerializeField] private SpriteRenderer[] colorSpriteRenderers = new SpriteRenderer[0];

    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    private Color teamColor = new Color();

    #region Server

    public override void OnStartServer()
    {
        RTSPlayer player = connectionToClient.identity.GetComponent<RTSPlayer>();

        teamColor = player.GetTeamColor();
    }

    #endregion

    #region CLient

    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        //For setting the actual Models of units & buildings to the Player / Team colour.
        foreach(Renderer renderer in colorRenderers)
        {
            renderer.material.SetColor("_BaseColor", newColor);
            
            /* - The below is backup code final release models have the second level mat/renderer as the main - this is not normally the case and will try to avoid it when being made.
            foreach(Material material in renderer.materials)
            {
                material.SetColor("_BaseColor", newColor);
            }
            */
        }

        //For setting the Minimap Sprite icons to the Player / Team colour.
        foreach(SpriteRenderer spriteRenderer in colorSpriteRenderers)
        {
            spriteRenderer.color = newColor;
        }
    }

    #endregion
}