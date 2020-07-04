using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour {
    public static ScreenManager Instance { get; private set; }

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    [Header("Transition")]
    [SerializeField]
    private Animator m_FaderAnimator;
    [SerializeField]
    private Canvas m_TransitionCanvas;

    [Header("Loading")]
    [SerializeField]
    private GameObject m_LoadingContent;

    [SerializeField]
    private float m_DelayAfterLoading = 2.0f;

    private void Start() {
        if (m_FaderAnimator) {
            m_FaderAnimator.SetTrigger("Open");
        }
        
        if (m_TransitionCanvas) m_TransitionCanvas.enabled = false;
        
        if (m_LoadingContent) {
            m_LoadingContent.SetActive(false);
        }
    }

    public void LoadLevel(string nextSceneName) {
        StartCoroutine(ChangeScene(nextSceneName, false));
    }

    public void LoadLevelLoading(string nextSceneName) {
        StartCoroutine(ChangeScene(nextSceneName, true));
    }

    public IEnumerator ChangeScene(string nextSceneName, bool loading) {
        if (m_TransitionCanvas) m_TransitionCanvas.enabled = true;
        if (m_FaderAnimator) {
            m_FaderAnimator.SetTrigger("Close");
            yield return new WaitForSeconds(m_FaderAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        if (nextSceneName.Equals("Quit")) {
            Application.Quit();
        }
        else {
            if (loading) {
                if (m_LoadingContent) {
                    m_LoadingContent.SetActive(true);
                }

                if (m_FaderAnimator) {
                    m_FaderAnimator.SetTrigger("Open");
                    yield return new WaitForSeconds(m_FaderAnimator.GetCurrentAnimatorStateInfo(0).length);
                }
            }

            AsyncOperation asyncScene = SceneManager.LoadSceneAsync(nextSceneName);
            asyncScene.allowSceneActivation = false;

            while (!asyncScene.isDone) {
                if (asyncScene.progress >= 0.9f) {
                    if (loading) {
                        yield return new WaitForSeconds(m_DelayAfterLoading);

                        if (m_FaderAnimator) {
                            m_FaderAnimator.SetTrigger("Close");
                            yield return new WaitForSeconds(m_FaderAnimator.GetCurrentAnimatorStateInfo(0).length);
                        }

                        //if (m_LoadingContent) {
                        //    m_LoadingContent.SetActive(false);
                        //}
                    }

                    asyncScene.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}