using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    
    private int _numberOfCompleted;
    [SerializeField]  int _totalWiresNumber = 4;

    [SerializeField] private GameObject _levelWonMenu; 
    
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddScore()
    {
        _numberOfCompleted++;
        
        if (_numberOfCompleted == _totalWiresNumber)
        {
            GameWon();
        }
    }

    private void GameWon()
    {
        _levelWonMenu.SetActive(true);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
