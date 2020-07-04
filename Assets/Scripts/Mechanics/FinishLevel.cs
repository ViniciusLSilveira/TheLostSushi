using Cinemachine;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{

    [Header("Animation")]
    [SerializeField]
    [Tooltip("OPCIONAL: Se tiver animação para o fim da fase")]
    private Animator m_FinishLevelAnim;

    [Space]
    [SerializeField]
    [Tooltip("Nome exato da cena que será a fase seguinte")]
    private string m_NextScene;

    [Header("Camera")]
    [SerializeField]
    [Tooltip("Cinemachine Virtual Camera controlling the real camera")]
    private CinemachineVirtualCamera CmVCam;
    [SerializeField]
    [Tooltip("O tamanho da camera quando o jogador chegar no fim da fase")]
    private float m_DesiredZoom = 6.5f;
    [SerializeField]
    [Tooltip("O multiplcador de velocidade do zoom")]
    private float m_SpeedZoomMultiplier = 2;

    private bool m_ZoomCamera;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.GetComponent<Player>().SetPlayerMaxPoints();
            collision.GetComponent<PlayerMovement>().LockPlayerMovement(true);
            m_ZoomCamera = true;
        }
        if (m_FinishLevelAnim) BeginFinishLevelAnimation();
        GetComponentInChildren<DialogTrigger>().ToggleDialog(true); // Mudar quando tiver animação de fim de nível
    }

    private void BeginFinishLevelAnimation() {
        m_FinishLevelAnim.SetTrigger("Finish");
        if (m_FinishLevelAnim.GetBool("hasFinished")) {
            GetComponentInChildren<DialogTrigger>().ToggleDialog(true);
        }
    }

    private void Update() {
        if (!m_ZoomCamera) return;
        
        CmVCam.m_Lens.OrthographicSize -= Time.deltaTime * m_SpeedZoomMultiplier;
        if(CmVCam.m_Lens.OrthographicSize <= m_DesiredZoom) {
            CmVCam.m_Lens.OrthographicSize = m_DesiredZoom;
            m_ZoomCamera = false;
        }
    }

    public void ChangeScene() {
        ScreenManager.Instance.LoadLevelLoading(m_NextScene);
    }

}
