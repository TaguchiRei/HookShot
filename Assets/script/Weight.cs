using UnityEngine;

public class Weight : MonoBehaviour
{
    public GameObject Fulcrum;
    public LineRenderer LineRenderer;
    private void Update()
    {
        LineRenderer.SetPosition(0, Fulcrum.transform.position);
        LineRenderer.SetPosition(1, transform.position);
    }
}
