using Mirror;
using TMPro;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text playerResourcesText = null;

    private RTSPlayer player;

    private void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        ClientHandlePlayerResourcesUpdated(player.GetPlayerResources());

        player.ClientOnPlayerResourcesUpdated += ClientHandlePlayerResourcesUpdated;
    }

    private void OnDestroy()
    {
        player.ClientOnPlayerResourcesUpdated -= ClientHandlePlayerResourcesUpdated;
    }

    private void ClientHandlePlayerResourcesUpdated(int resources)
    {
        playerResourcesText.text = $"Resources: {resources}";
    }
}