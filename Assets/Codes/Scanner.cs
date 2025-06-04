using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    [Header("Visual Settings")]
    public bool showScanRange = true;
    public Color rangeColor;
    public int circleSegments;
    public float rangeThickness = 0.05f;

    void Start()
    {
        // LineRenderer 컴포넌트가 없으면 자동으로 추가
        if (GetComponent<LineRenderer>() == null)
        {
            LineRenderer lr = gameObject.AddComponent<LineRenderer>();
            SetupLineRenderer(lr);
        }
    }

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (showScanRange)
        {
            DrawScanRange();
        }
    }

    void SetupLineRenderer(LineRenderer lr)
    {
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = rangeColor;
        lr.startWidth = rangeThickness;
        lr.endWidth = rangeThickness;
        lr.positionCount = circleSegments + 1;
        lr.useWorldSpace = true;
        lr.loop = true;
    }

    void DrawScanRange()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        if (lr == null) return;

        lr.material.color = rangeColor;
        lr.positionCount = circleSegments + 1;

        float angle = 0f;
        for (int i = 0; i <= circleSegments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * scanRange;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * scanRange;

            Vector3 pos = transform.position + new Vector3(x, y, 0);
            lr.SetPosition(i, pos);

            angle += (360f / circleSegments);
        }
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;
        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}