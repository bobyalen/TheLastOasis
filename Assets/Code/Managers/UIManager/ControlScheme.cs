using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ControlScheme : MonoBehaviour
{

    [SerializeField] Button close;
    public static ControlScheme Instance { get; private set; }

    void Start()
    {
        close.onClick.AddListener(Hide);
    }

    void Awake()
    {
        Instance = this;
        Hide();
    }


    public void showControls()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
