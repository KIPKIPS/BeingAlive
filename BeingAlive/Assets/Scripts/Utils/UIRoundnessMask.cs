using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//*************************************************
//优化性能，用UV代替坐标进行圆形计算 2020/09
//不能用UV去计算：UV横纵比例不一致，或Image有九宫切割 2020/09/07
//*************************************************

/// <summary>
///支持对Rawimage和Image进行圆形裁剪
/// </summary>
[RequireComponent(typeof(Graphic))]
[DisallowMultipleComponent, ExecuteInEditMode]
public class UIRoundnessMask : BaseMeshEffect {
    static Vector3[] st_FourVectorArray = new Vector3[4];
    static Material st_MaskUIMat;
    static Material st_MaskGreyUIMat;

    static Material getMaskMat {
        get {
            if (st_MaskUIMat == null) {
                st_MaskUIMat = new Material(Shader.Find("UI/xProject-Default"));
                st_MaskUIMat.hideFlags = HideFlags.DontSave;
                st_MaskUIMat.name = "static_UIRoundnessMask";
                st_MaskUIMat.EnableKeyword("UI_ROUNDNESS_MASK");
            }
            return st_MaskUIMat;
        }
    }
    static Material getMaskGreyMat {
        get {
            if (st_MaskGreyUIMat == null) {
                st_MaskGreyUIMat = new Material(Shader.Find("UI/xProject-Default"));
                st_MaskGreyUIMat.hideFlags = HideFlags.DontSave;
                st_MaskGreyUIMat.name = "static_UIRoundnessMaskGrey";
                st_MaskGreyUIMat.EnableKeyword("UI_ROUNDNESS_MASK");
                st_MaskGreyUIMat.EnableKeyword("GAME_GREY");
            }
            return st_MaskGreyUIMat;
        }
    }

    [SerializeField, Range(0.01f, 1f)]
    [Tooltip("边缘抗锯齿强度")]
    float m_MsaaVolume = 0.01f;
    [SerializeField]
    bool m_IsGrey = false;

    Vector3[] m_WroldCornersArray = new Vector3[4];

    public bool isGrey {
        get { return m_IsGrey; }
        set { m_IsGrey = value; UpdateGraphicMat(); }
    }

    protected override void OnEnable() {
        base.OnEnable();
        UpdateGraphicMat();
        Canvas canvas = base.graphic.canvas;
        if (canvas != null) {
            if (canvas.rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                Debug.LogError("Do't Support ScreenSpaceOverlay");
            }
            canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
            canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord3;
        }
    }
    protected override void OnDisable() {
        base.graphic.material = null;
        base.OnDisable();
    }

    void LateUpdate() {
        base.graphic.rectTransform.GetWorldCorners(st_FourVectorArray);
        for (int idx = 0; idx < 4; ++idx) {
            Vector3 last_pos = m_WroldCornersArray[idx];
            Vector3 cur_pos = st_FourVectorArray[idx];
            float pos_diff = Vector3.SqrMagnitude(cur_pos - last_pos);
            if (pos_diff > 1e-6) {
                base.graphic.SetVerticesDirty();
                break;
            }
        }
    }

    void UpdateGraphicMat() {
        if (!this.isActiveAndEnabled) {
            return;
        }
        Material curMat = m_IsGrey ? getMaskGreyMat : getMaskMat;
        base.graphic.material = curMat;
    }

    public override void ModifyMesh(VertexHelper vh) {
        if (!this.isActiveAndEnabled) {
            return;
        }
        base.graphic.rectTransform.GetWorldCorners(m_WroldCornersArray);
        Vector3 world_center_pos = (m_WroldCornersArray[0] + m_WroldCornersArray[2]) * 0.5f;
        float world_min_side = Mathf.Min(
            Vector3.Distance(m_WroldCornersArray[0], m_WroldCornersArray[1]),
            Vector3.Distance(m_WroldCornersArray[0], m_WroldCornersArray[3])
            );
        world_min_side *= 0.5f;

        int v_count = vh.currentVertCount;
        UIVertex ui_v = new UIVertex();
        Vector2 uv1 = new Vector2(world_center_pos.x, world_center_pos.y);
        Vector2 uv2 = new Vector2(world_center_pos.z, world_min_side);
        Vector2 uv3 = new Vector2(m_MsaaVolume, 0);
        for (int idx = 0; idx < v_count; ++idx) {
            vh.PopulateUIVertex(ref ui_v, idx);
            ui_v.uv1 = uv1;
            ui_v.uv2 = uv2;
            ui_v.uv3 = uv3;
            vh.SetUIVertex(ui_v, idx);
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate() {
        UpdateGraphicMat();
        base.graphic.SetVerticesDirty();
        base.OnValidate();
    }
#endif
}
