using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTilesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject escavationSitePrefab;

    private List<Transform> playerTilesTransforms = new();
    private List<GameObject> escavationSiteTiles = new();

    private PlayerActions playerActionsScript;

    private void OnEnable()
    {
        PlayerActions.OnMoveAction += UpdateSiteTiles;
    }

    private void OnDisable()
    {
        PlayerActions.OnMoveAction -= UpdateSiteTiles;
    }

    private void Start()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");

        foreach (Transform siteTransform in playerGO.GetComponentsInChildren<Transform>())
        {
            if (siteTransform.CompareTag("Ignore"))
            {
                continue;
            }
            GameObject tile = Instantiate(escavationSitePrefab, siteTransform.position, Quaternion.identity, transform);

            playerTilesTransforms.Add(siteTransform);
            escavationSiteTiles.Add(tile);
        }
    }

    private void UpdateSiteTiles()
    {
        foreach (Transform siteTransform in playerTilesTransforms)
        {
            Vector3 rayPos = siteTransform.position;
            rayPos.z = -3;

            Ray ray = new Ray(rayPos, Vector3.forward);
            var hits = Physics2D.GetRayIntersectionAll(ray, 10);

            if (hits.Length == 0)
            {
                GameObject tile = Instantiate(escavationSitePrefab, siteTransform.position, Quaternion.identity, transform);
                escavationSiteTiles.Add(tile);
            }
        }
    }
}
