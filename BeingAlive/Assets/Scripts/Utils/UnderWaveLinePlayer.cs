using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

//*************************************************
//****copyRight LuoYao 2020/11/17
//*************************************************

public class UnderWaveLinePlayer {

    [Serializable]
    public class WaveAttr {
        [Range(0f, 20f)]
        public float strength = 0f;
        [Range(1f, 20f)]
        public float length = 2.5f;
        [Range(1f, 20f)]
        public float thickness = 1f;
        public float offset = 0;
        public Color color = Color.white;
    }

    class ItemInfo {
        public float min_x = float.MaxValue;
        public float max_x = float.MinValue;
        public float y = float.MaxValue;
        public WaveAttr wave_attr;
    }

    class CharIndexInfo {
        public int start;
        public int end;
        public WaveAttr wave_attr;
        public CharIndexInfo(int s_idx, int e_idx) {
            this.start = s_idx;
            this.end = e_idx;
        }
    }

    List<CharIndexInfo> m_CharIndexInfoList = new List<CharIndexInfo>();


    static Material st_UIMateril;
    static char[] st_AttrSplitCharArray = new char[1] { ',' };
    static UIVertex[] st_FourVertexArray = new UIVertex[4];

    const string kAttrMarkStart = "<UnderWave/";
    const string kAttrMarkEnd = ">";
    const string kWholeEndMark = "</UnderWave>";

    public static Material getUIMaterial {
        get {
            if (st_UIMateril == null) {
                st_UIMateril = new Material(Shader.Find("UI/xProject-Default-UnderWaveLine"));
                st_UIMateril.name = "Static_UnderWaveLine";
                st_UIMateril.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            }
            return st_UIMateril;
        }
    }

    public string GetConvertText(string text_str, WaveAttr base_wave_attr) {
        if (Debug.isDebugBuild) {
            UnityEngine.Profiling.Profiler.BeginSample("UnderWaveLinePlayer.GetConvertText");
        }
        m_CharIndexInfoList.Clear();
        int kAttrMarkStartLength = kAttrMarkStart.Length;
        int kAttrMarkEndLength = kAttrMarkEnd.Length;
        int kWholeEndMarkLength = kWholeEndMark.Length;

        for (int i = 0; i < 1000; ++i) {
            int attr_mark_start_idx = text_str.IndexOf(kAttrMarkStart);
            int attr_mark_end_idx = -1;
            int total_str_length = text_str.Length;
            if (attr_mark_start_idx + kAttrMarkStartLength < total_str_length) {
                attr_mark_end_idx = text_str.IndexOf(kAttrMarkEnd, attr_mark_start_idx + kAttrMarkStartLength);
            }
            int whole_end_index = -1;
            if (attr_mark_end_idx + kAttrMarkEndLength < total_str_length) {
                whole_end_index = text_str.IndexOf(kWholeEndMark, attr_mark_end_idx + kAttrMarkEndLength);
            }
            if (attr_mark_start_idx < 0 || attr_mark_end_idx <= 0 || whole_end_index <= 0) {
                break;
            }
            int actual_start_index = attr_mark_start_idx + kAttrMarkStartLength;
            int attr_content_length = attr_mark_end_idx + kAttrMarkEndLength - attr_mark_start_idx;
            string all_attr_content = text_str.Substring(actual_start_index, attr_mark_end_idx - actual_start_index);
            CharIndexInfo index_info = new CharIndexInfo(attr_mark_start_idx, whole_end_index - attr_content_length - 1);
            WaveAttr wave_attr = new WaveAttr();
            wave_attr.color = base_wave_attr.color;
            wave_attr.strength = base_wave_attr.strength;
            wave_attr.length = base_wave_attr.length;
            wave_attr.thickness = base_wave_attr.thickness;
            wave_attr.offset = base_wave_attr.offset;
            index_info.wave_attr = wave_attr;
            m_CharIndexInfoList.Add(index_info);
            text_str = text_str.Remove(whole_end_index, kWholeEndMarkLength);
            text_str = text_str.Remove(attr_mark_start_idx, attr_content_length);

            string[] attr_str_array = all_attr_content.Split(st_AttrSplitCharArray, StringSplitOptions.None);
            if (attr_str_array != null) {
                string kEmptyStr = string.Empty;
                foreach (var orgin_str in attr_str_array) {
                    string str = orgin_str.Replace("=", kEmptyStr);
                    float f_value;
                    if (str.Contains("color")) {
                        Color ret_color;
                        if (ColorUtility.TryParseHtmlString(str.Replace("color", kEmptyStr), out ret_color))
                            wave_attr.color = ret_color;
                    } else if (str.Contains("strength")) {
                        if (float.TryParse(str.Replace("strength", kEmptyStr), out f_value))
                            wave_attr.strength = Mathf.Max(0, f_value);
                    } else if (str.Contains("length")) {
                        if (float.TryParse(str.Replace("length", kEmptyStr), out f_value))
                            wave_attr.length = Mathf.Max(0.1f, f_value);
                    } else if (str.Contains("thickness")) {
                        if (float.TryParse(str.Replace("thickness", kEmptyStr), out f_value))
                            wave_attr.thickness = Mathf.Max(0.1f, f_value);
                    } else if (str.Contains("offset")) {
                        if (float.TryParse(str.Replace("offset", kEmptyStr), out f_value))
                            wave_attr.offset = f_value;
                    }
                }
            }
        }
        if (Debug.isDebugBuild) {
            UnityEngine.Profiling.Profiler.EndSample();
        }
        return text_str;
    }

