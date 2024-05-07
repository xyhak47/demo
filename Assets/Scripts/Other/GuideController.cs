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

    public Image arrow; //����ָʾ��ͷ
    public Text info;   //������ɫ��ʾ��Ϣ
    public Text hint;   //������ɫ��ʾ��Ϣ

    private float m_Timer;
    private float m_Time;
    private bool m_IsScaling;
    private float m_Width;
    private float m_Height;
    private float m_ScaleWidth;
    private float m_ScaleHeight;
    private RectTransform m_RectTransform;
    // ����������������ȥ ǿ��������Target��Χ�ڵ��¼�
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
    // ����������ȡĿ��������ĸ������������ĵ㣬��Ϊ���ھ��λ���Բ��Ч����������Ե����ĵ���ȷ����
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
        // ���ĵ�
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

        //�̳���ʾ��Ϣ��������ƫ��һ��λ��
        UIManager.Instance._GuideUI.MoveText(arrow, m_Target);
        
    }

    public Vector2 WorldToScreenPoints(Vector3 world)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(UICamera, world);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, screenPoint, UICamera, out localPoint);
        return localPoint;
    }
    // RectangleContainsScreenPoint�ж�һ����Ļ���Ƿ���Ŀ��Rect��Χ�ڣ�����ڣ��򷵻���
    // IsRaycastLocationValidΪtrueʱ���¼����´�������Ч�ģ�
    // �������ڵ�ǰUI���棬Ϊfalse�����ڵ�ǰ��������Ч��
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (m_IsScaling) return true;
        if (m_IsSoftGuide) return false;
        return !RectTransformUtility.RectangleContainsScreenPoint(m_Target, sp, eventCamera);
    }
}
