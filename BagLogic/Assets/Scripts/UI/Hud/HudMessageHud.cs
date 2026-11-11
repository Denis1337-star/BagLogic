using UnityEngine;

public class HudMessageHud : MonoBehaviour
{
    [SerializeField] private HudMessageView _hudMessageView;
    [SerializeField] private float _duration = 2f;

    public HudMessage Message { get; private set; }

    void Awake()
    {
        Message = new HudMessage(_hudMessageView, _duration);
    }

    void Update()
    {
        Message.Tick();
    }
}