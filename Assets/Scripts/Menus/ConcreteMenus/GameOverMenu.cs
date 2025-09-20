using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : BaseMenu
{
    public Button mainMenuBtn;
    public Button exitBtn;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.GameOver;

        if (mainMenuBtn) mainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        if(exitBtn) exitBtn.onClick.AddListener(QuitGame);
    }
}
