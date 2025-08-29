using TMPro;
using UnityEngine;

public class InGame : BaseMenu
{
    public TMP_Text ballsText;
    public TMP_Text scoreText;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.InGame;

        ballsText.text = $"Balls: {GameManager.Instance.balls}";

        GameManager.Instance.OnBallsValueChanged += BallValueChanged;

        scoreText.text = $"Score: {GameManager.Instance.score}";

        GameManager.Instance.OnScoreValueChanged += ScoreValueChanged;
    }

    private void ScoreValueChanged(int value) => scoreText.text = $"Score: {value}";

    private void BallValueChanged(int value) => ballsText.text = $"Balls: {value}";

    private void OnDestroy()
    {
        GameManager.Instance.OnBallsValueChanged -= BallValueChanged;
        GameManager.Instance.OnScoreValueChanged -= ScoreValueChanged;
    }
}
