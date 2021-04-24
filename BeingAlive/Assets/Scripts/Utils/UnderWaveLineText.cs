// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// /// <summary>
// /// 扩展下划波浪线
// /// </summary>
// public class UnderWaveLineText : Text {
//
//     static UIVertex[] st_FourVertexArray = new UIVertex[4];
//
//     [SerializeField]
//     UnderWaveLinePlayer.WaveAttr m_UnderWaveLineAttr = new UnderWaveLinePlayer.WaveAttr();
//
//     string m_ConvertText;
//     UnderWaveLinePlayer m_UnderWavePlayer = new UnderWaveLinePlayer();
//
//     public override Material defaultMaterial {
//         get {
//             return UnderWaveLinePlayer.getUIMaterial;
//         }
//     }
//
//     public override string text {
//         get { return m_Text; }
//         set {
//             if (string.IsNullOrEmpty(value)) {
//                 if (!string.IsNullOrEmpty(m_Text)) {
//                     m_Text = "";
//                     UpdateConvertText();
//                     SetVerticesDirty();
//                 }
//             } else if (m_Text != value) {
//                 m_Text = value;
//                 UpdateConvertText();
//                 SetVerticesDirty();
//                 SetLayoutDirty();
//             }
//         }
//     }
//
//     public override float preferredWidth {
//         get {
//             TextGenerationSettings settings = base.GetGenerationSettings(Vector2.zero);
//             return base.cachedTextGeneratorForLayout.GetPreferredWidth(m_ConvertText, settings) / base.pixelsPerUnit;
//         }
//     }
//
//     public override float preferredHeight {
//         get {
//             TextGenerationSettings settings = base.GetGenerationSettings(new Vector2(base.GetPixelAdjustedRect().size.x, 0f));
//             return base.cachedTextGeneratorForLayout.GetPreferredHeight(m_ConvertText, settings) / base.pixelsPerUnit;
//         }
//     }
//
//     protected override void Awake() {
//         base.Awake();
//         UpdateConvertText();
//     }
//
//     protected override void OnPopulateMesh(VertexHelper toFill) {
//         if (base.font == null) {
//             return;
//         }
//         base.m_DisableFontTextureRebuiltCallback = true;
//         Vector2 extents = base.rectTransform.rect.size;
//         TextGenerationSettings settings = base.GetGenerationSettings(extents);
//         cachedTextGenerator.PopulateWithErrors(m_ConvertText, settings, base.gameObject);
//         IList<UIVertex> verts = cachedTextGenerator.verts;
//         float unitsPerPixel = 1f / base.pixelsPerUnit;
//         int vertCount = verts.Count - 4;
//         Vector2 roundingOffset2 = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
//         roundingOffset2 = base.PixelAdjustPoint(roundingOffset2) - roundingOffset2;
//         toFill.Clear();
//         if (roundingOffset2 != Vector2.zero) {
//             for (int j = 0; j < vertCount; j++) {
//                 int tempVertsIndex2 = j & 3;
//                 st_FourVertexArray[tempVertsIndex2] = verts[j];
//                 st_FourVertexArray[tempVertsIndex2].position *= unitsPerPixel;
//                 st_FourVertexArray[tempVertsIndex2].position.x += roundingOffset2.x;
//                 st_FourVertexArray[tempVertsIndex2].position.y += roundingOffset2.y;
//                 if (tempVertsIndex2 == 3) {
//                     toFill.AddUIVertexQuad(st_FourVertexArray);
//                 }
//             }
//         } else {
//             for (int i = 0; i < vertCount; i++) {
//                 int tempVertsIndex = i & 3;
//                 st_FourVertexArray[tempVertsIndex] = verts[i];
//                 st_FourVertexArray[tempVertsIndex].position *= unitsPerPixel;
//                 if (tempVertsIndex == 3) {
//                     toFill.AddUIVertexQuad(st_FourVertexArray);
//                 }
//             }
//         }
//         m_UnderWavePlayer.PopulateMesh(toFill);
//         base.m_DisableFontTextureRebuiltCallback = false;
//     }
//
//     void UpdateConvertText() {
//         if (m_UnderWavePlayer != null) {
//             m_ConvertText = m_UnderWavePlayer.GetConvertText(base.m_Text, m_UnderWaveLineAttr);
//         } else {
//             m_ConvertText = base.m_Text;
//         }
//     }
//
//     public UnderWaveLinePlayer.WaveAttr underWaveAttr {
//         get { return m_UnderWaveLineAttr; }
//         set {
//             if (value != null) {
//                 m_UnderWaveLineAttr = value;
//                 UpdateConvertText();
//                 base.SetVerticesDirty();
//             }
//         }
//     }
//
// #if UNITY_EDITOR
//     protected override void OnValidate() {
//         UpdateConvertText();
//         base.OnValidate();
//     }
// #endif
// }
