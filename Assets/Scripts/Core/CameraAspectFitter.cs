using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(Camera))]
public class CameraAspectFitter : MonoBehaviour
{
    [Tooltip("Ratio target width height. portrait 9/16")]
    public Vector2 targetAspectWH = new Vector2(9, 16);

    Camera cam;

    void OnEnable() { cam = GetComponent<Camera>(); Fit(); }
    void OnValidate() { if (cam == null) cam = GetComponent<Camera>(); Fit(); }
#if UNITY_EDITOR
    void Update() { Fit(); } 
#endif

    void Fit()
    {
        float target = targetAspectWH.x / targetAspectWH.y;
        float window = (float)Screen.width / Screen.height;
        float scaleHeight = window / target;

        if (scaleHeight < 1f)
        {
            // letterbox (פסים למעלה/למטה)
            cam.rect = new Rect(0f, (1f - scaleHeight) * 0.5f, 1f, scaleHeight);
        }
        else
        {
            // pillarbox (פסים לצדדים)
            float scaleWidth = 1f / scaleHeight;
            cam.rect = new Rect((1f - scaleWidth) * 0.5f, 0f, scaleWidth, 1f);
        }

        cam.orthographic = true;
        cam.aspect = target; 
    }
}
