/* ResourceDataSO
 * Jone Sainz Egea
 * 05/12/2025
 * 
 * ScriptableObject para tener la información de cada recurso que afecta al juego.
 * Los objetos tienen: ID único, nombre y Sprite asociado al recurso.
 * 
 * v1 -03/12/2025- ID, nombre y sprite.
 */

using System;
using UnityEngine;

public enum ResourceType { Money, Energy }

[CreateAssetMenu(menuName = "Resources/Resource")]
[Serializable]
public class ResourceDataSO : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public ResourceType ResourceType { get; private set; }

    [field: SerializeField]
    public float InitialAmount { get; private set; }

    [field: SerializeField]
    public Sprite Sprite { get; private set; }
}
