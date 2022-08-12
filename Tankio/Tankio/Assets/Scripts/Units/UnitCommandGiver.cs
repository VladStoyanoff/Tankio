using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] UnitSelection unitSelection;
    [SerializeField] LayerMask layerMask;

    void Start()
    {
        GameOverHandler.ClientOnGameOver += GameOverHandler_ClientOnGameOver;
    }

    void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask)) return;

        if(hit.collider.TryGetComponent<Targetable>(out Targetable target))
        {
            if (target.hasAuthority)
            {
                TryMove(hit.point);
                return;
            }

            TryTarget(target);
            return;
        }
        TryMove(hit.point);
    }

    void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= GameOverHandler_ClientOnGameOver;
    }

    void TryMove(Vector3 point)
    {
         foreach(Unit unit in unitSelection.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point);
        }
    }

    void TryTarget(Targetable target)
    {               
        foreach (Unit unit in unitSelection.SelectedUnits)
        {
            unit.GetTargeter().CmdSetTarget(target.gameObject);
        }
    }

    void GameOverHandler_ClientOnGameOver(string winnerName)
    {
        enabled = false;
    }
}
