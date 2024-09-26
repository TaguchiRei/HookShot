using System;
using UnityEngine;

public class MakePendulum : MonoBehaviour
{
    [Tooltip ("プレビュー表示をするかどうか")]
    [SerializeField] bool _showGizmo = true;
    [Tooltip ("開始時に生成したい場合はチェック。つけない場合はMakingPendulum()を呼び出す。")]
    [SerializeField] bool _makePendulumForStart = true;
    [Tooltip ("支点からおもりまでの距離")]
    [SerializeField] float _ropeLength = 2;
    [Tooltip ("プログラムがアタッチされたオブジェクトが支点と重りのどちらか")]
    [SerializeField] FulcrumOrPendulum _fulcrumOrPendulum = FulcrumOrPendulum.fulcrum;
    [Tooltip ("生成した振り子のSpringの値を変更できる。")]
    [SerializeField] ObjectSpring _spring = new();
    [Tooltip ("ロープのマテリアルを設定")]
    [SerializeField] Material _ropeMaterial;
    [Tooltip ("重りとなるオブジェクトの設定。")]
    [SerializeField] ObjectSetting _weightObjectSetting = new();

    GameObject obj;
    GameObject obj2;
    JointSpring _useSpring;

    private void Start()
    {
        _useSpring.spring = _spring._spring;
        _useSpring.damper = _spring._damper;
        _useSpring.targetPosition = _spring._TargetPosition;
        if(_weightObjectSetting.WeightObject == null)
        {
            //_weightObjectSetting.WeightObject = new("Weight", typeof(Rigidbody), typeof(HingeJoint), typeof(MeshRenderer), typeof(MeshCollider));
        }
        if (_makePendulumForStart)
        {
            MakingPendulum();
        }
    }

    public void MakingPendulum(Vector3? anchorPoint = null)
    {
        if (_fulcrumOrPendulum == FulcrumOrPendulum.fulcrum)
        {
            obj = new("Fulcrum", typeof(Rigidbody), typeof(HingeJoint));
            var hingeJoint = obj.GetComponent<HingeJoint>();
            hingeJoint.connectedBody = GetComponent<Rigidbody>();
            obj.transform.position = transform.position;
            hingeJoint.anchor = Vector3.zero;
            hingeJoint.axis = new Vector3(1, 0, 0);
            hingeJoint.spring = _useSpring;

            obj2 = new("Weight", typeof(Rigidbody), typeof( HingeJoint), typeof(MeshRenderer),typeof(MeshCollider),typeof(LineRenderer),typeof(Weight));
            obj2.transform.position = new Vector3(transform.position.x, transform.position.y - _ropeLength, transform.position.z);
            var hingeJoint2 = obj2.GetComponent<HingeJoint>();
            hingeJoint2.connectedBody = obj.GetComponent<Rigidbody>();
            hingeJoint2.anchor = new Vector3(0,_ropeLength,0);
            hingeJoint2.axis = new Vector3(0, 0, 1);
            hingeJoint2.spring = _useSpring;
            if(_ropeMaterial != null)
            {
                obj2.GetComponent<LineRenderer>().material = _ropeMaterial;
            }
            var weight = obj2.GetComponent<Weight>();
            weight.LineRenderer = obj2.GetComponent<LineRenderer>();
            weight.Fulcrum = obj;
        }
        else
        {
            obj2 = new("Anchor", typeof(Rigidbody));
            var rig = obj2.GetComponent<Rigidbody>();
            rig.useGravity = false;
            rig.isKinematic = true;
            obj = new GameObject("Fulcrum", typeof(Rigidbody), typeof(HingeJoint));
            obj.transform.position = (Vector3)anchorPoint;
            var hingeJoint = obj.GetComponent<HingeJoint>();
            hingeJoint.anchor = Vector3.zero;
            hingeJoint.axis = new Vector3(1, 0,0);
            hingeJoint.spring = _useSpring;

            var weight = GetComponent<HingeJoint>();
            weight.connectedBody = obj.GetComponent<Rigidbody>();
            weight.anchor = (Vector3)anchorPoint - transform.position;
            weight.axis = new Vector3(0, 0, 1);
            weight.spring = _useSpring;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * _ropeLength;
        if (_showGizmo)
        {
            if(obj2 != null)
            {
                end = obj2.transform.position;
            }

            // 線を描画
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(start, end);

            // 終端部分に円を描画
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(end, 0.2f); // 半径1.0の円

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(start, _ropeLength);
        }
    }
    enum FulcrumOrPendulum
    {
        fulcrum,
        pendulum,
    }
}

[Serializable]
public struct ObjectSetting
{
    [Tooltip ("当たり判定をつけるかどうか")]
    public bool _useCollider;
    [Tooltip ("当たり判定をIsTriggerにするかどうか")]
    public bool _useIsTrigger;
    [Tooltip ("重りとなるオブジェクトを入れられる。入れなくても動く")]
    public GameObject WeightObject;
}
[Serializable]
public struct ObjectSpring
{
    public float _spring;
    public float _damper;
    public float _TargetPosition;
}