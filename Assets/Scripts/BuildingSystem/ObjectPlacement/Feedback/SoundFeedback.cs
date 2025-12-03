/* SoundFeedback
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * Clase que se encarga de la retroalimentación sonora del sistema de construcción.
 * Recibe el tipo de sonido según el Enum SoundType y lo reproduce una vez.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- switch con diferentes tipos de sonidos 
 * 
 * TODO: unificar con un AudioManager para que le afecte el ajuste del volumen
 */

using UnityEngine;

public enum SoundType { Click, Place, Remove, WrongPlacement}

public class SoundFeedback : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound, placeSound, removeSound, wrongPlacementSound;
    [SerializeField] private AudioSource audioSource;

    public void PlaySound (SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Click:
                audioSource.PlayOneShot(clickSound);
                break;
            case SoundType.Place:
                audioSource.PlayOneShot(placeSound);
                break;
            case SoundType.Remove:
                audioSource.PlayOneShot(removeSound);
                break;
            case SoundType.WrongPlacement:
                audioSource.PlayOneShot(wrongPlacementSound);
                break;
            default:
                break;
        }
    }
}
