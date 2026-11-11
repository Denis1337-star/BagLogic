using UnityEngine;

public class LootMarkerPresenter
{
    readonly Camera _camera;
    readonly LootScanner _lootScanner;
    readonly LootWorldItem[] _bufferLootWorldItem;
    readonly LootMarkerView[] _markersLootMarkerView;
    readonly float _maxMarkerDistance;

    public LootMarkerPresenter(
        Camera camera,
        LootScanner lootScanner,
        LootWorldItem[] bufferLootWWorldItem,
        LootMarkerView[] markersLootMarkerView,
        float maxMarkerDistance)
    {
        _camera = camera;
        _lootScanner = lootScanner;
        _bufferLootWorldItem = bufferLootWWorldItem;
        _markersLootMarkerView = markersLootMarkerView;
        _maxMarkerDistance = maxMarkerDistance;
    }

    public void Tick()
    {
        int lootCount = _lootScanner.Collect(_bufferLootWorldItem);
        int markerIndex = 0;
        float maxSqr = _maxMarkerDistance * _maxMarkerDistance;
        Vector3 cameraPosition = _camera.transform.position;

        for (int i = 0; i < lootCount; i++)
        {
            if (markerIndex >= _markersLootMarkerView.Length)
                break;

            LootWorldItem lootWorldItem = _bufferLootWorldItem[i];
            Vector3 worldPosition = lootWorldItem.transform.position + Vector3.up * 0.35f;

            if ((worldPosition - cameraPosition).sqrMagnitude > maxSqr)
                continue;

            Vector3 screen = _camera.WorldToScreenPoint(worldPosition);
            if (screen.z <= 0f)
                continue;

            _markersLootMarkerView[markerIndex].ShowAt(new Vector2(screen.x, screen.y));
            markerIndex++;
        }

        for (int i = markerIndex; i < _markersLootMarkerView.Length; i++)
            _markersLootMarkerView[i].Hide();
    }
}