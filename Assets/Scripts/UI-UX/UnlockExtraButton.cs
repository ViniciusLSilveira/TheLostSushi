using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockExtraButton : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Botão extra para desbloquear")]
    private Button m_ExtraButton;

    [SerializeField]
    [Tooltip("A condição para o botão ser desbloqueado")]
    private int m_ConditionToUnlock;

    public void SetPointsToZero() {
        PlayerPrefs.SetInt("Points", 0);
    }

    private void OnEnable() {
        if(PlayerPrefs.GetInt("Points") >= m_ConditionToUnlock) {
            UnlockButton(m_ExtraButton);
        }
    }

    private void UnlockButton(Button buttonToUnlock) {
        buttonToUnlock.interactable = true;
    }

}
