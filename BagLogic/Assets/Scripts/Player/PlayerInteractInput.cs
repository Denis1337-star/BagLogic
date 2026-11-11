using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractInput : MonoBehaviour
{
    [SerializeField] private InputAction _interactAction;

    public event Action InteractPressed;

    void Awake()
    {
        if (_interactAction == null)
            _interactAction = new InputAction(name: "Interact", type: InputActionType.Button);

        if (_interactAction.bindings.Count == 0)
            _interactAction.AddBinding("<Keyboard>/e");
    }

    void OnEnable()
    {
        _interactAction.performed += OnInteractPerformed;
        _interactAction.Enable();
    }

    void OnDisable()
    {
        _interactAction.performed -= OnInteractPerformed;
        _interactAction.Disable();
    }

    void OnDestroy()
    {
        _interactAction.Dispose();
    }

    void OnInteractPerformed(InputAction.CallbackContext context)
    {
        InteractPressed?.Invoke();
    }
}