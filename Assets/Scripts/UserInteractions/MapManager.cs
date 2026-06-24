using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MainMenuInteractions mainMenuInteractions;
    [SerializeField] private TextMeshProUGUI coordsText;
    private RawImage mapRawImage;
    private double latitudCentro = 0.0;
    private double longitudCentro = 0.0;
    private int zoom = 0;

    private float mapWidth = 512f;
    private float mapHeight = 512f;

    private int currentTileX;
    private int currentTileY;

    private double longitud = 0d;
    private double latitud = 0d;

    void Start()
    {
        mapRawImage = GetComponent<RawImage>();

        mapWidth = mapRawImage.rectTransform.rect.width;
        mapHeight = mapRawImage.rectTransform.rect.height;

        CargarMapaOSM();
    }

    public void CargarMapaOSM()
    {
        currentTileX = ModuloXDeTile(longitudCentro, zoom);
        currentTileY = ModuloYDeTile(latitudCentro, zoom);

        StartCoroutine(DownloadOSMTile(currentTileX, currentTileY, zoom));
    }

    IEnumerator DownloadOSMTile(int x, int y, int z)
    {
        string url = $"https://tile.openstreetmap.org/{z}/{x}/{y}.png";

        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            webRequest.SetRequestHeader("User-Agent", "UnityOSM_PersonalProject_v1.0");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                mapRawImage.texture = DownloadHandlerTexture.GetContent(webRequest);
            }
            else
            {
                Debug.LogError($"Error al descargar el mapa de OSM: {webRequest.error}");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRawImage.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPos);

        float pixelX = localPos.x + (mapWidth / 2f);
        float pixelY = (mapHeight / 2f) - localPos.y;

        double xPos = currentTileX + (pixelX / (double)mapWidth);
        double yPos = currentTileY + (pixelY / (double)mapHeight);

        longitud = TileXALongitud(xPos, zoom);
        latitud = TileYALatitud(yPos, zoom);

        coordsText.text = ($" Lat: {latitud:F6} | Lon: {longitud:F6}");
    }

    #region Matemáticas de Proyección Mercator (OSM)

    int ModuloXDeTile(double lon, int z) => (int)Math.Floor((lon + 180.0) / 360.0 * Math.Pow(2, z));

    int ModuloYDeTile(double lat, int z)
    {
        double latRad = lat * Math.PI / 180.0;
        return (int)Math.Floor((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * Math.Pow(2, z));
    }

    double TileXALongitud(double x, int z) => x / Math.Pow(2, z) * 360.0 - 180.0;

    double TileYALatitud(double y, int z)
    {
        double n = Math.PI - 2.0 * Math.PI * y / Math.Pow(2, z);
        return 180.0 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
    }
    #endregion

    #region MapUI
    public void ConfirmCoords()
    {
        if(longitud == 0 && latitud == 0)
        {
            coordsText.text = "Es necesario seleccionar una ubicación antes de avanzar.";
        }
        else
        {
            GameManager.Instance.longitud = longitud;
            GameManager.Instance.latitud = latitud;
            mainMenuInteractions.OpenTimeSelection();
        }
    }
    #endregion
}
