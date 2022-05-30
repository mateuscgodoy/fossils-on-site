using UnityEngine;
using UnityEngine.EventSystems;

public class UIAnim : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private float playAnimFor = 2.5f;
    [SerializeField]
    private string paramName;

    private Animator buttonAnimator;
    private float timeCounter = 0;

    private void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        timeCounter = 0;
        buttonAnimator.SetBool(paramName, true);
    }

    private void Update()
    {
        if (!buttonAnimator.GetBool(paramName))
            return;

        timeCounter += Time.deltaTime;

        if (timeCounter > playAnimFor)
            buttonAnimator.SetBool(paramName, false);
    }


}
