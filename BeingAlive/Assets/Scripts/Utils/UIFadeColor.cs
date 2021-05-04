using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 支持UI的Image，RawImage和Text进行自定义颜色渐变(兼容九宫格切割的Image)
/// </summary>
[RequireComponent(typeof(Graphic))]
[DisallowMultipleComponent, ExecuteInEditMode]
public class UIFadeColor : BaseMeshEffect {
    [Serializable]
    public class FadeInfo {
        public Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        [Range(0f, 1.000f)]
        public float normalized_dist = 0.5f;
        public FadeInfo(float d) { this.normalized_dist = d; }
    }

#if UNITY_EDITOR
    [SerializeField, Header("Just For Display How Generated Mesh Look Like!")]
    Mesh m_editor_display_mesh;
#endif

    [Space(10)]
    [SerializeField, Range(0, 360)]
    int m_angle = 0;
    [SerializeField]
    List<FadeInfo> m_fade_info_list = new List<FadeInfo>() { new FadeInfo(0), new FadeInfo(1) };

    [SerializeField, HideInInspector]
    int m_last_stream_vertex_count = 100;//For Efficient

    class CutLineData {
        public float normalized_dist;
        Vector2[] pos_array;
        public CutLineData(float d, HashSet<Vector2> hash_pos) {
            this.normalized_dist = d;
            if (hash_pos != null) {
                this.pos_array = new Vector2[hash_pos.Count];
                int index = 0;
                foreach (var pos in hash_pos) {
                    this.pos_array[index] = pos;
                    ++index;
                }
            }
        }
        public void AddAllPosToList(ref List<Vector2> list) {
            if (this.pos_array == null)
                return;
            foreach (var pos in this.pos_array)
                list.Add(pos);
        }
    }

    public void SetFadeInfoList(int fade_angle, FadeInfo[] info_array) {
        m_angle = fade_angle;
        m_fade_info_list.Clear();
        m_fade_info_list.AddRange(info_array);
        base.graphic.SetVerticesDirty();
    }

