using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BuildingSystem/ObjectDatabase")]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
}
