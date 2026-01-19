/* BuildingPanelController
 * Jone Sainz Egea
 * 19/01/2026
 * 
 * Controlador con los métodos de los botones del panel de construcción.
 * 
 * v1 -19/01/2026- OnBuildClick, OnSolarClick.
 */
using UnityEngine;

public class BuildingPanelController : MonoBehaviour
{
    [SerializeField] private GameObject buildHousePanel;
    [SerializeField] private GameObject buildComponentPanel;

    public void OnBuildClick()
    {
        DeactivateAllPanels();
        buildHousePanel.SetActive(true);
    }

    public void OnSolarClick()
    {
        DeactivateAllPanels();
        buildComponentPanel.SetActive(true);
    }
    
    private void DeactivateAllPanels()
    {
        buildHousePanel.SetActive(false);
        buildComponentPanel.SetActive(false);
    }
}
