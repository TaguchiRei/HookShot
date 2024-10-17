using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance = default;
    GameObject _player;
    GameObject _hpUI;
    GameObject _energyUI;
    Camera _camera;
    [SerializeField] GameObject _enemyTarget;
    List<GameObject> _enemys = new();


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
        _energyUI = GameObject.FindGameObjectWithTag("Energy");
        _player = GameObject.FindGameObjectWithTag("Player");
        _camera = GameObject.FindObjectOfType<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        foreach (GameObject enemy in _enemys)
        {
            if(enemy.GetComponent<EnemyUI>()._onTarget == false)
            {
                enemy.GetComponent<EnemyUI>()._onTarget = true;
            }
        }
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShotGun(float full, float bullet)
    {
        _energyUI.GetComponent<Image>().DOFillAmount((bullet / full), 0.2f);
    }
    public void HpUpDown(float max, float now)
    {
        _hpUI.GetComponent<Image>().DOFillAmount(now / max, 0.2f);
    }
    void AddEnemy(GameObject enemy)
    {
        _enemys.Add(enemy);
    }
}
