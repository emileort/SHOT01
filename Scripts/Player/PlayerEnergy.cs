using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;

    [SerializeField] float OverdriveInterval = 0.1f;

    bool available = true;

    public const int MAX = 100;

    public const int PERCENT = 1;

    int energy;

    WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(OverdriveInterval);
    }
    
    void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    void Start()
    {
        energyBar.Initialize(energy, MAX);
        
    }

    public void Obtain(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;

        // engergy += value;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);

        if (energy == 0 && !available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }

   

    public bool IsEnough(int value) => energy >= value;

    void PlayerOverdriveOn()
    {
        available = false ;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }

    void PlayerOverdriveOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    IEnumerator KeepUsingCoroutine()
    {
        while(gameObject.activeSelf && energy > 0)
        {
            // 幾秒要消耗一次能量
            yield return waitForOverdriveInterval;

            // 每過幾秒消耗多少%能量
            Use(PERCENT);
        }
    }

}