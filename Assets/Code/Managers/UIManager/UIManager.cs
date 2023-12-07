using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("Main Player UI")]
    [SerializeField]
    GameObject UI_Parent;
    [SerializeField]
    TextMeshProUGUI _hpText;
    [SerializeField]
    Slider _hpSlider;
    [SerializeField]
    TextMeshProUGUI _coinText;
    [SerializeField]
    Slider _dashBar;
    [SerializeField]
    Image deathPanel;
    [SerializeField]
    TextMeshProUGUI _deadText;
    [SerializeField]
    TextMeshProUGUI _inventoryLostText;
    [SerializeField]
    Button _returnButton;
    [SerializeField]
    TextMeshProUGUI _returnButtonText;

    [SerializeField]
    float deathPanelFadeInTime = 1.5f;
    [SerializeField]
    float deathTextDelay = 0.5f;
    [SerializeField]
    float deathTextFadeIn = 1.0f;
    [SerializeField]
    float inventoryTextDelay = 0.5f;
    [SerializeField]
    float inventoryFadeIn = 1.0f;
    [SerializeField]
    float buttonDelay = 0.5f;
    [SerializeField]
    float buttonFadeIn = 1.0f;
    [Header("Boss UI")]
    [SerializeField]
    GameObject BossUI;
    [SerializeField]
    TextMeshProUGUI bossName;
    [SerializeField]
    Slider bossHPSlider;


    private int lostCoins = 0;
    private bool isBlind = false;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.StartListening(Event.PlayerDeath, OnPlayerDeath);
        EventManager.StartListening(Event.StatChanged, OnUIBlindness);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.PlayerDeath, OnPlayerDeath);
        EventManager.StopListening(Event.StatChanged, OnUIBlindness);
    }

    void OnUIBlindness(IEventPacket packet)
    {
        StatChangedPacket scp = packet as StatChangedPacket;
        if(scp != null)
        {
            if (scp.stat == Stat.UI_Blindness)
                isBlind = PlayerStats.Instance.cachedCalculatedValues[Stat.UI_Blindness] > 0;
            if (isBlind)
                UI_Parent.SetActive(false);
        }
    }
    void OnPlayerDeath(IEventPacket packet)
    {
        StartCoroutine(ShowDeathScreen());
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBlind)
        {
            _hpText.text = $"{PlayerStats.Instance.cachedCalculatedValues[Stat.Current_Health]}/{PlayerStats.Instance.cachedCalculatedValues[Stat.Health]}";
            _hpSlider.value = PlayerStats.Instance.GetCurrentHealthPercentage();
            _coinText.text = $"× {Inventory.Instance.GetCoins()}";
            _dashBar.value = PlayerController.Instance.GetDashPercentage();
        }
    }
    public IEnumerator ShowDeathScreen()
    {
        Color backPanelColor = deathPanel.color;
        backPanelColor.a = 0;
        deathPanel.color = backPanelColor;
        deathPanel.gameObject.SetActive(true);
        for(float i = 0; i < deathPanelFadeInTime; i+= Time.deltaTime)
        {
            backPanelColor.a = Mathf.Lerp(0.0f, 1.0f, i / deathPanelFadeInTime);
            deathPanel.color = backPanelColor;
            yield return null;
        }
        for (float i = 0; i < deathTextDelay; i += Time.deltaTime)
            yield return null;

        Color deathTextColor = _deadText.color;
        deathTextColor.a = 0.0f;
        _deadText.color = deathTextColor;
        _deadText.gameObject.SetActive(true);
        for(float i = 0; i < deathTextFadeIn; i+= Time.deltaTime)
        {
            deathTextColor.a = Mathf.Lerp(0.0f, 1.0f, i / deathTextFadeIn);
            _deadText.color = deathTextColor;
            yield return null;
        }
        for (float i = 0; i < inventoryTextDelay; i += Time.deltaTime)
            yield return null;

        Color inventoryTextColor = _inventoryLostText.color;

        lostCoins = (int)(Inventory.Instance.GetCoins() * PlayerStats.Instance.cachedCalculatedValues[Stat.Coin_Loss]/100.0f);
        if(lostCoins > Inventory.Instance.GetCoins())
            lostCoins = Inventory.Instance.GetCoins();
        if (lostCoins < 0)
            lostCoins = 0;
        _inventoryLostText.text = $"You've lost {lostCoins} coins xD";
        inventoryTextColor.a = 0.0f;
        _inventoryLostText.color = inventoryTextColor;
        _inventoryLostText.gameObject.SetActive(true);
        for(float i = 0; i < inventoryFadeIn; i+= Time.deltaTime)
        {
            inventoryTextColor.a = Mathf.Lerp(0.0f, 1.0f, i / inventoryFadeIn);
            _inventoryLostText.color = inventoryTextColor;
            yield return null;
        }
        for(float i = 0; i < buttonDelay; i+= Time.deltaTime)
        {
            yield return null;
        }
        Color returnButtonColor = _returnButtonText.color;
        returnButtonColor.a = 0.0f;
        _returnButtonText.color = returnButtonColor;
        _returnButton.gameObject.SetActive(true);
        for(float i = 0; i < buttonFadeIn; i+= Time.deltaTime)
        {
            returnButtonColor.a = Mathf.Lerp(0.0f, 1.0f, i / buttonFadeIn);
            _returnButtonText.color = returnButtonColor;
            yield return null;
        }
        _returnButton.Select();
        _returnButton.onClick.AddListener(delegate { ReturnToShip(); });




        yield return null;
    }

    public IEnumerator FadeOutDeath()
    {
        _returnButton.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        Color deadColor = _deadText.color;
        Color inventoryColor = _inventoryLostText.color;
        Color returnColor = _returnButtonText.color;
        for(float i = 0; i < deathTextFadeIn; i+= Time.deltaTime)
        {
            float a = Mathf.Lerp(0.0f, 1.0f, (deathTextFadeIn - i) / deathTextFadeIn);
            deadColor.a = a;
            inventoryColor.a = a;
            returnColor.a = a;
            _deadText.color = deadColor;
            _inventoryLostText.color = inventoryColor;
            _returnButtonText.color = returnColor;
            yield return null;
        }
    }

    void ReturnToShip()
    {
        _returnButton.image = null;
        StartCoroutine(ReturnTransition());
    }

    public void SetMainUIState(bool active)
    {
        UI_Parent.SetActive(active);
    }
    IEnumerator ReturnTransition()
    {
        int remainingCoins = 0;
        remainingCoins = Inventory.Instance.GetCoins() - lostCoins;
        Inventory.Instance.ClearInventory();
        Inventory.Instance.AddCoins(remainingCoins, true);
        yield return StartCoroutine(FadeOutDeath());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Ship");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    public void SetBossUIState(bool active)
    {
        BossUI.SetActive(active);
    }

    public void SetBossName(string name)
    {
        bossName.text = name;
    }

    public void SetBossHealth(float ratio)
    {
        bossHPSlider.value = ratio;
    }
}
