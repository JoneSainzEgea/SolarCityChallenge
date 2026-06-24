using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public double longitud = 42.339433;
    public double latitud = -3.703308;
    public int year = 2026;
    public int month = 6;
    public int day = 1;


    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public void UpdateDate(int y, int m, int d)
    {
        year = y;
        month = m;
        day = d;
    }
}
