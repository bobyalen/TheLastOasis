using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
    [SerializeField]
    private Button yesBtn;
    [SerializeField]
    private Button noBtn;

    //for keyboard UI movement
    RectTransform location;
    bool yesSelected = true;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void SpawnPopup(Action yes, Action no)
    {
        gameObject.SetActive(true);
        yesBtn.Select();
        yesBtn.onClick.AddListener(()=>
        {
            Hide();
            yes();
        });
        noBtn.onClick.AddListener(() =>
        {
            Hide();
            no();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
