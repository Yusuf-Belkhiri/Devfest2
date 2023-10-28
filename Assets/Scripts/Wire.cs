using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Wire : MonoBehaviour
{
    #region FIELDS

    private enum WireColor
    {
        Blue,
        Red,
        Yellow,
        Purple
    }

    private static Wire _selectedWire;
    
    [SerializeField] private WireColor _wireColor;
    [SerializeField] private bool _isStart = true;
    
    private bool _completed;
    private LineRenderer _lineRenderer;
    private Camera _mainCamera;

    #endregion
    
    
    #region METHODS

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        
        _mainCamera = Camera.main;

        _selectedWire = null;
        
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        if (_selectedWire == this)
        {
            _selectedWire = null;
            UpdateLineRenderer(transform.position);
            enabled = false;
        }
    }
    
    private void Update()
    {
        if (_selectedWire == this)
        {
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            UpdateLineRenderer(mousePos);
        }
    }
    
    
    private void OnMouseDown()
    {
        if (_completed) return;
        
        if (_isStart)
        {
            if (_selectedWire != null)
                _selectedWire.UpdateLineRenderer(_selectedWire.transform.position);
            
            _selectedWire = this;
        }
        else if (_selectedWire != null && _selectedWire._wireColor == _wireColor)
        {
            _selectedWire.OnComplete(transform.position);
        }
    }
    
    
    private void UpdateLineRenderer(Vector3 endPosition)
    {
        _lineRenderer.SetPosition(0, transform.localPosition);
        _lineRenderer.SetPosition(1, endPosition);
    }
    
    private void OnComplete(Vector3 endPosition)
    {
        _completed = true;
        UpdateLineRenderer(endPosition);
        _selectedWire = null;
        
        GameManager.Instance.AddScore();
    }

    #endregion
}
