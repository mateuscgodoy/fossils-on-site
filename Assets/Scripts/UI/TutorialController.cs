using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject firstTutorialPage, secondTutorialPage;
    [SerializeField]
    private bool shouldOscilate = false;
    [SerializeField]
    private float velocity = 7.5f, amplitude = 5.0f;

    private Vector3 startingPos;

    private void Awake()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        if (!shouldOscilate)
            return;

        transform.position = startingPos + (amplitude * Mathf.Sin(Mathf.PingPong(Time.time * velocity, 360)) * Vector3.left);
    }

    public void SwapTutorialPage()
    {
        firstTutorialPage.SetActive(!firstTutorialPage.activeInHierarchy);
        secondTutorialPage.SetActive(!secondTutorialPage.activeInHierarchy);
    }

    public void DismissTutorial()
    {
        secondTutorialPage.SetActive(false);
    }
}