    public override void ModifyMesh(VertexHelper vh) {
        if (Debug.isDebugBuild) {
            UnityEngine.Profiling.Profiler.BeginSample("Profiler_UIFadeColor");
        }
        int fade_info_total_count = m_fade_info_list.Count;
        if (!this.isActiveAndEnabled || fade_info_total_count < 2) {
            return;
        }
        float target_rad = m_angle * Mathf.Deg2Rad;
        Vector2 target_dir = new Vector2(Mathf.Cos(target_rad), Mathf.Sin(target_rad)).normalized;
        Vector2 whole_min_pos = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 whole_max_pos = new Vector2(float.MinValue, float.MinValue);
        int cur_vertex_count = vh.currentVertCount;
        Vector2[] whole_pos_array = new Vector2[cur_vertex_count];
        for (int i = 0; i < cur_vertex_count; ++i) {
            UIVertex ui_v = new UIVertex();
            vh.PopulateUIVertex(ref ui_v, i);
            Vector2 v_pos = ui_v.position;
            whole_min_pos = Vector2.Min(whole_min_pos, v_pos);
            whole_max_pos = Vector2.Max(whole_max_pos, v_pos);
            whole_pos_array[i] = v_pos;
        }
        Vector2 whole_center_pos = (whole_min_pos + whole_max_pos) * 0.5f;
        float max_dir_dist = 0;
        foreach (var v_pos in whole_pos_array) {
            max_dir_dist = Mathf.Max(Mathf.Abs(Vector2.Dot(v_pos - whole_center_pos, target_dir)), max_dir_dist);
        }
        Vector2 dir_max_pos = whole_center_pos + target_dir * max_dir_dist;
        Vector2 dir_min_pos = whole_center_pos - target_dir * max_dir_dist;
        float dir_near_far_dist = Vector2.Distance(dir_min_pos, dir_max_pos);

        List<UIVertex> ret_triangle_stream_list = new List<UIVertex>(m_last_stream_vertex_count);
        List<UIVertex> ui_vertex_stream_list = new List<UIVertex>();
        vh.GetUIVertexStream(ui_vertex_stream_list);
        List<CutLineData> tri_cut_data_list = new List<CutLineData>(fade_info_total_count + 1);
        HashSet<Vector2> tri_cut_pos_hash = new HashSet<Vector2>();
        List<Vector2> cycle_pos_list = new List<Vector2>(9);
        for (int index = 0; index + 2 < ui_vertex_stream_list.Count; index += 3) {
            UIVertex ui_v1 = ui_vertex_stream_list[index];
            UIVertex ui_v2 = ui_vertex_stream_list[index + 1];
            UIVertex ui_v3 = ui_vertex_stream_list[index + 2];
            Vector2 pos_v1 = ui_v1.position;
            Vector2 pos_v2 = ui_v2.position;
            Vector2 pos_v3 = ui_v3.position;
            tri_cut_data_list.Clear();
            for (int idx = 1; idx + 1 < fade_info_total_count; ++idx) {
                FadeInfo fade_info = m_fade_info_list[idx];
                Vector2 dir_v_pos = Vector2.Lerp(dir_min_pos, dir_max_pos, fade_info.normalized_dist);
                Vector2 meet_pos;
                tri_cut_pos_hash.Clear();
                if (CalLineAndSegmentMeetPos(pos_v1, pos_v2, dir_v_pos, target_dir, out meet_pos)) {
                    tri_cut_pos_hash.Add(meet_pos);
                }
                if (CalLineAndSegmentMeetPos(pos_v1, pos_v3, dir_v_pos, target_dir, out meet_pos)) {
                    tri_cut_pos_hash.Add(meet_pos);
                }
                if (CalLineAndSegmentMeetPos(pos_v2, pos_v3, dir_v_pos, target_dir, out meet_pos)) {
                    tri_cut_pos_hash.Add(meet_pos);
                }
                if (tri_cut_pos_hash.Count > 0) {
                    CutLineData cut_data = new CutLineData(fade_info.normalized_dist, tri_cut_pos_hash);
                    tri_cut_data_list.Add(cut_data);
                }
            }
            List<UIVertex> cur_triangle_list = null;
            if (tri_cut_data_list.Count > 0) {
                cur_triangle_list = new List<UIVertex>(4 * (tri_cut_data_list.Count + 1) * 3);
                List<Vector2> origin_tri_pos_list = new List<Vector2>(3) { pos_v1, pos_v2, pos_v3 };
                tri_cut_data_list.Add(new CutLineData(1.1f, null));
                for (int k = 0; k < tri_cut_data_list.Count; ++k) {
                    CutLineData cut_data = tri_cut_data_list[k];
                    cycle_pos_list.Clear();
                    for (int j = origin_tri_pos_list.Count - 1; j >= 0; --j) {
                        Vector2 origin_pos = origin_tri_pos_list[j];
                        float dir_dist = Vector2.Dot(origin_pos - dir_min_pos, target_dir) / dir_near_far_dist;
                        if (dir_dist < cut_data.normalized_dist) {
                            cycle_pos_list.Add(origin_pos);
                            origin_tri_pos_list.RemoveAt(j);
                        }
                    }
                    cut_data.AddAllPosToList(ref cycle_pos_list);
                    if (k > 0) {
                        tri_cut_data_list[k - 1].AddAllPosToList(ref cycle_pos_list);
                    }
                    RecalVerticesStreamList(cycle_pos_list, cur_triangle_list);
                }
                //Recal Other Vertex Data(normal, uv, uv0 ...)
                RecalVerticesUV(ui_v1, ui_v2, ui_v3, cur_triangle_list);
            }

            if (cur_triangle_list != null) {
                ret_triangle_stream_list.AddRange(cur_triangle_list);
            } else {
                ret_triangle_stream_list.Add(ui_v1);
                ret_triangle_stream_list.Add(ui_v2);
                ret_triangle_stream_list.Add(ui_v3);
            }
        }

        m_last_stream_vertex_count = ret_triangle_stream_list.Count;
        //Recal Vertex Color
        for (int idx = 0; idx < m_last_stream_vertex_count; ++idx) {
            UIVertex ui_v = ret_triangle_stream_list[idx];
            Vector2 cur_ui_pos = ui_v.position;
            float pos_lerp_dist = Vector2.Dot(cur_ui_pos - dir_min_pos, target_dir) / dir_near_far_dist;
            FadeInfo close_min_info = m_fade_info_list[0];
            FadeInfo close_max_info = m_fade_info_list[fade_info_total_count - 1];
            foreach (var info in m_fade_info_list) {
                if (info.normalized_dist <= pos_lerp_dist && info.normalized_dist > close_min_info.normalized_dist)
                    close_min_info = info;
                if (info.normalized_dist >= pos_lerp_dist && info.normalized_dist < close_max_info.normalized_dist)
                    close_max_info = info;
            }
            float delta_pos_dist = close_max_info.normalized_dist - close_min_info.normalized_dist;
            float lerp_pos_v = delta_pos_dist > 0 ? (pos_lerp_dist - close_min_info.normalized_dist) / delta_pos_dist : 0;
            ui_v.color = Color32.Lerp(close_min_info.color, close_max_info.color, lerp_pos_v);
            ret_triangle_stream_list[idx] = ui_v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(ret_triangle_stream_list);
        if (Debug.isDebugBuild) {
            UnityEngine.Profiling.Profiler.EndSample();
        }

#if UNITY_EDITOR
        if (Application.isEditor) {
            m_editor_display_mesh = new Mesh();
            m_editor_display_mesh.name = "ImageFadeColor_" + gameObject.GetInstanceID();
            vh.FillMesh(m_editor_display_mesh);
        }
#endif
    }
    /// <summary>
    /// 计算三角形内部点的UV。方法很笨，且只适用于直角三角形。目前还没有更好的方法（能适用所有三角形）
    /// </summary>
    /// <param name="tri_v1"></param>
    /// <param name="tri_v2"></param>
    /// <param name="tri_v3"></param>
    /// <param name="stream_list"></param>
    void RecalVerticesUV(UIVertex tri_v1, UIVertex tri_v2, UIVertex tri_v3, List<UIVertex> stream_list) {
        UIVertex vertical_v;
        UIVertex side_v_2;
        UIVertex side_v_3;
        Vector2 segment_1_2 = tri_v1.position - tri_v2.position;
        Vector2 segment_1_3 = tri_v1.position - tri_v3.position;
        Vector2 segment_2_3 = tri_v2.position - tri_v3.position;
        if (Mathf.Abs(Vector2.Dot(segment_1_2, segment_1_3)) < 1e-6) {
            vertical_v = tri_v1;
            side_v_2 = tri_v2;
            side_v_3 = tri_v3;
        } else if (Mathf.Abs(Vector2.Dot(segment_1_2, segment_2_3)) < 1e-6) {
            vertical_v = tri_v2;
            side_v_2 = tri_v3;
            side_v_3 = tri_v1;
        } else {
            vertical_v = tri_v3;
            side_v_2 = tri_v1;
            side_v_3 = tri_v2;
        }
        Vector2 u_pos_dir, v_pos_dir;
        Vector2 end_uv = new Vector2();
        Vector2 ver_pos = vertical_v.position;
        Vector2 ver_uv0 = vertical_v.uv0;
        if (Mathf.Abs(ver_uv0.x - side_v_2.uv0.x) < 1e-6) {
            end_uv.x = side_v_3.uv0.x;
            end_uv.y = side_v_2.uv0.y;
            u_pos_dir = side_v_3.position - vertical_v.position;
            v_pos_dir = side_v_2.position - vertical_v.position;
        } else {
            end_uv.x = side_v_2.uv0.x;
            end_uv.y = side_v_3.uv0.y;
            u_pos_dir = side_v_2.position - vertical_v.position;
            v_pos_dir = side_v_3.position - vertical_v.position;
        }
        Vector2 pos_dist = new Vector2(u_pos_dir.magnitude, v_pos_dir.magnitude);
        Vector2 normalized_u_pos_dir = u_pos_dir.normalized;
        Vector2 normalized_v_pos_dir = v_pos_dir.normalized;
        Vector2 cur_uv0 = new Vector2();
        int stream_vertex_count = stream_list.Count;
        for (int i = 0; i < stream_vertex_count; ++i) {
            UIVertex v = stream_list[i];
            Vector2 v_pos = v.position;
            float u_lerp = Mathf.Abs(Vector2.Dot(normalized_u_pos_dir, v_pos - ver_pos)) / pos_dist.x;
            cur_uv0.x = Mathf.Lerp(ver_uv0.x, end_uv.x, u_lerp);
            float v_lerp = Mathf.Abs(Vector2.Dot(normalized_v_pos_dir, v_pos - ver_pos)) / pos_dist.y;
            cur_uv0.y = Mathf.Lerp(ver_uv0.y, end_uv.y, v_lerp);
            v.uv0 = cur_uv0;
            stream_list[i] = v;
        }
    }

    /// <summary>
    /// 用直线方程的斜截式和一般式求相交点
    /// </summary>
    /// <param name="ret_pos"></param>
    /// <returns></returns>
    bool CalLineAndSegmentMeetPos(Vector2 segment_pos1, Vector2 segment_pos2, Vector2 line_pos, Vector2 line_vertical_dir, out Vector2 ret_pos) {
        ret_pos = new Vector2();
        Vector2 segment_dir = (segment_pos2 - segment_pos1).normalized;
        if (Mathf.Abs(Vector2.Dot(segment_dir, line_vertical_dir)) < 1e-6) {
            return false;
        }
        Vector2 min_segment_pos = Vector2.Min(segment_pos1, segment_pos2);
        Vector2 max_segment_pos = Vector2.Max(segment_pos1, segment_pos2);
        //一般式 ax + by + c = 0
        //y = (-ax - c) / b
        //y = -ax / b - c / b
        float a = segment_pos2.y - segment_pos1.y;
        float b = segment_pos1.x - segment_pos2.x;
        float c = segment_pos2.x * segment_pos1.y - segment_pos1.x * segment_pos2.y;

        bool is_segment_parallel_x = Mathf.Abs(a) < 1e-6;
        bool is_segment_parallel_y = Mathf.Abs(b) < 1e-6;
        float meet_x;
        float meet_y;
        if (is_segment_parallel_y) {
            meet_x = segment_pos1.x;
            meet_y = -line_vertical_dir.x * (meet_x - line_pos.x) / line_vertical_dir.y + line_pos.y;
        } else if (is_segment_parallel_x) {
            meet_y = segment_pos1.y;
            meet_x = -line_vertical_dir.y * (meet_y - line_pos.y) / line_vertical_dir.x + line_pos.x;
        } else {
            if (Mathf.Abs(line_vertical_dir.y) < 1e-6) {//Do't Have k
                meet_x = line_pos.x;
                meet_y = (-a * meet_x - c) / b;
            } else {
                //斜截式 y = kx + m
                float k = -1 / (line_vertical_dir.y / line_vertical_dir.x);
                float m = line_pos.y - k * line_pos.x;
                //kx + m = -ax / b - c/b;
                //kx + ax / b = - c/b - m;
                //x(k + a/b) = -c/b - m;
                //x = (-c/b - m) / (k + a/b);
                meet_x = (-c / b - m) / (k + a / b);
                meet_y = k * meet_x + m;
            }
        }
        if (meet_x >= min_segment_pos.x && meet_x <= max_segment_pos.x && meet_y >= min_segment_pos.y && meet_y <= max_segment_pos.y) {
            ret_pos.x = meet_x;
            ret_pos.y = meet_y;
            return true;
        }
        return false;
    }

    void RecalVerticesStreamList(List<Vector2> pos_list, List<UIVertex> vertex_stream_list) {
        Vector2 total_sum_pos = Vector2.zero;
        foreach (var pos in pos_list) {
            total_sum_pos.x += pos.x;
            total_sum_pos.y += pos.y;
        }
        int total_pos_count = pos_list.Count;
        m_pos_sort_center_pos = total_sum_pos / total_pos_count;
        pos_list.Sort(this.PosSortFunc);
        UIVertex center_ui_v = new UIVertex();
        center_ui_v.position = m_pos_sort_center_pos;
        UIVertex ui_v = new UIVertex();
        for (int idx = 0; idx < total_pos_count; ++idx) {
            vertex_stream_list.Add(center_ui_v);

            ui_v.position = pos_list[idx];
            vertex_stream_list.Add(ui_v);

            int next_idx = idx + 1;
            ui_v.position = pos_list[next_idx == total_pos_count ? 0 : next_idx];
            vertex_stream_list.Add(ui_v);
        }
    }
    Vector2 m_pos_sort_center_pos;
    int PosSortFunc(Vector2 pos_a, Vector2 pos_b) {
        Vector2 kCompareDir = new Vector2(1, 0);
        float offset_angle_a = Vector2.SignedAngle(pos_a - m_pos_sort_center_pos, kCompareDir);
        offset_angle_a = offset_angle_a < 0 ? offset_angle_a + 360 : offset_angle_a;
        float offset_angle_b = Vector2.SignedAngle(pos_b - m_pos_sort_center_pos, kCompareDir);
        offset_angle_b = offset_angle_b < 0 ? offset_angle_b + 360 : offset_angle_b;
        if (offset_angle_a > offset_angle_b) {
            return 1;
        } else if (offset_angle_a < offset_angle_b) {
            return -1;
        } else {
            return 0;
        }
    }


#if UNITY_EDITOR
    protected override void OnValidate() {
        CheckFadeListData();
        base.graphic.SetVerticesDirty();
    }

    void CheckFadeListData() {
        if (m_fade_info_list.Count < 2) {
            Debug.LogWarning("[UIImageFadeColor] Require At Least Two FadeInfo Data");
            m_fade_info_list = new List<FadeInfo>() { new FadeInfo(0), new FadeInfo(1) };
        }
        int count = m_fade_info_list.Count;
        m_fade_info_list[0].normalized_dist = 0;
        m_fade_info_list[count - 1].normalized_dist = 1;
        for (int idx = 1; idx + 1 < count; ++idx) {
            var info = m_fade_info_list[idx];
            var last_info = m_fade_info_list[idx - 1];
            var next_info = m_fade_info_list[idx + 1];
            info.normalized_dist = Mathf.Clamp(info.normalized_dist, last_info.normalized_dist + 0.001f, next_info.normalized_dist - 0.001f);
        }
    }
#endif
}
