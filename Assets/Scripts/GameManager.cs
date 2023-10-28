using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent GameOverEvent;
    // Wires
    private int _numberOfCompleted;
    private int _totalWiresNumber;
    
    [SerializeField] private List<Transform> _wiresStartPrefabs;
    [SerializeField] private List<Transform> _wiresFinishPrefabs;
    [SerializeField] private List<Transform> _randomStartPos;
    [SerializeField] private List<Transform> _randomFinishPos;
    
    // Time
    [SerializeField] private float _gameDuration = 15f;
    private float _gameRemainingTime;
    
    // Interface
    [SerializeField] private GameObject _levelWonMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private Slider _gameDurationSlider;
    
    
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        SetupLevel();
    }

    private void SetupLevel()
    {
        Time.timeScale = 1;
        _gameRemainingTime = _gameDuration;
        
        _totalWiresNumber = _randomStartPos.Count;
        
        for (int i = _totalWiresNumber - 1; i >= 0; i--)
        {
            int randomStartPosIndex = Random.Range(0, _randomStartPos.Count);
            Instantiate(_wiresStartPrefabs[i], _randomStartPos[randomStartPosIndex].position, Quaternion.identity);
            _wiresStartPrefabs.RemoveAt(i);
            _randomStartPos.RemoveAt(randomStartPosIndex);
            
            int randomFinishPosIndex = Random.Range(0, _randomFinishPos.Count);
            Instantiate(_wiresFinishPrefabs[i], _randomFinishPos[randomFinishPosIndex].position, Quaternion.identity);
            _wiresFinishPrefabs.RemoveAt(i);
            _randomFinishPos.RemoveAt(randomFinishPosIndex);
        }
    }
    
    private void Update()
    {
        _gameDurationSlider.value = _gameRemainingTime / _gameDuration;
        _gameRemainingTime -= Time.deltaTime;

        if (_gameRemainingTime <= 0)
        {
            GameOver();
        }
    }
    
    public void AddScore()
    {
        _numberOfCompleted++;
        
        if (_numberOfCompleted == _totalWiresNumber)
        {
            GameWon();
        }
    }

    private void GameOver()
    {
        GameOverEvent?.Invoke();
        
        Time.timeScale = 0;
        _gameDurationSlider.gameObject.SetActive(false);
        _gameOverMenu.SetActive(true);
        _levelWonMenu.SetActive(false);
    }
    
    private void GameWon()
    {
        Time.timeScale = 0;
        _gameDurationSlider.gameObject.SetActive(false);
        _levelWonMenu.SetActive(true);
        _gameOverMenu.SetActive(false);

    }
    
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
