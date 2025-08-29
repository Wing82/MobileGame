using UnityEngine;

public class Ball : MonoBehaviour
{
    private Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
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
}
