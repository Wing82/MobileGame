using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : BaseMenu
{
    public Button backBtn;
    public Button settingsBtn;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Credits;

        if (backBtn) backBtn.onClick.AddListener(JumpBack);
        if (settingsBtn) settingsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Settings));
    }
}
