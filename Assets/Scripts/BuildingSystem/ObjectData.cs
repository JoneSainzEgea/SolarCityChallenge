using System;
using UnityEngine;


[CreateAssetMenu(menuName = "BuildingSystem/Object")]
[Serializable]
public class ObjectData : ScriptableObject
{
    public int MyProperty { get; set; }
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}
