using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private GameObject _filledRoot;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _disassembleHint;
    private int _index;
    Action<int> _onSlotClicked;

    public void BindClickHandler(int index, Action<int> clicked)
    {
        _index = index;
        _onSlotClicked = clicked;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClicked);
    }
    void OnClicked()
    {
        _onSlotClicked?.Invoke(_index);
    }
    public void ShowItem(ItemDefinition itemConfig, int amount, bool canDisassemble)
    {
        _filledRoot.SetActive(true);
        _iconImage.sprite = itemConfig.IconSprite;
        _iconImage.enabled = itemConfig.IconSprite != null;

        if (amount > 1)
            _amountText.text = amount.ToString();
        else
            _amountText.text = string.Empty;

        if (_disassembleHint != null)
            _disassembleHint.SetActive(canDisassemble);

        _button.interactable = true;
    }

    public void ShowEmpty()
    {
        _filledRoot.SetActive(false);
        _iconImage.sprite = null;
        _amountText.text = string.Empty;

        if (_disassembleHint != null)
            _disassembleHint.SetActive(false);
    }
}