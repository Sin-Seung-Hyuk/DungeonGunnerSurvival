using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;


    public void EnableHealthBar()
    {
        gameObject.SetActive(true);
    }
    public void DisableHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void SetHealthBar(float ratio)
    {
        healthBar.transform.localScale = new Vector3(ratio, 1f, 1f);
    }
}
