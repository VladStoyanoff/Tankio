using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] Health health;
    [SerializeField] Unit unitPrefab;
    [SerializeField] Transform unitSpawnPoint;
    [SerializeField] TMP_Text remainingUnitsText;
    [SerializeField] Image unitProgressImage;
    [SerializeField] int maxUnitQueue = 5;
    [SerializeField] float spawnMoveRange = 7;
    [SerializeField] float unitSpawnDuration = 5f;

    [SyncVar(hook = nameof(ClientHandleQueuedUnitsUpdated))]
    int queuedUnits;
    [SyncVar]
    float unitTimer;

    float progressImageVelocity;

    void Update()
    {
        if (isServer)
        {
            ProduceUnits();
        }
        
        if (isClient)
        {
            UpdateTimerDisplay();
        }
    }

    #region Server

    public override void OnStartServer()
    {
        health.ServerOnDie += Health_ServerOnDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= Health_ServerOnDie;
    }

    [Server]
    void ProduceUnits()
    {
        if (queuedUnits == 0) return;

        unitTimer += Time.deltaTime;

        if (unitTimer < unitSpawnDuration) return;

        var unitInstance = Instantiate(unitPrefab.gameObject, unitSpawnPoint.position, unitSpawnPoint.rotation);
        NetworkServer.Spawn(unitInstance, connectionToClient);

        var spawnOffset = UnityEngine.Random.insideUnitSphere * spawnMoveRange;
        spawnOffset.y = unitSpawnPoint.position.y;

        var unitMovement = unitInstance.GetComponent<UnitMovement>();
        unitMovement.ServerMove(unitSpawnPoint.position + spawnOffset);

        queuedUnits--;
        unitTimer = 0f;
    }

    [Server]
    void Health_ServerOnDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    void CmdSpawnUnit()
    {
        if (queuedUnits == maxUnitQueue) return;
        var player = connectionToClient.identity.GetComponent<NetworkPlayerTankio>();
        if (player.GetResources() < unitPrefab.GetResourceCost()) return;

        queuedUnits++;

        player.SetResources(player.GetResources() - unitPrefab.GetResourceCost());
    }

    #endregion

    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!hasAuthority) return;
        CmdSpawnUnit();
    }

    void ClientHandleQueuedUnitsUpdated(int oldUnits, int newUnits)
    {
        remainingUnitsText.text = newUnits.ToString();
    }

    void UpdateTimerDisplay()
    {
        var newProgress = unitTimer / unitSpawnDuration;

        if(newProgress < unitProgressImage.fillAmount)
        {
            unitProgressImage.fillAmount = newProgress;
        }
        else
        {
            unitProgressImage.fillAmount = Mathf.SmoothDamp(unitProgressImage.fillAmount, newProgress, ref progressImageVelocity, 0.1f);
        }
    }

    #endregion
}