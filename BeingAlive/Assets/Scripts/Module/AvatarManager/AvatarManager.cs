using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoSingleton<AvatarManager> {
    private GameObject modelSource;//模型的资源
    private Transform modelTrs;
    private GameObject modelEntity;//模型的表现个体,骨架

    //存储模型的结构数据 <身体部位,<部位索引,SkinMeshRenderer>>
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> modelDict = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    private Dictionary<string, SkinnedMeshRenderer> entitySKMDict = new Dictionary<string, SkinnedMeshRenderer>();
    private Transform[] modelHips;

    public override void Start() {
        base.Start();
        InstantiateSource();//加载资源模型
        InstantiateEntity();//加载骨架
        modelHips = modelEntity.transform.Find("Root").GetComponentsInChildren<Transform>();//保存骨架的信息

        SaveModelData();
    }

    void InstantiateSource() {
        modelSource = Instantiate(Resources.Load<GameObject>("Prefabs/Character/ModularCharactersSource"));//资源
        modelSource.name = "ModularCharactersSource";
        modelTrs = modelSource.transform;
        modelSource.SetActive(false);
    }
    void InstantiateEntity() {
        modelEntity = Instantiate(Resources.Load<GameObject>("Prefabs/Character/ModularCharactersEntity"));//资源
        modelEntity.name = "ModularCharactersEntity";
    }

    void SaveModelData() {
        if (modelSource == null) {
            return;
        }
        //查找男性的SkinnedMeshRenderer组件
        SkinnedMeshRenderer[] maleParts = modelSource.transform.Find("Modular_Characters/Male_Parts").GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in maleParts) {
            string name = smr.name;
            int lastUnderlineIndex = -1;
            for (int i = name.Length - 1; i >= 0; i--) {
                if (name[i] == '_') {
                    lastUnderlineIndex = i;
                    break;
                }
            }
            //print(lastUnderlineIndex);
            string key = smr.transform.parent.name;//name.Substring(0, lastUnderlineIndex);
            string partKey = name.Substring(lastUnderlineIndex + 1, name.Length - lastUnderlineIndex - 1);
            //print(partKey);
            if (!modelDict.ContainsKey(key)) {
                modelDict.Add(key, new Dictionary<string, SkinnedMeshRenderer>());

                GameObject partGo = new GameObject();
                partGo.name = key + "_" + partKey;
                partGo.transform.parent = Utils.FindObj<Transform>(modelEntity.transform, key);

                entitySKMDict.Add(key, partGo.AddComponent<SkinnedMeshRenderer>()); //把骨骼上的skm存到entity上
            }
            modelDict[key].Add(key + "_" + partKey, smr);//存储所有的skm信息
            //print(key);
        }
        RandomChange();
    }
    //随机更换
    void RandomChange() {
        Hashtable ignorePart = new Hashtable();
        if (Random.Range(0, 2) % 2 == 0) { //有面罩的话就不显示Head,Eyebrows,FacialHair,会穿模
            ignorePart.Add("Male_Head_All_Elements", true);
            ignorePart.Add("Male_01_Eyebrows", true);
            ignorePart.Add("Male_02_FacialHair", true);
        } else {
            ignorePart.Add("Male_Head_No_Elements", true);
        }
        foreach (KeyValuePair<string, SkinnedMeshRenderer> kvp in entitySKMDict) {
            //print(kvp.Key);
            SkinnedMeshRenderer skm = kvp.Value;
            if (ignorePart.ContainsKey(kvp.Key)) {
                skm.gameObject.SetActive(false);
                continue;
            }
            skm.gameObject.SetActive(true);
            Dictionary<string, SkinnedMeshRenderer> partSkmDict = modelDict[kvp.Key];
            int num = Random.Range(0, partSkmDict.Count);

            //print(num);
            string key = kvp.Key + "_" + (num < 10 ? ("0" + num) : num.ToString());
            //print(partSkmDict[key]);
            if (!partSkmDict.ContainsKey(key)) {
                num++;
                key = kvp.Key + "_" + (num < 10 ? ("0" + num) : num.ToString());
            }
            ReplaceSMRData(skm, partSkmDict[key]);
            // skm.sharedMesh = randomPartSkm.sharedMesh;
        }
    }

    //更换smr组件的bones,material,mesh
    void ReplaceSMRData(SkinnedMeshRenderer modelSMR, SkinnedMeshRenderer target) {
        List<Transform> bones = new List<Transform>();
        foreach (Transform trs in target.bones) {
            foreach (var bone in modelHips) {
                if (trs.name == bone.name) {
                    //print(trs.name);
                    bones.Add(bone);
                    break;
                }
            }
        }
        // foreach (Transform trs in modelHips) {
        //     print(trs.name);
        // }
        modelSMR.bones = bones.ToArray();
        modelSMR.materials = target.materials;
        modelSMR.sharedMesh = target.sharedMesh;
    }

    public override void Update() {
        base.Update();
        if (Input.GetMouseButtonDown(0)) {
            RandomChange();
        }
    }
}
