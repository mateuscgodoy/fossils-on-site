using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFromAction : MonoBehaviour
{
    private bool inAction = false;
    private BoxCollider2D playerBC;

    private void Awake() 
    {
        playerBC = GetComponent<BoxCollider2D>();
    }

    public bool PlayerInAction
    {
        get { return inAction; }
        set { inAction = value; }
    }

    public delegate void CancelAction();
    public static CancelAction OnDismissClick;

    private void Update()
    {
        if (!inAction)
        {
            return;
        }

            
        if (Input.GetMouseButtonDown(0))
        {
            inAction = false;
            OnDismissClick?.Invoke();
            //Debug.Log("Dismiss");
        }
    }
}
