using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Overlay : MonoBehaviour
{
    public static Overlay Instance { get; private set; }
    
    public TextMeshProUGUI messageText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            messageText.gameObject.SetActive(false);
        }
    }

    public void ShowMessage(string msg, float displayTime = 1f)
    {
        StartCoroutine(ShowMessageCoroutine(msg, displayTime));
    }
    IEnumerator ShowMessageCoroutine(string msg, float displayTime)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = msg;
        yield return new WaitForSeconds(displayTime);
        messageText.gameObject.SetActive(false);
    }
}
