using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Mirror;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Building building;
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text priceText;
    [SerializeField] LayerMask floorMask = new LayerMask();

    UnitSelection unitSelection;
    NetworkPlayerTankio player;
    GameObject buildingPreviewInstance;
    Renderer buildingRendererInstance;
    BoxCollider buildingCollider;

    void Start()
    {
        unitSelection = FindObjectOfType<UnitSelection>();
        iconImage.overrideSprite = building.GetIcon();
        priceText.text = building.GetPrice().ToString();
        player = NetworkClient.connection.identity.GetComponent<NetworkPlayerTankio>();
        buildingCollider = building.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (buildingPreviewInstance == null) return;

        UpdateBuildingPreview();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (player.GetResources() < building.GetPrice()) return;

        unitSelection.ActivateBoolIsPlacingABuilding();

        buildingPreviewInstance = Instantiate(building.GetBuildingPreview());
        buildingRendererInstance = buildingPreviewInstance.GetComponentInChildren<Renderer>();

        buildingPreviewInstance.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buildingPreviewInstance == null) return;

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, floorMask))
        {
            player.CmdTryPlaceBuilding(building.GetId(), hit.point);
        }

        Destroy(buildingPreviewInstance);

        unitSelection.DisactivateBoolIsPlacingABuilding();
    }

    void UpdateBuildingPreview()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, floorMask)) return;

        buildingPreviewInstance.transform.position = hit.point;

        if (!buildingPreviewInstance.activeSelf)
        {
            buildingPreviewInstance.SetActive(true);
        }

        var color = Color.green;

        buildingRendererInstance.material.SetColor("_BaseColor", color);

        //player.CanPlaceBuilding(buildingCollider, hit.point) ? Color.green : Color.red;
    }
}
