/* BuildingPanelController
 * Jone Sainz Egea
 * 19/01/2026
 * 
 * Controlador con los métodos de los botones del panel de construcción.
 * 
 * v1 -19/01/2026- OnBuildClick, OnSolarClick.
 */
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIController : MonoBehaviour
{
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private GameObject buildHousePanel;
    [SerializeField] private GameObject buildSolarPanel;
    [SerializeField] private GameObject buildRotatePanel;
    [SerializeField] private GameObject buildRemovePanel;

    [SerializeField] private Image buildingMainImage;
    [SerializeField] private Sprite[] selectedOptionImage = new Sprite[5];

    [SerializeField] private Image buildingOptionsDonut;
    [SerializeField] private Sprite[] selectedOption = new Sprite[5];

    private bool panelIsActive = false;

    private void Start()
    {
        DeactivateAllPanels();
        buildingOptionsDonut.sprite = selectedOption[0];
        buildingMainImage.sprite = selectedOptionImage[0];
    }

    public void OnBuildClick()
    {
        DeactivateAllPanels();
        buildHousePanel.SetActive(true);
        buildingOptionsDonut.sprite = selectedOption[1];
        buildingMainImage.sprite = selectedOptionImage[1];
        panelIsActive = true;
    }

    public void OnSolarClick()
    {
        DeactivateAllPanels();
        buildSolarPanel.SetActive(true);
        buildingOptionsDonut.sprite = selectedOption[2];
        buildingMainImage.sprite = selectedOptionImage[2];
        panelIsActive = true;
    }
    public void OnRotateClick()
    {
        DeactivateAllPanels();
        buildRotatePanel.SetActive(true);
        buildingOptionsDonut.sprite = selectedOption[3];
        buildingMainImage.sprite = selectedOptionImage[3];
        panelIsActive = true;
    }

    public void OnRemoveClick()
    {
        DeactivateAllPanels();
        buildRemovePanel.SetActive(true);
        buildingOptionsDonut.sprite = selectedOption[4];
        buildingMainImage.sprite = selectedOptionImage[4];
        panelIsActive = true;
    }

    public void OnToggleBuildingPanel()
    {
        if (panelIsActive)
        {
            DeactivateAllPanels();
            buildingOptionsDonut.sprite = selectedOption[0];
            buildingMainImage.sprite = selectedOptionImage[0];
        }
        else
            OpenInitialBuildingPanel();
    }

    private void OpenInitialBuildingPanel()
    {
        DeactivateAllPanels();
        buildingPanel.SetActive(true);
        buildingOptionsDonut.sprite = selectedOption[0];
        buildingMainImage.sprite = selectedOptionImage[0];
        panelIsActive = true;
    }
    
    private void DeactivateAllPanels()
    {
        buildingPanel.SetActive(false);
        buildHousePanel.SetActive(false);
        buildSolarPanel.SetActive(false);
        buildRotatePanel.SetActive(false);
        buildRemovePanel.SetActive(false);

        panelIsActive = false;
    }
}
