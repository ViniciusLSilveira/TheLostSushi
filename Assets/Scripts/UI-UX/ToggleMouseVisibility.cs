using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMouseVisibility : MonoBehaviour
{

    public static ToggleMouseVisibility Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        ToggleMouse(true);
    }

    public void ToggleMouse(bool visible) {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Confined;
    }

}
