using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SphereProductionManager : MonoBehaviour
{
    public ProductionType _productionType;

    [SerializeField] GameObject _wire;
    [SerializeField] GameObject _light;

    List<GameObject> _tileList = new();
    private void Start()
    {
        _tileList.Clear();
        foreach (Transform child in _wire.transform)
        {
            _tileList.Add(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DoMethod();
        }
    }

    public void Reset()
    {
        _tileList.ForEach(tile => tile.SetActive(true));
        _wire.SetActive(true);
    }

    /// <summary>
    /// 順番に消えていく。渦を書くように消えるのが特徴
    /// </summary>
    /// <returns></returns>
    IEnumerator NomallRelease()
    {
        for (int i = 0; i < _tileList.Count; i++)
        {
            _tileList[i].SetActive(false);
            yield return null;
        }
    }
    /// <summary>
    /// ランダムに消える。乱数を使用するため動作は安定しない可能性がある。
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomRelease()
    {
        _wire.GetComponent<Renderer>().enabled = false;
        List<GameObject> tileList = _tileList;
        var num = tileList.Count / 10;
        for (int i = 0; i < num; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                var remove = Random.Range(0, tileList.Count);
                tileList[remove].SetActive(false);
                tileList.RemoveAt(remove);
            }
            yield return null;
        }
    }

    IEnumerator FrontStandardRelease()
    {
        List<GameObject> tileList = _tileList;
        var num = tileList.Count;
        for (int i = 0;i < num; i++)
        {
            yield return null;
        }
    }

    public void DoMethod()
    {
        if (_productionType == ProductionType.NomallRelease)
        {
            Reset();
            StartCoroutine(NomallRelease());
        }
        else if (_productionType == ProductionType.RandomRelease)
        {
            Reset();
            StartCoroutine(RandomRelease());
        }
        else if (_productionType == ProductionType.FrontStandardRelease)
        {
            Reset();
            StartCoroutine(FrontStandardRelease());
        }
    }


    public enum ProductionType
    {
        NomallRelease,
        RandomRelease,
        FrontStandardRelease,
    }
}

[CustomEditor(typeof(SphereProductionManager))]
public class SphereProductionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SphereProductionManager manager = target as SphereProductionManager;
        if (GUILayout.Button("関数を実行"))
        {
            manager.DoMethod();
        }
    }
}
