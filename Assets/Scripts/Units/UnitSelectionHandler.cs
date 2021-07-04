using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Mirror;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform unitSelectionArea = null;
    [SerializeField] private LayerMask layerMask = new LayerMask(); 
    public List<Unit> SelectedUnits { get; } = new List<Unit>();
    
    private Camera cam;
    private RTSPlayer player;
    private Vector2 startMousePos;

    private void Start() {
        cam = Camera.main;
    }

    private void Update() {
        if(player == null) {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if(Mouse.current.leftButton.wasPressedThisFrame) {
            StartSelectionArea();
        } 
        else if(Mouse.current.leftButton.wasReleasedThisFrame) {
            ClearSelectionArea();
        } 
        else if(Mouse.current.leftButton.isPressed) {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea() {
        foreach (Unit selectedUnit in SelectedUnits) {
            selectedUnit.Deselect();
        }

        SelectedUnits.Clear();

        unitSelectionArea.gameObject.SetActive(true);

        startMousePos = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }

    private void UpdateSelectionArea() {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float areaWidth = mousePos.x - startMousePos.x;
        float areadHeight = mousePos.y - startMousePos.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areadHeight));
        unitSelectionArea.anchoredPosition = startMousePos + new Vector2((areaWidth / 2), (areadHeight / 2));
    }

    private void ClearSelectionArea() {
        unitSelectionArea.gameObject.SetActive(false);
        
        if(unitSelectionArea.sizeDelta.magnitude == 0) {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

            if (!unit.hasAuthority) return;

            SelectedUnits.Add(unit);

            foreach (Unit selectedunit in SelectedUnits) {
                selectedunit.Select();
            }

            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (Unit unit in player.GetMyUnits()) {
            
            Vector2 screenPos = cam.WorldToScreenPoint(unit.transform.position);
            
            if(screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y) {
                SelectedUnits.Add(unit);
                unit.Select();
            }
        }
    
    }
}
