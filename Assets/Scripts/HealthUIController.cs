using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public GameObject healthBar;
    private float fillValue;

    void Update(){
        fillValue = (float) GameController.Health;
        fillValue = fillValue / GameController.MaxHealth;
        if(fillValue != healthBar.GetComponent<Image>().fillAmount){
            healthBar.GetComponent<Image>().fillAmount = Mathf.Lerp(healthBar.GetComponent<Image>().fillAmount, fillValue, Time.deltaTime * 2);
        }
    }
}
