//27/01/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

[System.Serializable]
public struct WallConfig
{
    public int prefabIndex;
    public Vector3 offset;
    public Vector3 rotation;
}

public static class WallConfigurations
{
    public static Dictionary<WallType, WallConfig> WallConfigs;
    public static void InitializeWallConfigs()
    {
        WallConfigs = new Dictionary<WallType, WallConfig>
        {
            {WallType.Left, new WallConfig {prefabIndex = 0}},
            {WallType.Right, new WallConfig {prefabIndex = 0, offset = new Vector3(0.85f,0,0)}},
            {WallType.Top, new WallConfig {prefabIndex = 0, offset = Vector3.forward, rotation = new Vector3(0,90,0)}},
            {WallType.Bottom, new WallConfig {prefabIndex = 0, offset = new Vector3(0,0,0.15f), rotation = new Vector3(0,90,0)}},

            {WallType.CornerBL, new WallConfig {prefabIndex = 1}},
            {WallType.CornerTL, new WallConfig {prefabIndex = 1, offset = Vector3.forward, rotation = new Vector3(0,90,0)}},
            {WallType.CornerBR, new WallConfig {prefabIndex = 1, offset = Vector3.right, rotation = new Vector3(0,270,0)}},
            {WallType.CornerTR, new WallConfig {prefabIndex = 1, offset = Vector3.right + Vector3.forward, rotation = new Vector3(0,180,0)}},

            {WallType.Vertical, new WallConfig {prefabIndex = 2}},
            {WallType.VerticalCornerStart, new WallConfig {prefabIndex = 3}},
            {WallType.VerticalCornerEnd, new WallConfig {prefabIndex = 3, offset = Vector3.forward + Vector3.right, rotation = new Vector3(0,180,0)}},
            
            {WallType.Horizontal, new WallConfig {prefabIndex = 2, offset = Vector3.forward, rotation = new Vector3(0,90,0)}},
            {WallType.HorizontalCornerStart, new WallConfig {prefabIndex = 3, offset = Vector3.forward,rotation = new Vector3(0,90,0)}},
            {WallType.HorizontalCornerEnd, new WallConfig {prefabIndex = 3, offset = Vector3.right, rotation = new Vector3(0,270,0)}},

            {WallType.Single, new WallConfig {prefabIndex = 4}}
        };
    }
}
