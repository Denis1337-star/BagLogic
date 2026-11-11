using TMPro;
using UnityEngine;

public class HudMessageView : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _text;

    public void Show(string message)
    {
        _text.text = message;
        _root.SetActive(true);
    }

    public void Hide()
    {
        _root.SetActive(false);
    }
}