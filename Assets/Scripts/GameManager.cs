using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public event Action<int> OnBallsValueChanged;
    public event Action<int> OnScoreValueChanged;

    private MenuController currentMenuController;

    #region GAME PROPERTIES
    [SerializeField] private int maxBalls = 10;
    private int _balls = 5;

    public int balls
    {
        get => _balls;
        set
        {

            _balls = value;

            if (value <= 0)
            {
                _balls = 0;
                GameOver();
            }

            //if (_balls > value) //Respawn();

            if (_balls > maxBalls) _balls = maxBalls;

            OnBallsValueChanged?.Invoke(_balls);

            Debug.Log($"{gameObject.name} lives has changed to {_balls}");
        }
    }

    private int _score = 0;
    public int score
    {
        get => _score;
        set
        {
            _score = value;

            OnScoreValueChanged?.Invoke(_score);

            Debug.Log($"{gameObject.name} score has changed to {_score}");
        }
    }
    #endregion

    public void SetMenuController(MenuController newMenuController) => currentMenuController = newMenuController;

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
    }

    void Start()
    {
        // Initialize game state if needed
    }
    void Update()
    {
        // Handle global input if needed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOver");
    }

}
