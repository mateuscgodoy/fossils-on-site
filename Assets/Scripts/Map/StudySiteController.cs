using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudySiteController : MonoBehaviour
{
    [SerializeField]
    private Color defaultColor, studiedColor, unknownColor;
   
    private int studyActionCost = 1;
    private SpriteRenderer siteSR;
    private BoxCollider2D siteBC2D;
    private EscavationSiteController siteScript;
    private AudioSource studyAS;
    private bool registerMouseInput = false;

    public delegate bool InformEnergy(int energyUsed);
    public static InformEnergy OnSiteStudy;


    private void OnEnable()
    {
        PlayerActions.TurnSiteCollidersOff += TurnColliderOnAndOff;
        ReturnFromAction.OnDismissClick += DismissAction;
    }

    private void OnDisable()
    {
        PlayerActions.TurnSiteCollidersOff -= TurnColliderOnAndOff;
        ReturnFromAction.OnDismissClick -= DismissAction;
    }

    private void Awake()
    {
        studyAS = GetComponent<AudioSource>();
        siteScript = GetComponent<EscavationSiteController>();
        siteBC2D = GetComponent<BoxCollider2D>();
        siteSR = GetComponent<SpriteRenderer>();
        siteSR.color = defaultColor;
    }

    internal void StudyAction(int energy)
    {
        if (siteScript.Studied)
        {
            siteSR.color = studiedColor;
            return;
        }

        studyActionCost = energy;
        siteSR.color = unknownColor;
        registerMouseInput = true;
    }

    private void OnMouseDown()
    {
        if (!registerMouseInput)
        {
            return;
        }

        if (!(bool)OnSiteStudy?.Invoke(-studyActionCost))
        {
            return;
        }
        studyAS.Play();
        siteScript.Studied = true;
        registerMouseInput = false;
    }

    private void DismissAction()
    {
        siteSR.color = siteScript.Studied == true ? studiedColor : defaultColor;
        siteBC2D.enabled = true;
        registerMouseInput = false;
    }

    private void TurnColliderOnAndOff(bool value)
    {
        siteBC2D.enabled = value;
    }
}
