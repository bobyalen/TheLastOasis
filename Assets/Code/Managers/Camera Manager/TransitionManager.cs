using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    Image bottomPanel;
    [SerializeField]
    Image topPanel;
    [SerializeField]
    Image flashPanel;

    [SerializeField]
    float fadeOutTime;
    [SerializeField]
    float fadeInTime;
    [SerializeField]
    GameObject inventory;
    [SerializeField]
    GameObject playerStats;

    bool canBeSpedUp = false;
    // Start is called before the first frame update
    public static TransitionManager Instance;

    CinemachineVirtualCamera v_cam = null;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.sceneUnloaded += OnLevelUnloaded;
        EventManager.StartListening(Event.BossRoomTransitionStarted, StartBossRoomTransition);
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        SceneManager.sceneUnloaded -= OnLevelUnloaded;
        EventManager.StopListening(Event.BossRoomTransitionStarted, StartBossRoomTransition);
    }
    void Start()
    {
        var camera = Camera.main;
        var brain = camera.GetComponent<CinemachineBrain>();
        if(brain != null)
            v_cam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }


    public void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Ship")
        {
            PlayerStats.Instance.ResetDeath();
        }
        StartCoroutine(FadeOut());
    }

    public void OnLevelUnloaded(Scene scene)
    {

    }

    private void Update()
    {
        if (canBeSpedUp)
        {
            if (Input.GetMouseButton(0))
                Time.timeScale = 3.0f;
            if (Input.GetMouseButtonUp(0))
                Time.timeScale = 1.0f;
        }
        else
            Time.timeScale = 1.0f;
    }

    public IEnumerator FadeOut(bool givePlayerAgencyAtEnd = true, bool fadetoCinematicBars = false)
    {
        topPanel.gameObject.SetActive(true);
        bottomPanel.gameObject.SetActive(true);
        EventManager.TriggerEvent(Event.DialogueFinish, null);
        float minLerp = 230;
        float maxLerp = 460 + (fadetoCinematicBars ? -50 : 0);
        for (float i = 0; i < 0.1f; i += Time.deltaTime)
            yield return null;
        EventManager.TriggerEvent(Event.DialogueStart, null);
        for (float i = 0; i < fadeOutTime; i += Time.deltaTime)
        {
            topPanel.rectTransform.offsetMin =
                new Vector2(topPanel.rectTransform.offsetMin.x,
                Mathf.Lerp(minLerp, maxLerp, i /fadeOutTime));

            bottomPanel.rectTransform.offsetMax =
                new Vector2(bottomPanel.rectTransform.offsetMax.x,
                -Mathf.Lerp(minLerp, maxLerp, i /fadeOutTime));
            yield return null;
        }
        yield return null;
        if(givePlayerAgencyAtEnd)
        EventManager.TriggerEvent(Event.DialogueFinish, null);
    }

    public IEnumerator FadeIn()
    {
        EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
        PlayerStats.Instance.SaveValues();
        topPanel.gameObject.SetActive(true);
        bottomPanel.gameObject.SetActive(true);
        float maxLerp = topPanel.rectTransform.offsetMin.y + 10;
        float minLerp = (maxLerp - 10)/ 2.0f;
        for(float i = 0; i < fadeInTime; i+= Time.deltaTime)
        {
            topPanel.rectTransform.offsetMin =
                new Vector2(topPanel.rectTransform.offsetMin.x, 
                Mathf.Lerp(minLerp, maxLerp, (fadeInTime - i)/fadeInTime));

            bottomPanel.rectTransform.offsetMax =
                new Vector2(bottomPanel.rectTransform.offsetMax.x, 
                -Mathf.Lerp(minLerp, maxLerp, (fadeInTime - i)/fadeInTime));
            yield return null;
        }
        yield return null;

    }

    public IEnumerator ShowCinematicBars(float time)
    {
        topPanel.gameObject.SetActive(true);
        bottomPanel.gameObject.SetActive(true);
        float minLerp = topPanel.rectTransform.offsetMin.y - 50;
        float maxLerp = topPanel.rectTransform.offsetMin.y;
        for(float i = 0; i < time; i += Time.deltaTime)
        {
            topPanel.rectTransform.offsetMin = new Vector2(topPanel.rectTransform.offsetMin.x,
                Mathf.Lerp(minLerp, maxLerp, (time - i) / time));

            bottomPanel.rectTransform.offsetMax = new Vector2(bottomPanel.rectTransform.offsetMin.x,
                -Mathf.Lerp(minLerp, maxLerp, (time - i) / time));
            yield return null;
        }

    }

    public IEnumerator FadeOutCinematicBars(float time)
    {
        float minLerp = topPanel.rectTransform.offsetMin.y;
        float maxLerp = 460;

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            topPanel.rectTransform.offsetMin = new Vector2(topPanel.rectTransform.offsetMin.x,
                Mathf.Lerp(minLerp, maxLerp, i / time));

            bottomPanel.rectTransform.offsetMax = new Vector2(bottomPanel.rectTransform.offsetMin.x,
                -Mathf.Lerp(minLerp, maxLerp,  i / time));
            yield return null;
        }
        yield return null;
    }
    public IEnumerator Flash(float time)
    {
        //flash in - short and brisk
        flashPanel.gameObject.SetActive(true);
        Color c = new Color(1, 1, 1, 0);
        flashPanel.color = c;
        for(float i = 0; i < time / 10; i+= Time.deltaTime)
        {
            c.a = Mathf.Lerp(0, 1, i / time);
            flashPanel.color = c;
            yield return null;
        }
        PlayerController.Instance.Vanish(false);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0, 1, (time - i) / time);
            flashPanel.color = c;
            yield return null;
        }
        flashPanel.gameObject.SetActive(false);
    }
    void StartBossRoomTransition(IEventPacket packet)
    {
        StartCoroutine(BossRoomTransition());
    }

    public IEnumerator BossRoomTransition()
    {
        canBeSpedUp = true;
        EventManager.TriggerEvent(Event.DialogueStart, null);
        UIManager.Instance.SetMainUIState(false);
        StartCoroutine(ShowCinematicBars(0.5f));
        BossCircleScript bcs = FindObjectOfType<BossCircleScript>();
        yield return StartCoroutine(PlayerController.Instance.CinematicMove(bcs.transform.position + Vector3.up * 0.25f, Vector2.down));
        yield return StartCoroutine(bcs.LightUpRunes(0.75f));
        yield return StartCoroutine(PlayerController.Instance.Spin(7.5f));
        yield return StartCoroutine(Flash(1.0f));
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(FadeIn());
        LevelGeneration.Instance.TransportToActualBossRoom();
        BossPattern bp = FindObjectOfType<BossPattern>();
        MoveCameraToBossRoom(bp.transform);
        yield return StartCoroutine(FadeOut(false, true));
        yield return StartCoroutine(bp.Awaken());
        v_cam.Follow = PlayerController.Instance.transform;
        yield return StartCoroutine(FadeOutCinematicBars(0.25f));
        UIManager.Instance.SetMainUIState(true);
        UIManager.Instance.SetBossUIState(true);
        EventManager.TriggerEvent(Event.DialogueFinish, null);
        EventManager.TriggerEvent(Event.AwakenBoss, null);
        canBeSpedUp = false;

    }

    void MoveCameraToBossRoom(Transform bossTransform)
    {
        Camera.main.transform.position = PlayerController.Instance.transform.position;
        Start();
        if (v_cam != null)
        {
            v_cam.m_Lens.OrthographicSize *= 1.33f;
            v_cam.Follow = bossTransform;
        }
        PlayerController.Instance.Vanish(true);

    }
    public IEnumerator Shake(float time)
    {
        if(v_cam != null)
            v_cam.Follow = null;
        Vector3 originalPosCam = Camera.main.transform.position;
        float shakeAmount = 0.7f;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            Vector3 randPos = Random.insideUnitSphere;
            Camera.main.transform.position = originalPosCam + randPos * shakeAmount;
            yield return null;
        }
        Camera.main.transform.position = originalPosCam;
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
            yield return null;
        if(v_cam != null)
            v_cam.Follow = PlayerController.Instance.transform;

    }
}