    public void PopulateMesh(VertexHelper vh) {
        if (vh.currentVertCount == 0 || m_CharIndexInfoList.Count == 0) {
            return;
        }
        if (Debug.isDebugBuild) {
            UnityEngine.Profiling.Profiler.BeginSample("UnderWaveLinePlayer.PopulateMesh");
        }
        List<UIVertex> m_vertex_stream_list = new List<UIVertex>();
        vh.GetUIVertexStream(m_vertex_stream_list);
        int total_stream_count = m_vertex_stream_list.Count;
        List<ItemInfo> item_info_list = new List<ItemInfo>();
        Vector3 kLocalRightDir = Vector3.right;
        int cur_item_index = -1;
        foreach (var index_info in m_CharIndexInfoList) {
            if (index_info.start < 0 || index_info.end < index_info.start) {
                continue;
            }
            item_info_list.Add(new ItemInfo());
            ++cur_item_index;
            for (int char_idx = index_info.start; char_idx <= index_info.end; ++char_idx) {
                int start_vertex_idx = char_idx * 6;
                if (start_vertex_idx >= total_stream_count) {
                    break;
                }
                Vector3 pos0 = m_vertex_stream_list[start_vertex_idx].position;
                Vector3 pos1 = m_vertex_stream_list[start_vertex_idx + 1].position;
                Vector3 pos2 = m_vertex_stream_list[start_vertex_idx + 2].position;

                float min_x = Mathf.Min(Mathf.Min(pos0.x, pos1.x), pos2.x);
                float max_x = Mathf.Max(Mathf.Max(pos0.x, pos1.x), pos2.x);
                float min_y = Mathf.Min(Mathf.Min(pos0.y, pos1.y), pos2.y);
                ItemInfo info = item_info_list[cur_item_index];
                info.min_x = Mathf.Min(info.min_x, min_x);
                info.max_x = Mathf.Max(info.max_x, max_x);
                info.y = Mathf.Min(info.y, min_y);
                info.wave_attr = index_info.wave_attr;

                int next_start_vertex_idx = start_vertex_idx + 6;
                if (next_start_vertex_idx + 5 < total_stream_count) {
                    Vector3 pos3 = m_vertex_stream_list[start_vertex_idx + 3].position;
                    Vector3 pos4 = m_vertex_stream_list[start_vertex_idx + 4].position;
                    Vector3 pos5 = m_vertex_stream_list[start_vertex_idx + 5].position;
                    Vector3 cur_center = (pos0 + pos1 + pos2 + pos3 + pos4 + pos5) / 6;

                    Vector3 pos10 = m_vertex_stream_list[next_start_vertex_idx].position;
                    Vector3 pos11 = m_vertex_stream_list[next_start_vertex_idx + 1].position;
                    Vector3 pos12 = m_vertex_stream_list[next_start_vertex_idx + 2].position;
                    Vector3 pos13 = m_vertex_stream_list[next_start_vertex_idx + 3].position;
                    Vector3 pos14 = m_vertex_stream_list[next_start_vertex_idx + 4].position;
                    Vector3 pos15 = m_vertex_stream_list[next_start_vertex_idx + 5].position;
                    Vector3 next_center = (pos10 + pos11 + pos12 + pos13 + pos14 + pos15) / 6;

                    float angle = Vector3.Angle(next_center - cur_center, kLocalRightDir);
                    if (angle > 60 && next_center != cur_center) {
                        item_info_list.Add(new ItemInfo());
                        ++cur_item_index;
                    }
                }
            }
        }
        GenerateWaveMesh(vh, item_info_list);
        if (Debug.isDebugBuild) {
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    void GenerateWaveMesh(VertexHelper vh, List<ItemInfo> item_info_list) {
        UIVertex ui_v = new UIVertex();
        ui_v.uv0 = new Vector2(-1, -1);
        List<UIVertex> item_vertex_list = new List<UIVertex>();
        foreach (var item_info in item_info_list) {
            WaveAttr cur_wave_attr = item_info.wave_attr;
            if (cur_wave_attr == null)
                continue;
            float cur_pos_y = item_info.y + cur_wave_attr.offset;
            ui_v.color = cur_wave_attr.color;
            //line
            if (cur_wave_attr.strength <= 0.01f) {
                ui_v.position = new Vector3(item_info.min_x, cur_pos_y - cur_wave_attr.thickness, 0);
                st_FourVertexArray[0] = ui_v;
                ui_v.position = new Vector3(item_info.min_x, cur_pos_y, 0);
                st_FourVertexArray[1] = ui_v;
                ui_v.position = new Vector3(item_info.max_x, cur_pos_y, 0);
                st_FourVertexArray[2] = ui_v;
                ui_v.position = new Vector3(item_info.max_x, cur_pos_y - cur_wave_attr.thickness, 0);
                st_FourVertexArray[3] = ui_v;
                vh.AddUIVertexQuad(st_FourVertexArray);
                continue;
            }
            //wave
            float cur_x = item_info.min_x;
            item_vertex_list.Clear();
            bool is_arrive = false;
            float angle_norm = 180f / cur_wave_attr.length;
            for (int i = 0; i < 1000; ++i) {
                if (cur_x >= item_info.max_x) {
                    is_arrive = true;
                    cur_x = item_info.max_x;
                }
                float rad = Mathf.Deg2Rad * (angle_norm * (cur_x - item_info.min_x));
                float cur_y = cur_pos_y - (Mathf.Cos(rad) + 1) * 0.5f * cur_wave_attr.strength;
                ui_v.position = new Vector3(cur_x, cur_y, 0);
                item_vertex_list.Add(ui_v);
                if (is_arrive) {
                    break;
                }
                cur_x += cur_wave_attr.length;
            }
            for (int idx = 0; idx < item_vertex_list.Count - 1; ++idx) {
                UIVertex v1 = item_vertex_list[idx];
                UIVertex v2 = item_vertex_list[idx + 1];

                UIVertex v1_down = ui_v;
                v1_down.position = new Vector3(v1.position.x, v1.position.y - cur_wave_attr.thickness, 0);
                UIVertex v2_down = ui_v;
                v2_down.position = new Vector3(v2.position.x, v2.position.y - cur_wave_attr.thickness, 0);

                st_FourVertexArray[0] = v1_down;
                st_FourVertexArray[1] = v1;
                st_FourVertexArray[2] = v2;
                st_FourVertexArray[3] = v2_down;
                vh.AddUIVertexQuad(st_FourVertexArray);
            }
        }
    }
}
