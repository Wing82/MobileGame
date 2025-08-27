using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, PlayerInput.ICandyDropActions
{
    //add some gamestate enum to manage player states (spawning, dragging, released, finished)
    public enum PlayerState
    {
        Spawning,
        Dragging,
        Released,
        Finished
    }

    public PlayerState CurrentState { get; private set; } = PlayerState.Finished;

    [Header("Ball Prefab")]
    [SerializeField] private Ball _ballPrefab;
    public bool IsBallSpawned = false;

    [Header("Play Area Collider")]
    [SerializeField] private BoxCollider2D _playAreaCollider;

    [Header("Player Settings")]
    private Ball _ball;
    private PlayerInput input;
    private Camera mainCam;
    private Rigidbody2D rb;

    private bool isDragging = false;
    private float fixedY; // lock Y position at spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playAreaCollider = GameObject.Find("PlayArea").GetComponent<BoxCollider2D>();
        mainCam = Camera.main;

        PlayerSpawn();
    }

    private void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();
        input.CandyDrop.SetCallbacks(this);
    }

    private void OnDisable()
    {
        input.Disable();
        input.CandyDrop.RemoveCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {

    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.started) // finger down
        {
            isDragging = true;
        }
        else if (context.canceled) // finger released
        {
            isDragging = false;
            ReleaseBall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging && _ball != null)
        {
            // read position from touch or mouse
            Vector2 screenPos = Touchscreen.current != null && Touchscreen.current.press.isPressed
                ? Touchscreen.current.position.ReadValue()
                : Mouse.current.position.ReadValue();

            Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPos);

            worldPos.y = fixedY; // lock Y position at spawn
            worldPos.z = 0;

            // clamp inside play area
            float halfWidth = _playAreaCollider.size.x / 2f;
            float leftLimit = _playAreaCollider.bounds.center.x - 2.5f;
            float rightLimit = _playAreaCollider.bounds.center.x + 2.5f;

            worldPos.x = Mathf.Clamp(worldPos.x, leftLimit, rightLimit);

            _ball.transform.position = worldPos; // ball follows finger
        }
    }

    // change top object pool to spawn the ball //
    public void PlayerSpawn()
    {
        //if gamestate is in release or dragging, do not spawn
        if (CurrentState == PlayerState.Released || CurrentState == PlayerState.Dragging)
        {
            Debug.LogWarning("Cannot spawn while in Released or Dragging state.");
            CurrentState = PlayerState.Finished;
            return;
        }

        if (isDragging)
        {
            Debug.LogWarning("Cannot spawn while dragging.");
            return;
        }

        if (_ball != null)
        {
            Debug.LogWarning("Player already spawned.");
            return;
        }

        // Set the player's position within the play area
        Vector3 playAreaSize = _playAreaCollider.size;
        Vector3 playAreaCenter = _playAreaCollider.bounds.center;

        Vector3 spawnPosition = new Vector3(
            playAreaCenter.x, // spawn in center X
            6f, // near top
            0
        );

        // Instantiate the player object
        Ball ballObject = Instantiate(_ballPrefab, spawnPosition, Quaternion.identity);
        ballObject.name = "Player";

        //set gamestate to dragging
        CurrentState = PlayerState.Dragging;

        rb = ballObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // disable physics until released

        _ball = ballObject;
        fixedY = spawnPosition.y; // lock Y position at spawn
    }

    private void ReleaseBall()
    {
        if (_ball != null)
        {
            rb = _ball.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic; // enable physics
            Debug.Log("Ball released!");
            //set gamestate to released
            CurrentState = PlayerState.Released;
        }
    }

    // Call this when the ball reaches the bottom or is destroyed
    public void OnBallFinished()
    {
        _ball = null; // allow spawning a new ball
        PlayerSpawn(); // auto-spawn next one
    }
}
