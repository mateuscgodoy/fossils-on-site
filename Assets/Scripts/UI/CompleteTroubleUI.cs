using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteTroubleUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] troubleUIIcons;

    private void OnEnable()
    {
        ClockController.OnNewTroubleAddition += ActivateUIElement;
    }

    private void OnDisable()
    {
        ClockController.OnNewTroubleAddition -= ActivateUIElement;
    }

    private void ActivateUIElement(Dictionary<string, int> troubleEffectsDict)
    {
        int i = 0;

        foreach (var item in troubleEffectsDict)
        {
            //if (item.Value == 0 && troubleUIIcons[i].activeInHierarchy)
            //{
            //    troubleUIIcons[i].SetActive(false);
            //    i++;
            //    continue;
            //}

            troubleUIIcons[i].GetComponent<DisplayTooltip>().SetUIElement(item.Key, item.Value);
            troubleUIIcons[i].SetActive(true);

            i++;
        }

        for (int j = i; j < troubleUIIcons.Length; j++)
        {
            troubleUIIcons[j].SetActive(false);
        }
    }
}
