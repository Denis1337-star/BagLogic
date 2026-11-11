using UnityEngine;

public class LootMarkerHud : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private LootMarkerView _markerPrefab;
    [SerializeField] private RectTransform _markersRoot;

    [Header("Scan")]
    [SerializeField] private LayerMask _lootMask;
    [SerializeField] private float _markerRadius = 6f;
    [SerializeField] private int _maxMarkers = 8;

    LootMarkerPresenter _presenter;

    void Start()
    {
        if (_camera == null)
            _camera = Camera.main;

        LootMarkerView[] markers = new LootMarkerView[_maxMarkers];
        for (int i = 0; i < _maxMarkers; i++)
        {
            LootMarkerView marker = Object.Instantiate(_markerPrefab, _markersRoot);
            marker.Hide();
            markers[i] = marker;
        }

        LootScanner scanner = new LootScanner(_player, _markerRadius, _lootMask, _maxMarkers);
        LootWorldItem[] buffer = new LootWorldItem[_maxMarkers];
        _presenter = new LootMarkerPresenter(_camera, scanner, buffer, markers, _markerRadius);
    }

    void LateUpdate()
    {
        if (_presenter == null)
            return;

        _presenter.Tick();
    }
}