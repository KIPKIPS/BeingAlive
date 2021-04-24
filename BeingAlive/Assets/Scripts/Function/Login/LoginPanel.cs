using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : BasePanel {
    public override void Awake() {
        base.Awake();
        panelType = UIManager.PanelType.Module;
        showTween = false;
    }

    private Transform mainTrs;
    private bool isPressKey = false;
    private Text freeText;
    private Transform freeTextTrs;
    private CanvasGroup GameName1;
    private CanvasGroup GameName2;
    private float oriX = -40;
    private Button[] btnList = new Button[3];
    private GameObject[] arrowList = new GameObject[3];
    public override void InitPanel() {
        base.InitPanel();
        mainTrs = FindObj<Transform>("Main");
        freeText = FindObj<Text>("FreeText", mainTrs);
        freeTextTrs = freeText.transform;
        freeTextTrs.localScale = Vector3.one;
        GameName1 = FindObj<CanvasGroup>("GameName1", mainTrs);
        GameName1.alpha = 1;
        GameName2 = FindObj<CanvasGroup>("GameName2", mainTrs);
        GameName2.alpha = 0;
        for (int i = 0; i < 3; i++) {
            btnList[i] = FindObj<Button>("Button" + i, mainTrs, MouseEnter, MouseExit);
            arrowList[i] = FindObj<Transform>("Arrow", btnList[i].transform).gameObject;
        }

        // for (int i = 0; i < btnList.Length; i++) {
        //     print(btnList[i].transform.name);
        // }
    }

    private float offsetX = 60f;
    private bool showArrow;
    private void MouseEnter(BaseEventData baseData) {
        PointerEventData data = baseData as PointerEventData;
        showArrow = true;
        //print(data.pointerEnter.name);
        int index = int.Parse(data.pointerEnter.name.Replace("Button", ""));
        //print(index);
        float oriX = data.pointerEnter.transform.localPosition.x;
        data.pointerEnter.transform.DOLocalMoveX(oriX + offsetX, 0.3f).SetEase(Ease.OutBack).OnComplete(() => {
            for (int i = 0; i < arrowList.Length; i++) {
                arrowList[i].SetActive(i == index && showArrow);
            }
        });
    }

    private void MouseExit(BaseEventData baseData) {
        PointerEventData data = baseData as PointerEventData;
        //print(data.pointerEnter.name);
        showArrow = false;
        data.pointerEnter.transform.DOLocalMoveX(oriX, 0.3f).OnComplete(() => {
            for (int i = 0; i < arrowList.Length; i++) {
                arrowList[i].SetActive(false);
            }
        });
    }

    public override void OnEnter() {
        base.OnEnter();

    }

    private float freeTextMaxScale = 1.05f;
    private float freeTextMinScale = 0.95f;
    private bool needGrow = true;
    private float smoothSpeed = 0.08f;
    private float scale = 1;
    private float fadeTime = 0.5f;
    public override void Update() {
        base.Update();
        if (Input.anyKey && !isPressKey) {
            isPressKey = true;
            freeTextTrs.gameObject.SetActive(false);
            StartCoroutine("ShowOptions");
        }

        if (!isPressKey) {
            if (needGrow && freeTextTrs.localScale.x < freeTextMaxScale) {
                scale += smoothSpeed * Time.deltaTime;
                freeTextTrs.localScale = Vector3.one * scale;
                if (freeTextTrs.localScale.x > freeTextMaxScale) {
                    needGrow = false;
                }
            }
            if (!needGrow && freeTextTrs.localScale.x > freeTextMinScale) {
                scale -= smoothSpeed * Time.deltaTime;
                freeTextTrs.localScale = Vector3.one * scale;
                if (freeTextTrs.localScale.x < freeTextMinScale) {
                    needGrow = true;
                }
            }
        }
    }

    IEnumerator ShowOptions() {
        print("any key");
        GameName1.DOFade(0, fadeTime).SetEase(Ease.Linear).OnComplete(() => GameName1.alpha = 0);
        GameName2.DOFade(1, fadeTime).SetEase(Ease.Linear).OnComplete(() => GameName2.alpha = 1);
        // Sequence seq = DOTween.Sequence();//创建队列
        // for (int i = 0; i < btnList.Length; i++) {
        //     seq.Append(null).AppendInterval(0.1f).AppendCallback(() => {
        //
        //     });
        // }

        float timer = 0;
        int index = 0;
        DOTween.To(() => timer, x => timer = x, 1, 0.1f).OnStepComplete(() => {
            btnList[index].transform.DOLocalMoveX(oriX, 0.3f).SetEase(Ease.OutBack);
            index++;
        }).SetLoops(btnList.Length);
        yield return null;
    }
}
