using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, PlayerInput.ICandyDropActions
{
    //add some gamestate enum to manage player states (spawning, dragging, released, finished)
    public enum PlayerState
    {
        Spawning,
        Dragging,
        Dropping,
        Finished
    }

    public PlayerState CurrentState { get; set; } = PlayerState.Spawning;

    [Header("Ball Settings")]
    [SerializeField] private Ball _ballPrefab;
    private Ball _ball;
    private Rigidbody2D rb;

    [Header("Play Area Collider")]
    [SerializeField] private BoxCollider2D _playAreaCollider;

    [Header("Player Settings")]
    private PlayerInput input;
    private Camera mainCam;

    private bool isDragging = false;
    private float fixedY; // lock Y position at spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playAreaCollider = GameObject.Find("PlayArea").GetComponent<BoxCollider2D>();
        mainCam = Camera.main;
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

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.started && _ball == null && CurrentState == PlayerState.Spawning) // finger touched
        {
            PlayerSpawn();
        }
        else if (context.performed && _ball != null && CurrentState == PlayerState.Dragging) // finger is moving
        {
            isDragging = true;
        }
        else if (context.canceled && _ball != null && CurrentState == PlayerState.Dragging) // finger released
        {
            isDragging = false;
            ReleaseBall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging && _ball != null && CurrentState == PlayerState.Dragging)
        {
            // read position from touch or mouse
            Vector2 screenPos = Touchscreen.current != null && Touchscreen.current.press.isPressed
                ? Touchscreen.current.position.ReadValue()
                : Mouse.current.position.ReadValue();

            Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPos);

            worldPos.y = fixedY; // lock Y position at spawn
            worldPos.z = 0;

            // clamp inside play area
            float leftLimit = _playAreaCollider.bounds.center.x - 2.5f;
            float rightLimit = _playAreaCollider.bounds.center.x + 2.5f;

            worldPos.x = Mathf.Clamp(worldPos.x, leftLimit, rightLimit);

            _ball.transform.position = worldPos; // ball follows finger
        }
    }

    // change top object pool to spawn the ball //
    public void PlayerSpawn()
    {
        Debug.Log($"Player State: {CurrentState}");

        if (_ball != null) return; // already spawned

        //if gamestate is in release, dragging or dropping, do not spawn
        if (CurrentState == PlayerState.Dragging || CurrentState == PlayerState.Dropping)
        {
            Debug.LogWarning("Cannot spawn during Dragging, Released, or Dropping states.");
            return;
        }

        // Set the player's position within the play area
        Vector3 playAreaSize = _playAreaCollider.size;
        Vector3 playAreaCenter = _playAreaCollider.bounds.center;

        Vector3 spawnPosition = new Vector3(_playAreaCollider.bounds.center.x, 6f, 0);

        // Instantiate the player object
        Ball ballObject = Instantiate(_ballPrefab, spawnPosition, Quaternion.identity);
        ballObject.name = "Ball";
        ballObject.SetPlayer(this); // link back to player

        rb = ballObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // disable physics until released

        _ball = ballObject;
        fixedY = spawnPosition.y; // lock Y position at spawn

        //set gamestate to dragging
        CurrentState = PlayerState.Dragging;
        Debug.Log($"Player State: {CurrentState}");
    }

    private void ReleaseBall()
    {
        if (_ball == null) return;

        if (CurrentState == PlayerState.Dragging)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // enable physics
            rb.linearVelocity = Vector2.zero; // reset any existing velocity
            Debug.Log("Ball released!");

            //set gamestate to released
            CurrentState = PlayerState.Dropping;
            Debug.Log($"Player State: {CurrentState}");
        }
    }

    // Call this when the ball reaches the bottom or is destroyed
    public void OnBallFinished()
    {
        _ball = null;
        rb = null;

        CurrentState = PlayerState.Finished;
        Debug.Log($"Player State: {CurrentState}");

        Invoke(nameof(ResetForNextSpawn), 1f); // respawn after 1 second
    }

    private void ResetForNextSpawn()
    {
        CurrentState = PlayerState.Spawning;
        Debug.Log($"Player State: {CurrentState}");
    }
}