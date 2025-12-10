using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPConnection : MonoBehaviour
{
    [SerializeField] Component component;

    public void ConnectComponent()
    {
        component.OnConnection();
    }
}
