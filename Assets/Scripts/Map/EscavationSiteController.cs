using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EscavationSiteController : MonoBehaviour
{
    public enum SiteQualityConditions
    {
        Terrible = 0,
        Bad = 1,
        Regular = 2,
        Good = 3,
        Great = 4
    }

    [SerializeField]
    private SiteQualityConditions fossilDensity, siteCondition;

    private int chanceToFindFossil, chanceOfHavingTrouble;

    [SerializeField]
    private bool showing = false;

    public delegate void ExcavationEvent();
    public static ExcavationEvent OnFossilAcquire;
    public static ExcavationEvent OnSiteTrouble;

    public delegate bool EscavateEnergy(int energyUsed);
    public static EscavateEnergy OnEscavateAction;


    public bool Studied { get { return showing; } set { showing = value; } }
    public SiteQualityConditions FossilDensity { get { return fossilDensity; } private set { } }
    public SiteQualityConditions SiteCondition { get { return siteCondition; } private set { } }

    private void Awake()
    {
        fossilDensity = (SiteQualityConditions)Random.Range((int)SiteQualityConditions.Terrible, (int)SiteQualityConditions.Great + 1);
        siteCondition = (SiteQualityConditions)Random.Range((int)SiteQualityConditions.Terrible, (int)SiteQualityConditions.Great + 1);
        chanceToFindFossil = SetFieldProperty(fossilDensity);
        chanceOfHavingTrouble = SetFieldProperty(siteCondition);
    }

    public void ExcavateSite(int energyUsed)
    {
        if (!(bool)OnEscavateAction?.Invoke(-energyUsed))
        {
            return;
        }


        if (Random.Range(1, 101) <= chanceToFindFossil)
        {
            //Debug.Log($"FOSSIL FOUND.");
            OnFossilAcquire?.Invoke();

            DecreaseSiteQuality();
        }

        if (Random.Range(1, 101) >= chanceOfHavingTrouble)
        {
            OnSiteTrouble?.Invoke();
        }

    }

    private void DecreaseSiteQuality()
    {
        int decreaseSiteQuality = Random.Range(5, 16);

        if (chanceToFindFossil - decreaseSiteQuality < 0)
        {
            chanceToFindFossil = 1;
            //Debug.Log($"New site fossil density: {chanceToFindFossil}");
            fossilDensity = (SiteQualityConditions)((int)chanceToFindFossil / 20);
            return;
        }

        chanceToFindFossil -= decreaseSiteQuality;
        //Debug.Log($"New site fossil density: {chanceToFindFossil}");
        fossilDensity = (SiteQualityConditions)((int)chanceToFindFossil / 20);
    }

    private int SetFieldProperty(SiteQualityConditions quality) =>
        quality switch
        {
            SiteQualityConditions.Terrible => Random.Range(1, 21),
            SiteQualityConditions.Bad => Random.Range(21, 41),
            SiteQualityConditions.Regular => Random.Range(41, 61),
            SiteQualityConditions.Good => Random.Range(61, 81),
            SiteQualityConditions.Great => Random.Range(81, 101),
            _ => 0
        };

}
