using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance = default;
    GameObject _player;
    GameObject _hpUI;
    GameObject _EnergyUI;
    [SerializeField] GameObject _enemyTarget;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Start()
    {
        _hpUI = GameObject.FindGameObjectWithTag("HP");
        _EnergyUI = GameObject.FindGameObjectWithTag("Energy");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShotGun(float full, float bullet)
    {
        _EnergyUI.GetComponent<Image>().DOFillAmount((bullet / full), 0.2f);
    }
    public void HpUpDown(float max, float now)
    {
        _hpUI.GetComponent<Image>().DOFillAmount(now / max, 0.2f);
    }
}
