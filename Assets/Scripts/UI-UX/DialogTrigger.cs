using UnityEngine;

public class DialogTrigger : MonoBehaviour {
    public Dialog m_Dialog;

    public void ToggleDialog(bool begin) {
        DialogManager.Instance.BeginDialog(m_Dialog, begin);
        ToggleMouseVisibility.Instance.ToggleMouse(begin);
    }
}