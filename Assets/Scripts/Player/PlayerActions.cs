using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private ReturnFromAction playerDismissActionScript;
    private BoxCollider2D playerBC2D;
    private GameObject playerTile;
    private PlayerAttributes playerStats;

    public delegate void PlayerAction();
    public static PlayerAction OnMoveAction;

    public delegate void AjustSitesColliders(bool value);
    public static AjustSitesColliders TurnSiteCollidersOff;

    public GameObject PlayerTile
    {
        get { return playerTile; }
        set { playerTile = value; }
    }

    private void OnEnable()
    {
        PlayerMovementOption.OnTileSelection += MovePlayer;
    }

    private void OnDisable()
    {
        PlayerMovementOption.OnTileSelection -= MovePlayer;
    }

    private void Awake()
    {
        playerDismissActionScript = GetComponent<ReturnFromAction>();
        playerBC2D = GetComponent<BoxCollider2D>();
        playerStats = GetComponent<PlayerAttributes>();
    }

    private void Start()
    {
        SetPlayerTile();
    }

    // Called inside Unity by the EXCAVATE button.
    public void Excavate()
    {
        if (playerTile != null)
        {
            playerTile.GetComponent<EscavationSiteController>().ExcavateSite(playerStats.ExcavateEnergy);
        }
    }

    // Called inside Unity by the STUDY button.
    public void Study()
    {
        playerDismissActionScript.PlayerInAction = true;
        TurnSiteCollidersOff?.Invoke(true);

        var hits = Physics2D.OverlapBoxAll(transform.position, playerBC2D.size * 1.25f, 180, -1, -3, 3);

        //Debug.Log(hits.Length);

        foreach (var hit in hits)
        {
            var studyScript = hit.GetComponent<StudySiteController>();
            if(studyScript != null)
            {
                studyScript.StudyAction(playerStats.StudyEnergy);
            }
        }
    }

    // Called inside Unity by the MOVE button.
    public void Move()
    {
        playerDismissActionScript.PlayerInAction = true;
        TurnSiteCollidersOff?.Invoke(false);

        foreach (PlayerMovementOption optionScript in GetComponentsInChildren<PlayerMovementOption>())
        {
            optionScript.ActivateMovementOption(playerStats.MoveEnergy);
        }

    }

    private void MovePlayer(Vector3 direction)
    {
        // In order for the next line to work, we must reactivate the scavation site tiles collider
        transform.position = direction;
        TurnSiteCollidersOff?.Invoke(true);
        OnMoveAction?.Invoke(); // Currently the update tiles is register here
        SetPlayerTile();
    }

    private void SetPlayerTile()
    {
        Vector3 rayPos = transform.position;
        rayPos.z = -3;

        Ray ray = new(rayPos, Vector3.forward);
        var hits = Physics2D.GetRayIntersectionAll(ray, 5);

        //Debug.Log(hits.Length);

        foreach (var hit in hits)
        {
            if(hit.transform.CompareTag("SiteTile"))
            {
                playerTile = hit.transform.gameObject;
            }
        }
    }
}
