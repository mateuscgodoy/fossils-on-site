using System;
using UnityEngine;
using UnityEngine.UI;

public class UpdateClockSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite[] clockSprites;

    private ClockController controller;
    private Image clockImage;

    private void Awake()
    {
        clockImage = GetComponent<Image>();
        controller = GetComponent<ClockController>();
    }

    private void Update()
    {
        clockImage.sprite = clockSprites[controller.PlayerEnergy];
    }
}
