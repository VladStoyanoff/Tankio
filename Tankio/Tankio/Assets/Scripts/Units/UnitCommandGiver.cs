using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] UnitSelection unitSelection;
    [SerializeField] LayerMask layerMask;

    void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask)) return;

        TryMove(hit.point);
    }

    void TryMove(Vector3 point)
    {
         foreach(Unit unit in unitSelection.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point);
        }
    }
}
