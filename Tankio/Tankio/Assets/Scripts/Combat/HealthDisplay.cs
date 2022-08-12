using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] GameObject healthBarParent;
    [SerializeField] Image healthBarImage;

    void Awake()
    {
        health.ClientOnHealthUpdated += Health_ClientOnHealthUpdated;
    }

    void OnDestroy()
    {
        health.ClientOnHealthUpdated -= Health_ClientOnHealthUpdated;
    }

    void Health_ClientOnHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
