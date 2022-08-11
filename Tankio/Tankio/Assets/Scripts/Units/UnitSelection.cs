using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelection : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    List<Unit> selectedUnits = new List<Unit>();

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DeselectAnySelectedUnits();
        }

        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SelectUnits();
        }
    }

    void DeselectAnySelectedUnits()
    {
        foreach (Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Deselect();
        }

        selectedUnits.Clear();
    }

    void SelectUnits()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask)) return;
        if (!hit.collider.TryGetComponent<Unit>(out Unit unit))                   return;
        if (!unit.hasAuthority)                                                   return;

        selectedUnits.Add(unit);

        foreach(Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Select();
        }

    }
}
