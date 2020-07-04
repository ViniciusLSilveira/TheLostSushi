using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUIManager : MonoBehaviour
{
    public static LifeUIManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private GameObject m_HeartsContainer;

    public void UpdateHeartUI(int lifes) {
        DisableAllHearts();
        ActivateHearts(lifes);
    }

    private void DisableAllHearts() {
        List<GameObject> objects = GetAllChildrenObjects();

        if (objects == null) return;


        foreach (var go in objects) {
            go.GetComponent<Image>().enabled = false;
        }
    }

    private void ActivateHearts(int numberToActivate) {
        List<GameObject> childrenObj = GetAllChildrenObjects();

        if (childrenObj == null) return;

        for (int i = 0; i < numberToActivate; i++) {
            childrenObj[i].GetComponent<Image>().enabled = true;
        }
    }

    private List<GameObject> GetAllChildrenObjects() {
        List<GameObject> objects = new List<GameObject>();
        Transform[] ts = m_HeartsContainer.GetComponentsInChildren<Transform>();
        if (ts == null)
            return objects;
        foreach (Transform t in ts) {
            if (t != null && t.gameObject != null && t.gameObject != m_HeartsContainer)
                objects.Add(t.gameObject);
        }

        return objects;
    }

}
