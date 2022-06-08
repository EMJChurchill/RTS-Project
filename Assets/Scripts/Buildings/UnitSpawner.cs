using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private Health health = null;
    [SerializeField] private Unit unitPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;
    [SerializeField] private TMP_Text unitsInQueueText = null;
    [SerializeField] private Image unitBuildTimerImage = null;
    [SerializeField] int maxUnitQueue = 5;
    [SerializeField] private float spawnMoveRange = 7;
    [SerializeField] private float unitBuildTime = 5f;

    [SyncVar(hook = nameof(CLientHandleQueuedUnitsUpdated))]
    private int queuedUnits;
    [SyncVar]
    private float unitTimer;

    private float progressImageVelocity;

    private void Update()
    {
        if(isServer)
        {
            ProduceUnits();
        }

        if(isClient)
        {
            UpdateTimerDisplay();
        }
    }

    #region Server

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDeath;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDeath;
    }

    [Server]
    private void ProduceUnits()
    {
        if(queuedUnits == 0) { return; }

        unitTimer += Time.deltaTime;

        if(unitTimer < unitBuildTime) { return; }

        GameObject unitInstance = Instantiate(unitPrefab.gameObject, unitSpawnPoint.position, unitSpawnPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);

        Vector3 spawnOffset = Random.insideUnitSphere * spawnMoveRange;
        spawnOffset.y = unitSpawnPoint.position.y;

        UnitMovement unitMovement = unitInstance.GetComponent<UnitMovement>();
        unitMovement.ServerMove(unitSpawnPoint.position + spawnOffset);

        queuedUnits--;
        unitTimer = 0;
    }

    [Server]
    private void ServerHandleDeath()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        if(queuedUnits == maxUnitQueue) { return; }

        RTSPlayer player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        if (player.GetPlayerResources() < unitPrefab.GetResourceCost()) { return; }

        queuedUnits++;

        player.SetPlayerResources(player.GetPlayerResources() - unitPrefab.GetResourceCost());
    }

    #endregion

    #region Client

    private void UpdateTimerDisplay()
    {
        float newProgess = unitTimer / unitBuildTime;

        if(newProgess < unitBuildTimerImage.fillAmount)
        {
            unitBuildTimerImage.fillAmount = newProgess;
        }
        else
        {
            unitBuildTimerImage.fillAmount = Mathf.SmoothDamp(unitBuildTimerImage.fillAmount, newProgess, ref progressImageVelocity, 0.1f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) { return; }

        if(!hasAuthority) { return; }

        CmdSpawnUnit();
    }

    private void CLientHandleQueuedUnitsUpdated(int oldUnits, int newUnits)
    {
        unitsInQueueText.text = newUnits.ToString();
    }

    #endregion

}