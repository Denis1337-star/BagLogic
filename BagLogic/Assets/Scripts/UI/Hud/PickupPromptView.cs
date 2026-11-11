using TMPro;
using UnityEngine;

public class PickupPromptView : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _labelText;

    public void ShowForItem(string itemDisplayName)
    {
        _labelText.text = $"E — {itemDisplayName}";
        _root.SetActive(true);
    }

    public void Hide()
    {
        _root.SetActive(false);
    }
}