using UnityEngine;


/// <summary>
/// 脚本挂载新建相机上 —— 新相机Clear Flags清除标记设置为：Solid Color，不然会显示天空盒
/// </summary>
public class Ui3DCamera : MonoBehaviour
{
    // 显示的目标
    public Transform target;
    // 物体围绕旋转的点
    public Transform pivot;
    // 与旋转的的偏移值
    public Vector3 pivotOffset = Vector3.zero;
   
    // 摄像机距离目标的距离
    public float distance = 10.0f;
    // 最短、最长距离
    public float minDistance = 2f;
    public float maxDistance = 15f;
    // 缩放速度
    public float zoomSpeed = 1f;
    // x，y轴的旋转速度
    public float xSpeed = 250.0f;
    public float ySpeed = 250.0f;
    // y轴的最大、最小偏移值
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    // y轴方向是不是允许旋转
    public bool allowYTilt = true;

    // 记录相机与 最后要移动的距离
    public float targetDistance;
    // 记录摄像机的x,y轴旋转
    private float x = 0.0f;
    private float y = 0.0f;
    // 记录摄像机的x,y轴旋转的目标值
    private float targetX = 0.0f;
    private float targetY = 0.0f;
    //x,y相对缓冲减速
    private float xVelocity = 1f;
    private float yVelocity = 1f;
    //缩放相对缓冲减速
    private float zoomVelocity = 1f;
    //获取鼠标滚轮的值
    internal float scroll; 


    private void Start()
    {
        // 计录现在相机的旋转角度
        var angles = transform.eulerAngles;
        // 刚开始相机的x,y轴旋转
        targetX = x = angles.x;
        //targetY = y = Mathf.Clamp(angles.y, yMinLimit, yMaxLimit);
        targetY = y;
        y = Mathf.Clamp(angles.y, yMinLimit, yMaxLimit);
        // 设置目标距离
        targetDistance = distance;
    }


    private void LateUpdate()
    {
        // 如果没有旋转点，不执行旋转和缩放
        if (!pivot) return;
        // 获取鼠标滚轮的偏移
        scroll = Input.GetAxis("Mouse ScrollWheel");
        // 如果有值，就将targetDistance增大、缩小，就是距离摄像机的远近
        if (scroll > 0.0f) targetDistance -= zoomSpeed;
        else if (scroll < 0.0f)
            targetDistance += zoomSpeed;

        // 距离目标的距离-再最大、最小值之间取值
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        // 如果按下鼠标右键，或者（鼠标左键的同时按下左边的ctrl) 或者 
        if (Input.GetMouseButton(1) || (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            // 获取水平方向的偏移值
            targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;

            // 如果允许Y轴偏移，获取鼠标在y轴上的偏移，记录改变的摄像机y轴的角度
            if (allowYTilt)
            {
                targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                //targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
                targetY = Mathf.Clamp(targetY, yMinLimit, yMaxLimit);
            }
        }

        // Mathf.SmoothDamp用于做相机的缓冲跟踪
        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
        y = allowYTilt ? Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.3f) : targetY;
        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.5f);
        // 旋转
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // 摄像机的最终位置
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position + pivotOffset;
        // 设置摄像机的位置和旋转
        transform.rotation = rotation;
        transform.position = position;
    }


}