using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]public static GameManager Instance = default;
    GameObject _player;
    [SerializeField]GameObject _hpUI;
    [SerializeField]GameObject _EnergyUI;
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
}
