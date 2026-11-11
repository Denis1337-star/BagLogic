using UnityEngine;

public class LootMarkerView : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    public void ShowAt(Vector2 screenPosition)
    {
        _rectTransform.position = screenPosition;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}