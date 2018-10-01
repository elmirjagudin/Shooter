using UnityEngine;
using UnityEngine.XR;

///
/// Adapted from a script by IJM:
/// http://answers.unity3d.com/questions/29741/mouse-look-script.html
///
/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation
///
public class MouseLook : MonoBehaviour
{
    public delegate void CurrentMouseRotation(out float xRotation, out float yRotation);

    const float sensitivityX = 6F;
    const float sensitivityY = 6F;

    const float minimumX = -360F;
    const float maximumX = 360F;

    const float minimumY = -86F;
    const float maximumY = 86F;

    float rotationX = 0F;
    float rotationY = 0F;

    Quaternion originalRotation;

    void Start()
    {
        //originalRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);
        originalRotation = gameObject.transform.rotation;
    }

    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        gameObject.transform.rotation = (originalRotation * xQuaternion * yQuaternion);
//        xRotation = rot.x;
//        yRotation = rot.y;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }

        if (angle > 360F)
        {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }

}