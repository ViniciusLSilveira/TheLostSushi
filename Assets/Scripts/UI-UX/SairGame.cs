using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SairGame : MonoBehaviour
{

    public string m_Level = "MainMenu1";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenManager.Instance.LoadLevel(m_Level);
        }
    }
}
