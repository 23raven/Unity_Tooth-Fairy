using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    public RectTransform indicatorUI; // UI стрелка
    public Canvas canvas;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // делаем индикатор чуть прозрачным
        Image img = indicatorUI.GetComponent<Image>();
        Color c = img.color;
        c.a = 0.4f;
        img.color = c;
    }

    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        bool isOffScreen =
            screenPos.x <= 0 ||
            screenPos.x >= Screen.width ||
            screenPos.y <= 0 ||
            screenPos.y >= Screen.height ||
            screenPos.z < 0;

        if (!isOffScreen)
        {
            indicatorUI.gameObject.SetActive(false);
            return;
        }

        indicatorUI.gameObject.SetActive(true);

        Vector3 clampedPos = screenPos;

        clampedPos.x = Mathf.Clamp(clampedPos.x, 50, Screen.width - 50);
        clampedPos.y = Mathf.Clamp(clampedPos.y, 50, Screen.height - 50);

        indicatorUI.position = clampedPos;

        Vector3 dir = transform.position - cam.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        indicatorUI.rotation = Quaternion.Euler(0, 0, angle);
    }
}