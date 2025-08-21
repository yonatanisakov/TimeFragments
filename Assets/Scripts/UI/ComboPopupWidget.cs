using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ComboPopupWidget : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI label;

    [Header("Follow")]
    [SerializeField] private Vector2 screenOffset = new Vector2(0f, 60f); // ������� ��� �����
    [SerializeField] private float lifetime = 0.9f;                       // ��� ��� ��

    [Header("Motion")]
    [SerializeField] private Vector2 drift = new Vector2(0f, 70f);        // ���� ��� �����

    Camera _cam;
    Transform _target;
    CanvasGroup _cg;
    RectTransform _rt;
    float _t;
    bool _running;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();

        // �����: �� �-Canvas ��� Overlay ����� �-MainCamera; �� Camera/World - ���� ��-Canvas
        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay && canvas.worldCamera != null)
            _cam = canvas.worldCamera;
        else
            _cam = Camera.main;

        // CanvasGroup for fade
        _cg = GetComponent<CanvasGroup>();
        if (_cg == null) _cg = gameObject.AddComponent<CanvasGroup>();

        // TMP label
        if (label == null)
            label = GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);

        // ����� ���� ��� ��� ���� �� ������ �������
        Debug.Log($"[ComboPopupWidget] Awake cam={_cam} label={(label ? label.name : "NULL")}");

    }

    /// <summary> �����/����� �� ������ �� ������ ���� target. </summary>
    public void Show(Transform target, string text, Color color)
    {
        Debug.Log($"[ComboPopupWidget] Show target={(target ? target.name : "NULL")} text={text}");

        if (target == null) return;

        // �� ��� label, ���� ���� ����� ����� �����
        if (label == null)
            label = GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);

        // �� ����� ��� � ����� ����
        if (label == null) return;

        _target = target;
        label.text = text;
        label.color = color;

        _t = 0f;
        if (_cg != null) _cg.alpha = 1f;

        _running = true;
        gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if (!_running || _target == null || _cam == null) { HideImmediate(); return; }

        _t += Time.unscaledDeltaTime;
        float k = Mathf.Clamp01(_t / lifetime);

        // ����� ���� �� ����� + offset + drift
        Vector2 basePos = _cam.WorldToScreenPoint(_target.position);
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rt.parent as RectTransform, basePos, null, out uiPos);
        _rt.anchoredPosition = uiPos + screenOffset + drift * k;
        if (_cg != null) _cg.alpha = 1f - k;

        if (_t >= lifetime)
            HideImmediate();
    }

    void HideImmediate()
    {
        _running = false;
        gameObject.SetActive(false);
    }
}
