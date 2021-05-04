using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 按钮效果
/// </summary>
public class TransitionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private Vector3 originScale = new Vector3(1, 1, 1);
    private bool isStart = false;

    // 是否缩放
    public bool scaleSetting = true;
    // 缩放比率
    public float scaleRate = 0.9f;

    // 是否有音效
    public bool soundSetting = true;
    // 音效ID
    public int soundId = 401;

    void OnEnable() {
        if (isStart) {
            transform.localScale = originScale;
        }
    }

    void Start() {
        originScale = transform.localScale;
        isStart = true;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (scaleSetting) {
            transform.localScale = new Vector3(originScale.x * scaleRate, originScale.y * scaleRate, originScale.z);
        }
        if (soundSetting) {
            //CSSoundManager.GetInstance().PlayButton(soundId);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (scaleSetting) {
            transform.localScale = originScale;
        }
    }
}