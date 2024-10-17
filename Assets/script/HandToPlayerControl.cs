using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandToPlayerControl : MonoBehaviour
{
    [SerializeField] PlayerMove _playerMove;
    [SerializeField] Animator _animator;
    public void Reload()
    {
        _playerMove.Reload();
    }
    public void CanShoot()
    {
        _playerMove._canShootRailgun = true;
    }
    public void Shoot()
    {
        _animator.SetBool("shootRailgun", false);
        _animator.SetBool("railgun", false);
        _playerMove._gunMode = true;
    }
}
