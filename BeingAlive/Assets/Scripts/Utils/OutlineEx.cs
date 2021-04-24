using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Game.Logic {
    /// <summary>
    /// 给UI上面的字体，图形等加上描边
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class OutlineEx : BaseMeshEffect {
        public Color OutlineColor = Color.white;
        [Range(1.0f, 3.0f)]
        public float OutlineWidth = 1;

        //[SerializeField, Header("性能受品质影响；当Offset太大造成描边效果不好时，可以适当增加品质")]
        SampleQuality m_quality = SampleQuality.Low_X8;
        enum SampleQuality { Low_X8, Normal_X12, High_X18, VeryHigh_X26 }

        List<Vector2> m_offset_pos_list = new List<Vector2>();

        public void ChangeOutlineColor(Color col) {
            OutlineColor = col;
            base.graphic.SetVerticesDirty();
        }

        public void ChangeOutlineColor(Color col, float width) {
            OutlineColor = col;
            if (OutlineWidth != width) {
                OutlineWidth = width;
                RecalOffsetPosList();
            }
            base.graphic.SetVerticesDirty();
        }

        protected override void OnEnable() {
            base.OnEnable();
            var mat = base.graphic.material;
            if (mat != null && mat.name.Contains("Outline")) {
                base.graphic.material = null;
            }
            RecalOffsetPosList();
        }

        void RecalOffsetPosList() {
            int sample_cicle_count = 0;
            int sample_count = 0;
            switch (m_quality) {
                case SampleQuality.Low_X8: { sample_cicle_count = 1; sample_count = 8; break; }
                case SampleQuality.Normal_X12: { sample_cicle_count = 1; sample_count = 12; break; }
                case SampleQuality.High_X18: { sample_cicle_count = 2; sample_count = 8; break; }
                case SampleQuality.VeryHigh_X26: { sample_cicle_count = 2; sample_count = 12; break; }
            }
            m_offset_pos_list.Clear();
            int cur_sample_count = sample_count;
            Vector2 pos_offset = Vector2.one * (OutlineWidth + 0.2f);
            for (int i = 1; i <= sample_cicle_count; i++) {
                var offset_pos = (pos_offset / sample_cicle_count) * i;
                float rad_step = 2 * Mathf.PI / cur_sample_count;
                float rad = (i % 2) * rad_step * 0.5f;
                for (int j = 0; j < cur_sample_count; j++) {
                    m_offset_pos_list.Add(new Vector2(offset_pos.x * Mathf.Cos(rad), offset_pos.y * Mathf.Sin(rad)));
                    rad += rad_step;
                }
                cur_sample_count += 2;
            }
        }

        public override void ModifyMesh(VertexHelper vh) {
            if (!this.isActiveAndEnabled || vh.currentVertCount == 0) return;
            List<UIVertex> list = new List<UIVertex>();
            vh.GetUIVertexStream(list);
            vh.Clear();
            GenerateOutlineMesh(list);
            vh.AddUIVertexTriangleStream(list);
        }

        void GenerateOutlineMesh(List<UIVertex> ui_vertex_list) {
            int need_vertex_count = ui_vertex_list.Count * (m_offset_pos_list.Count + 1);
            if (ui_vertex_list.Capacity < need_vertex_count) {
                ui_vertex_list.Capacity = need_vertex_count;
            }
            int start_index = 0;
            int end_index = 0;
            Color32 target_col = new Color32();
            target_col.r = Convert.ToByte(OutlineColor.r * byte.MaxValue);
            target_col.g = Convert.ToByte(OutlineColor.g * byte.MaxValue);
            target_col.b = Convert.ToByte(OutlineColor.b * byte.MaxValue);
            for (int i = 0; i < m_offset_pos_list.Count; ++i) {
                end_index = ui_vertex_list.Count;
                var offset_pos = m_offset_pos_list[i];
                for (int k = start_index; k < end_index; ++k) {
                    var vt = ui_vertex_list[k];
                    ui_vertex_list.Add(vt);
                    Vector3 pos = vt.position;
                    pos.x += offset_pos.x;
                    pos.y += offset_pos.y;
                    vt.position = pos;
                    target_col.a = vt.color.a;
                    vt.color = target_col;
                    ui_vertex_list[k] = vt;
                }
                start_index = end_index;
                end_index = ui_vertex_list.Count;
            }
        }


#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            RecalOffsetPosList();
            base.graphic.SetVerticesDirty();
        }
#endif
    }
}