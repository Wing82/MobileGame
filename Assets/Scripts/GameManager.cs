using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public event Action<int> OnLifeValueChanged;
    public event Action<int> OnScoreValueChanged;

    private MenuController currentMenuController;

    #region GAME PROPERTIES
    [SerializeField] private int maxLives = 5;
    private int _lives = 3;

    public int lives
    {
        get => _lives;
        set
        {
            if (value <= 0)
            {
                //GameOver();
                return;
            }

            if (_lives > value) //Respawn();

            _lives = value;

            if (_lives > maxLives) _lives = maxLives;

            OnLifeValueChanged?.Invoke(_lives);

            Debug.Log($"{gameObject.name} lives has changed to {_lives}");
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (maxLives <= 0) maxLives = 5;
        _lives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
