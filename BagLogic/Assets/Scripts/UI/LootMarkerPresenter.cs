using UnityEngine;

public class LootMarkerPresenter
{
    readonly Camera _camera;
    readonly LootScanner _scanner;
    readonly LootWorldItem[] _buffer;
    readonly LootMarkerView[] _markers;
    readonly float _maxMarkerDistance;

    public LootMarkerPresenter(
        Camera camera,
        LootScanner scanner,
        LootWorldItem[] buffer,
        LootMarkerView[] markers,
        float maxMarkerDistance)
    {
        _camera = camera;
        _scanner = scanner;
        _buffer = buffer;
        _markers = markers;
        _maxMarkerDistance = maxMarkerDistance;
    }

    public void Tick()
    {
        int lootCount = _scanner.Collect(_buffer);
        int markerIndex = 0;
        float maxSqr = _maxMarkerDistance * _maxMarkerDistance;
        Vector3 camPos = _camera.transform.position;

        for (int i = 0; i < lootCount; i++)
        {
            if (markerIndex >= _markers.Length)
                break;

            LootWorldItem loot = _buffer[i];
            Vector3 worldPos = loot.transform.position + Vector3.up * 0.35f;

            if ((worldPos - camPos).sqrMagnitude > maxSqr)
                continue;

            Vector3 screen = _camera.WorldToScreenPoint(worldPos);
            if (screen.z <= 0f)
                continue;

            _markers[markerIndex].ShowAt(new Vector2(screen.x, screen.y));
            markerIndex++;
        }

        for (int i = markerIndex; i < _markers.Length; i++)
            _markers[i].Hide();
    }
}