using UnityEngine;


/// <summary>
/// �ű������½������ ���� �����Clear Flags����������Ϊ��Solid Color����Ȼ����ʾ��պ�
/// </summary>
public class Ui3DCamera : MonoBehaviour
{
    // ��ʾ��Ŀ��
    public Transform target;
    // ����Χ����ת�ĵ�
    public Transform pivot;
    // ����ת�ĵ�ƫ��ֵ
    public Vector3 pivotOffset = Vector3.zero;
   
    // ���������Ŀ��ľ���
    public float distance = 10.0f;
    // ��̡������
    public float minDistance = 2f;
    public float maxDistance = 15f;
    // �����ٶ�
    public float zoomSpeed = 1f;
    // x��y�����ת�ٶ�
    public float xSpeed = 250.0f;
    public float ySpeed = 250.0f;
    // y��������Сƫ��ֵ
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    // y�᷽���ǲ���������ת
    public bool allowYTilt = true;

    // ��¼����� ���Ҫ�ƶ��ľ���
    public float targetDistance;
    // ��¼�������x,y����ת
    private float x = 0.0f;
    private float y = 0.0f;
    // ��¼�������x,y����ת��Ŀ��ֵ
    private float targetX = 0.0f;
    private float targetY = 0.0f;
    //x,y��Ի������
    private float xVelocity = 1f;
    private float yVelocity = 1f;
    //������Ի������
    private float zoomVelocity = 1f;
    //��ȡ�����ֵ�ֵ
    internal float scroll; 


    private void Start()
    {
        // ��¼�����������ת�Ƕ�
        var angles = transform.eulerAngles;
        // �տ�ʼ�����x,y����ת
        targetX = x = angles.x;
        //targetY = y = Mathf.Clamp(angles.y, yMinLimit, yMaxLimit);
        targetY = y;
        y = Mathf.Clamp(angles.y, yMinLimit, yMaxLimit);
        // ����Ŀ�����
        targetDistance = distance;
    }


    private void LateUpdate()
    {
        // ���û����ת�㣬��ִ����ת������
        if (!pivot) return;
        // ��ȡ�����ֵ�ƫ��
        scroll = Input.GetAxis("Mouse ScrollWheel");
        // �����ֵ���ͽ�targetDistance������С�����Ǿ����������Զ��
        if (scroll > 0.0f) targetDistance -= zoomSpeed;
        else if (scroll < 0.0f)
            targetDistance += zoomSpeed;

        // ����Ŀ��ľ���-�������Сֵ֮��ȡֵ
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        // �����������Ҽ������ߣ���������ͬʱ������ߵ�ctrl) ���� 
        if (Input.GetMouseButton(1) || (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            // ��ȡˮƽ�����ƫ��ֵ
            targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;

            // �������Y��ƫ�ƣ���ȡ�����y���ϵ�ƫ�ƣ���¼�ı�������y��ĽǶ�
            if (allowYTilt)
            {
                targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                //targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
                targetY = Mathf.Clamp(targetY, yMinLimit, yMaxLimit);
            }
        }

        // Mathf.SmoothDamp����������Ļ������
        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
        y = allowYTilt ? Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.3f) : targetY;
        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.5f);
        // ��ת
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // �����������λ��
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position + pivotOffset;
        // �����������λ�ú���ת
        transform.rotation = rotation;
        transform.position = position;
    }


}