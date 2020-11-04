using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    /********** MARK: Class Variables **********/

    [SerializeField] Health health = null;
    [SerializeField] GameObject healthBarParent = null;
    [SerializeField] Image healthBarImage = null;

    /********** MARK: Properties **********/
    #region Properties

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    private void Awake()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true);
    }

    private void OnMouseExit()
    {
        healthBarParent.SetActive(false);
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }

    #endregion
}
