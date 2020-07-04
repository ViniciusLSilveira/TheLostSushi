using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    public GameObject m_SoundManager;

    void Start()
    {
        Instantiate(m_SoundManager);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
