using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour {

    public Canvas m_PauseCanvas;

    public static bool m_Paused = false;

    private void Start() {
        m_Paused = false;
        Pause(m_Paused);
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel") && m_PauseCanvas) {
            m_Paused = !m_Paused;
            Pause(m_Paused);
        }
    }

    public void Pause(bool pause) {
        m_Paused = pause;
        Time.timeScale = pause ? 0 : 1;
        m_PauseCanvas.enabled = pause;
        if (DialogManager.Instance.m_IsOpen) return;
        ToggleMouseVisibility.Instance.ToggleMouse(pause);
    }

    public void BackToMenu(string levelName) {
        Pause(false);
        ScreenManager.Instance.LoadLevelLoading(levelName);
    }
}
