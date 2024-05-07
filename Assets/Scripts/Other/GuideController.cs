using UnityEngine;
using UnityEngine.UI;
using XD.TheManager;

[RequireComponent(typeof(Image))]
public class GuideController : MonoBehaviour, ICanvasRaycastFilter
{
    private Material m_Material;
    private Vector3 m_Center;
    private RectTransform m_Target;
    private Vector3[] m_TargetCorners = new Vector3[4];
    private Camera UICamera;

    public Image arrow; //引导指示箭头
    public Text info;   //引导白色提示信息
    public Text hint;   //引导黄色提示信息

    private float m_Timer;
    private float m_Time;
    private bool m_IsScaling;
    private float m_Width;
    private float m_Height;
    private float m_ScaleWidth;
    private float m_ScaleHeight;
    private RectTransform m_RectTransform;
    // 弱引导整个传递下去 强引导传递Target范围内的事件
    private bool m_IsSoftGuide;
    private void Awake()
    {
        UICamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_Material = GetComponent<Image>().material;
        m_RectTransform = GetComponent<RectTransform>();
    }
    protected virtual void Update()
    {
        if (m_IsScaling)
        {
            m_Timer += Time.deltaTime / m_Time;
            m_Timer = Mathf.Clamp(m_Timer, 0, 1);
            m_Material.SetFloat("_SliderX", Mathf.Lerp(m_ScaleWidth, m_Width, m_Timer));
            m_Material.SetFloat("_SliderY", Mathf.Lerp(m_ScaleHeight, m_Height, m_Timer));
            if (m_Timer >= 1)
            {
                m_Timer = 0;
                m_IsScaling = false;
            }
        }
    }
    // 这里是来获取目标物体的四个点来计算中心点，因为对于矩形或者圆形效果，他们面对的中心点是确定的
    public virtual void Guide(RectTransform target, bool isSoftGuide, float scale = 1, float time = 0)
    {
        if (target == null)
            return;
        m_Target = target;
        m_IsSoftGuide = isSoftGuide;
        target.GetWorldCorners(m_TargetCorners);
        for (int i = 0; i < m_TargetCorners.Length; i++)
        {
            m_TargetCorners[i] = WorldToScreenPoints(m_TargetCorners[i]);
        }
        // 中心点
        m_Center.x = m_TargetCorners[0].x + (m_TargetCorners[3].x - m_TargetCorners[0].x) / 2;
        m_Center.y = m_TargetCorners[0].y + (m_TargetCorners[1].y - m_TargetCorners[0].y) / 2;
        m_Material.SetVector("_Center", m_Center);

        m_Width = (m_TargetCorners[3].x - m_TargetCorners[0].x) / 2;
        m_Height = (m_TargetCorners[1].y - m_TargetCorners[0].y) / 2;
        if (scale <= 1)
            scale = 1;
        m_ScaleWidth = m_Width * scale;
        m_ScaleHeight = m_Height * scale;
        m_Material.SetFloat("_SliderX", m_ScaleWidth);
        m_Material.SetFloat("_SliderY", m_ScaleHeight);

        m_Time = time;
        m_Timer = 0;
        if (m_Time > 0)
            m_IsScaling = true;

        //教程提示信息随着亮处偏移一定位置
        UIManager.Instance._GuideUI.MoveText(arrow, m_Target);
        
    }

    public Vector2 WorldToScreenPoints(Vector3 world)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(UICamera, world);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, screenPoint, UICamera, out localPoint);
        return localPoint;
    }
    // RectangleContainsScreenPoint判断一个屏幕点是否在目标Rect范围内，如果在，则返回真
    // IsRaycastLocationValid为true时，事件向下传递是无效的，
    // 被拦截在当前UI界面，为false，则在当前界面是无效的
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (m_IsScaling) return true;
        if (m_IsSoftGuide) return false;
        return !RectTransformUtility.RectangleContainsScreenPoint(m_Target, sp, eventCamera);
    }
}
