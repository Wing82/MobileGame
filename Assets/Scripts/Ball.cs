using UnityEngine;

public class Ball : MonoBehaviour
{
    private Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void Update()
    {
        if (transform.position.x > 2.64f)
        {
            // Out of bounds
            Debug.Log("Ball out of bounds");
            transform.position = new Vector3(2.64f, transform.position.y, transform.position.z);
        }

        if (transform.position.x < -2.45f)
        {
            // Out of bounds
            Debug.Log("Ball out of bounds");
            transform.position = new Vector3(-2.45f, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.Instance.balls <= 0)
        {
            GameManager.Instance.balls = 0;
            return;
        }
        if (collision.CompareTag("OneScore"))
        {
            Debug.Log("Ball hit Score area");

            _player.OnBallFinished();
            Destroy(gameObject);
            GameManager.Instance.balls--;
            GameManager.Instance.score += 10;
        }

        if (collision.CompareTag("TwoScore"))
        {
            Debug.Log("Ball hit Score area");

            _player.OnBallFinished();
            Destroy(gameObject);
            GameManager.Instance.balls--;
            GameManager.Instance.score += 20;
        }

        if (collision.CompareTag("FiveScore"))
        {
            Debug.Log("Ball hit Score area");

            _player.OnBallFinished();
            Destroy(gameObject);
            GameManager.Instance.balls--;
            GameManager.Instance.score += 50;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Tile"))
        {
            Debug.Log("Ball hit Tile");
        }
    }
}
