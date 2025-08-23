using TMPro;
using UnityEngine;

public class InGame : BaseMenu
{
    public TMP_Text livesText;
    public TMP_Text scoreText;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.InGame;

        livesText.text = $"Lives: {GameManager.Instance.lives}";

        GameManager.Instance.OnLifeValueChanged += LifeValueChanged;
        //GameManager.Instance.SetMenuController(context);

        scoreText.text = $"Score: {GameManager.Instance.score}";

        GameManager.Instance.OnScoreValueChanged += ScoreValueChanged;
    }

    private void ScoreValueChanged(int value) => scoreText.text = $"Score: {value}";

    private void LifeValueChanged(int value) => livesText.text = $"Lives: {value}";

    private void OnDestroy()
    {
        GameManager.Instance.OnLifeValueChanged -= LifeValueChanged;
        GameManager.Instance.OnScoreValueChanged -= ScoreValueChanged;
    }
}
