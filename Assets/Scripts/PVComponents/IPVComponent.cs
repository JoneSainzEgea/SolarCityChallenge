using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPVComponent
{
    void OnPlacement();
    void CheckDependencies();
    void UpdateState();
    void OnRemoval();
}
