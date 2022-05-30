using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementOption : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] movementSoundClips;

    private int movementEnergyCost = 2;
    private bool registerInput = false;
    private SpriteRenderer childSR;
    private AudioSource movementAS;
    private BoxCollider2D childBC2D;

    public delegate void MovePlayerAction(Vector3 direction);
    public static MovePlayerAction OnTileSelection;

    public delegate bool EnergySpent(int energy);
    public static EnergySpent AfterPlayerMove;


    private void OnEnable()
    {
        ReturnFromAction.OnDismissClick += ActionDismiss;
    }

    private void OnDisable()
    {
        ReturnFromAction.OnDismissClick -= ActionDismiss;
    }

    private void Awake()
    {
        movementAS = GetComponent<AudioSource>();
        childSR = GetComponent<SpriteRenderer>();
        childBC2D = GetComponent<BoxCollider2D>();
    }

    internal void ActivateMovementOption(int energy)
    {
        Vector3 rayPos = transform.position;
        rayPos.z = -3;
        Ray ray = new(rayPos, Vector3.forward);
        var hits = Physics2D.GetRayIntersectionAll(ray, 10);

        if(hits.Length > 0)
        {
            return;
        }

        movementEnergyCost = energy;
        childSR.enabled = true;
        childBC2D.enabled = true;
        registerInput = true;
    }

    private void OnMouseDown()
    {
        if (!registerInput)
        {
            return;
        }

        if ((bool)AfterPlayerMove?.Invoke(-movementEnergyCost))
        {
            OnTileSelection?.Invoke(transform.position);
            movementAS.clip = movementSoundClips[Random.Range(0, movementSoundClips.Length)];
            movementAS.Stop();
            movementAS.Play();
        }
    }

    private void ActionDismiss()
    {
        childSR.enabled = false;
        childBC2D.enabled = false;
        registerInput = false;
    }
}
