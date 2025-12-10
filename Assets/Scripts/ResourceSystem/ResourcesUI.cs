/* ResourcesUI
 * Jone Sainz Egea
 * 05/12/2025
 * 
 * Script que se encarga de la actualización de los elementos de la UI relacionados con los recursos.
 * Contiene acceso a los elementos de la UI. Se llama a la actualización desde ResourceManagement.
 * 
 * v1 -05/12/2025- actualiza texto de dinero y energía.
 */

using System.Windows.Forms;
using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private ButtonRenderer button1x1;

    public void UpdateTextValues(float moneyAmount, float energyAmount)
    {
        moneyText.text = moneyAmount.ToString();
        energyText.text = energyAmount.ToString();
    }

    public void UpdateButtonVisibility(float moneyAmount)
    {

    }
}
