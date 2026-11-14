using UnityEngine;

public class LootMarkerHud : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private LootMarkerView _markerPrefab;
    [SerializeField] private RectTransform _markersRoot;

    [Header("Scan")]
    [SerializeField] private LayerMask _lootMask;
    [SerializeField] private float _markerRadius = 6f;
    [SerializeField] private int _maxMarkers = 8;

    LootMarkerPresenter _lootMarkerPresenter;

    void Start()
    {
        if (_camera == null)
            _camera = Camera.main;

        LootMarkerView[] markersLootView = new LootMarkerView[_maxMarkers];
        for (int i = 0; i < _maxMarkers; i++)
        {
            LootMarkerView markerLootView = Object.Instantiate(_markerPrefab, _markersRoot);
            markerLootView.Hide();
            markersLootView[i] = markerLootView;
        }

        LootScanner lootScanner = new LootScanner(_playerTransform, _markerRadius, _lootMask, _maxMarkers);
        LootWorldItem[] bufferLootWorldItem = new LootWorldItem[_maxMarkers];
        _lootMarkerPresenter = new LootMarkerPresenter(_camera, lootScanner, bufferLootWorldItem, markersLootView, _markerRadius);
    }

    void LateUpdate()
    {
        if (_lootMarkerPresenter == null)
            return;

        _lootMarkerPresenter.Tick();
    }
}