using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoSingleton<AvatarManager> {
    private GameObject modelSource;//模型的资源
    private Transform modelTrs;
    private GameObject modelEntity;//模型的表现个体,骨架

    //存储模型的结构数据 <身体部位,<部位索引,SkinMeshRenderer>>
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> modleData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    private Transform[] modelHips;

    public override void Start() {
        base.Start();
        InstantiateSource();//加载资源模型
        InstantiateEntity();//加载骨架
        modelHips = modelEntity.GetComponentsInChildren<Transform>();//保存骨架的信息
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
}
