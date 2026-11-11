using UnityEngine;

public class HudMessage
{
    readonly HudMessageView _hudMessageView;
    readonly float _duration;
    private float _hideAtTime;
    private bool _visible;

    public HudMessage(HudMessageView hudMessageView, float duration)
    {
        _hudMessageView = hudMessageView;
        _duration = duration;
        _hudMessageView.Hide();
    }

    public void Show(string message)
    {
        _hudMessageView.Show(message);
        _hideAtTime = Time.time + _duration;
        _visible = true;
    }

    public void Tick()
    {
        if (!_visible)
            return;

        if (Time.time < _hideAtTime)
            return;

        _hudMessageView.Hide();
        _visible = false;
    }
}