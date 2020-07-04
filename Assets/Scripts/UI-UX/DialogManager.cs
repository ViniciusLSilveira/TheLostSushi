using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("UI (User Interface)")]
    public Text m_PlayerNameText;
    public Text m_ReferencePointNameText;
    public Text m_SentenceText;
    public Text m_SkipText;
    public Image m_PlayerImage;
    public Image m_ReferencePointImage;

    [Header("Animator")]
    public Animator m_Animator;
    private Queue<DialogSentence> m_Sentences = new Queue<DialogSentence>();
    public bool m_IsOpen { get; private set; }

    public void OpenDialogAnimation(bool open)
    {
        m_IsOpen = open;
        if (m_Animator) m_Animator.SetBool("Open", open);
    }

    public void BeginDialog(Dialog dialog, bool firtsDialog)
    {
        if (m_IsOpen && !firtsDialog)
        {
            CloseDialog();
            return;
        }

        OpenDialogAnimation(true);
        m_Sentences.Clear();
        UpdateUI(dialog);
    }

    public void UpdateUI(Dialog dialog)
    {
        if (m_PlayerNameText) m_PlayerNameText.text = dialog.m_PlayerName;
        if (m_ReferencePointNameText) m_ReferencePointNameText.text = dialog.m_ReferencePointName;
        if (m_PlayerImage) m_PlayerImage.sprite = dialog.m_PlayerImage;
        if (m_ReferencePointImage) m_ReferencePointImage.sprite = dialog.m_ReferencePointImage;

        foreach (var sentence in dialog.m_Sentences)
            m_Sentences.Enqueue(sentence);

        StartCoroutine(FirstSentence());
    }

    public IEnumerator FirstSentence()
    {
        m_SkipText.text = "NEXT";

        m_SentenceText.text = string.Empty;
        yield return new WaitForSeconds(0.5f);

        NextSentence();
    }

    public void NextSentence()
    {
        if (m_Sentences.Count == 0)
        {
            CloseDialog();
            return;
        }

        m_SkipText.text = m_Sentences.Count == 1 ? "CLOSE" : "NEXT";

        var sentence = m_Sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(WriteSentence(sentence));
    }

    public void CloseDialog()
    {
        OpenDialogAnimation(false);
        GameObject.Find("FinishLevel").GetComponent<FinishLevel>().ChangeScene();
    }

    private IEnumerator WriteSentence(DialogSentence sentence)
    {
        m_SentenceText.text = string.Empty;
        foreach (char letter in sentence.m_Text.ToCharArray())
        {
            while (Time.timeScale == 0) yield return null;
            m_SentenceText.text += letter;
            yield return null;
        }
    }
}

[Serializable]
public class Dialog
{
    public string m_PlayerName;
    public string m_ReferencePointName;
    public Sprite m_PlayerImage;
    public Sprite m_ReferencePointImage;
    public List<DialogSentence> m_Sentences;
}

[Serializable]
public class DialogSentence
{
    [TextArea(1, 10)]
    public string m_Text;
}