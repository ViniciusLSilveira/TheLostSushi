using UnityEngine;
using UnityEngine.UI;

public class PointsUIManager : MonoBehaviour
{

    public static PointsUIManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        m_AllPoints = FindAllPointsCollectable();
    }

    public Text m_PointsText;

    private int m_AllPoints = 0;

    public void UpdatePointsUI(int points) {
        m_PointsText.text = $"{points}/{m_AllPoints}";
    }

    private int FindAllPointsCollectable() {
        return GameObject.FindGameObjectsWithTag("Ball").Length;
    }

}
