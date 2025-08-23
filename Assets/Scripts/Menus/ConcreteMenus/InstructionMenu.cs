using UnityEngine;
using UnityEngine.UI;

public class InstructionMenu : BaseMenu
{
    public Button backBtn;
    public Button creditsBtn;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Instruction;

        if (backBtn) backBtn.onClick.AddListener(JumpBack);
        if (creditsBtn) creditsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Credits));
    }
}
