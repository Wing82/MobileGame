using UnityEngine;

public class Ball : MonoBehaviour
{
    Player _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Score"))
        {
            Destroy(collision.gameObject);
            _player.OnBallFinished();
            _player.CurrentState = Player.PlayerState.Finished;
        }
    }
}
