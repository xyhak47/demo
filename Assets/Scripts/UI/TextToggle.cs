using UnityEngine;
using UnityEngine.UI;
using XD.TheManager;

public class TextToggle : Toggle
{
    enum Selection
    {
        Normal,
        Highlighted,
        Pressed,
        Disabled
    }
    Selection selection;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        switch (state)
        {
            //����״̬
            case SelectionState.Normal:
                selection = Selection.Normal;
                break;
            case SelectionState.Highlighted:
                selection = Selection.Highlighted;
                break;
            case SelectionState.Pressed:
                selection = Selection.Pressed;
                break;
            case SelectionState.Disabled:
                selection = Selection.Disabled;
                break;
            default:
                break;
        }
    }

    private void OnGUI()
    {
        GUI.skin = UIManager.Instance.guiSkin;
        GUI.skin.box.fontSize = 20;

        switch (selection)
        {
            case Selection.Highlighted:
                {
                    if (gameObject.name == "HelpIcon")
                        GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 50, 35), "����");
                    else if (gameObject.name == "MapIcon")
                        GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 50, 35), "��ͼ");
                    else if (gameObject.name == "TaskIcon")
                        GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 50, 35), "����");
                    else if (gameObject.name == "ElecIcon")
                        GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 70, 35), "����ͼ");
                }
                break;
            default:
                break;
        }
    }
}
