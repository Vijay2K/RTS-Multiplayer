using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    private void Update() {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;
        
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        TryMove(hit.point);
    }

    private void TryMove(Vector3 pos) {
        foreach(Unit unit in unitSelectionHandler.SelectedUnits) {
            unit.GetUnitMovement().CmdMove(pos);
        }
    }
}
