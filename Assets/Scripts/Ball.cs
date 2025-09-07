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

        if (transform.position.x < -2.64f)
        {
            // Out of bounds
            Debug.Log("Ball out of bounds");
            transform.position = new Vector3(-2.64f, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Score"))
        {
            Debug.Log("Ball hit Score area");

            _player.OnBallFinished();
            Destroy(gameObject);
            GameManager.Instance.balls--;
            GameManager.Instance.score += 10;
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
