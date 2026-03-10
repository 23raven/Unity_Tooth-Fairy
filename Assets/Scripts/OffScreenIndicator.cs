using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OffScreenIndicator : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public int indicatorDistance = 250;
    public float separation = 40f;

    private RectTransform indicator;
    private Camera cam;

    static List<RectTransform> activeIndicators = new List<RectTransform>();

    void Start()
    {
        cam = Camera.main;

        Canvas canvas = FindObjectOfType<Canvas>();

        GameObject ui = Instantiate(indicatorPrefab, canvas.transform);
        indicator = ui.GetComponent<RectTransform>();

        activeIndicators.Add(indicator);

        Image img = indicator.GetComponent<Image>();
        Color c = img.color;
        c.a = 0.4f;
        img.color = c;
    }

    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        bool offScreen =
            screenPos.x < 0 || screenPos.x > Screen.width ||
            screenPos.y < 0 || screenPos.y > Screen.height ||
            screenPos.z < 0;

        if (!offScreen)
        {
            indicator.gameObject.SetActive(false);
            return;
        }

        indicator.gameObject.SetActive(true);

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;

        Vector2 indicatorPos = screenCenter + direction * indicatorDistance;

        // Проверяем пересечения
        foreach (RectTransform other in activeIndicators)
        {
            if (other == indicator) continue;

            float dist = Vector2.Distance(indicatorPos, other.position);

            if (dist < separation)
            {
                float angleOffset = 10f * Mathf.Deg2Rad;

                float angle = Mathf.Atan2(direction.y, direction.x);
                angle += angleOffset;

                direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                indicatorPos = screenCenter + direction * indicatorDistance;
            }
        }

        indicator.position = indicatorPos;

        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        indicator.rotation = Quaternion.Euler(0, 0, rot);
    }

    void OnDestroy()
    {
        if (indicator != null)
        {
            activeIndicators.Remove(indicator);
            Destroy(indicator.gameObject);
        }
    }
}