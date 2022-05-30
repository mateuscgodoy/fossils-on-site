using UnityEngine;
using TMPro;
using System;

public class DisplaySiteInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject studiedLandPanel;
    [SerializeField]
    private TextMeshProUGUI fossilDensityText, siteDangerText;

    private EscavationSiteController siteControllerScript;

    private void Awake()
    {
        siteControllerScript = GetComponent<EscavationSiteController>();
    }

    private void OnMouseEnter()
    {
        if (siteControllerScript.Studied)
        {
            UpdateTextInfo();
            studiedLandPanel.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        studiedLandPanel.SetActive(false);
    }

    private void UpdateTextInfo()
    {
        fossilDensityText.text = siteControllerScript.FossilDensity.ToString();
        fossilDensityText.color = SetTextColor(siteControllerScript.FossilDensity);

        siteDangerText.text = siteControllerScript.SiteCondition.ToString();
        siteDangerText.color = SetTextColor(siteControllerScript.SiteCondition);
    }

    private Color SetTextColor(EscavationSiteController.SiteQualityConditions item) =>
        item switch
        {
            EscavationSiteController.SiteQualityConditions.Terrible => Color.red,
            EscavationSiteController.SiteQualityConditions.Bad => new Color(1, 0.5f, 0.5f),
            EscavationSiteController.SiteQualityConditions.Regular => Color.yellow,
            EscavationSiteController.SiteQualityConditions.Good => new Color(0.5f, 1, 0.5f),
            EscavationSiteController.SiteQualityConditions.Great => Color.green,
            _ => Color.white
        };
}
