using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = new LayerMask(); 
    public List<Unit> SelectedUnits { get; } = new List<Unit>();
    
    private Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    private void Update() {
        if(Mouse.current.leftButton.wasPressedThisFrame) {

            foreach(Unit selectedUnit in SelectedUnits) {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();

        } else if(Mouse.current.leftButton.wasReleasedThisFrame) {
            ClearSelectionArea();
        }
    }

    private void ClearSelectionArea() {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

        if (!unit.hasAuthority) return;

        SelectedUnits.Add(unit);

        foreach (Unit selectedunit in SelectedUnits) {
            selectedunit.Select();
        }
    }
}
