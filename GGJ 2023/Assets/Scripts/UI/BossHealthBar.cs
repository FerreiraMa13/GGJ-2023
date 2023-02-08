using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public Enemy enemy;
    private void Awake()
    {
        slider.value = 100;
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void Update()
    {
        if(enemy.hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
        slider.value = enemy.hp;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
