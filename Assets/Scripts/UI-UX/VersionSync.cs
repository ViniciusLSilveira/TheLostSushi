using UnityEngine;
using UnityEngine.UI;

public class VersionSync : MonoBehaviour
{

    [SerializeField]
    private Text m_VersionText;

    private void Start() {
        if (!m_VersionText) m_VersionText = this.gameObject.GetComponent<Text>();
        m_VersionText.text = $"<size=25>{Application.version}v</size>";
    }

}
