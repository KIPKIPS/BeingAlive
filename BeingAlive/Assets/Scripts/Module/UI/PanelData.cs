using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelData {
    public int id { get; set; }
    public string name { get; set; }
    public string path { get; set; }
    //public UIManager.PanelType panelType { get; set; }
    public PanelData(int _id, string _path, string _name) {//, UIManager.PanelType _panelType
        id = _id;
        path = _path;
        name = _name;
        //panelType = _panelType;
    }
}
