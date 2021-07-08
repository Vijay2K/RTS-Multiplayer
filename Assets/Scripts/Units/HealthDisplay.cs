using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private GameObject healthBarParent = null;
    [SerializeField] private Image healthFillImage = null;

    private void Awake() {
        health.ClientOnHealthUpdated += HandleHealthBar;
    }

    private void OnDestroy() {
        health.ClientOnHealthUpdated -= HandleHealthBar;
    }

    private void OnMouseEnter() {
        healthBarParent.SetActive(true);
    }

    private void OnMouseExit() {
        healthBarParent.SetActive(false);
    }

    private void HandleHealthBar(int currentHealth, int maxHealth) {
        healthFillImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
