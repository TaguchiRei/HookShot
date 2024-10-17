using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBullet : MonoBehaviour
{
    GameObject _muzzle;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Material _material;
    [SerializeField] float _range = 20f;
    float _timer = 3f;
    void Start()
    {
        _muzzle = GameObject.Find("muzzle");
        transform.SetPositionAndRotation(_muzzle.transform.position, _muzzle.transform.rotation);
        var ray = Physics.RaycastAll(transform.position, transform.forward, _range);
        _lineRenderer.material = _material;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.TransformPoint(new Vector3(0, 0, _range * 1000)));
    }
    private void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
