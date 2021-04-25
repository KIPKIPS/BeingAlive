using DG.Tweening;
using System;
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
    private float[] oriY = new float[3];
    private RectTransform[] btnRect = new RectTransform[3];
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
            btnRect[i] = btnList[i].transform.GetComponent<RectTransform>();
            oriY[i] = btnRect[i].anchoredPosition.y;
            int index = i;
            FindObj<Button>("Button" + i, mainTrs, () => {
                BtnClick(index);
            });
        }

        // for (int i = 0; i < btnList.Length; i++) {
        //     print(btnList[i].transform.name);
        // }
    }

    private void BtnClick(int index) {
        //print(index);
        switch (index) {
            case 0:
                base.OnPause();
                break;
            case 1:

                break;
            case 2:
                if (optionAnimFinish) {
                    TitleFade(GameName2, GameName1);
                    isPressKey = false;
                    backAnimFinish = false;
                    optionAnimFinish = false;
                    for (int i = 0; i < 3; i++) {
                        //btnRect[i].anchoredPosition = new Vector2(-250, oriY[i]);
                        arrowList[i].SetActive(false);
                    }
                    float timer = 0;
                    int count = 0;
                    DOTween.To(() => timer, x => timer = x, 1, 0.1f).OnStepComplete(() => {
                        btnList[count].transform.DOLocalMoveX(-250, 0.2f).SetEase(Ease.OutBack);
                        count++;
                    }).SetLoops(btnList.Length).OnComplete(() => {
                        freeTextTrs.gameObject.SetActive(true);
                        backAnimFinish = true;
                    });
                }
                break;
        }
    }

    private float offsetX = 60f;
    private bool showArrow;
    private void MouseEnter(BaseEventData baseData) {
        if (isPressKey && optionAnimFinish) {
            PointerEventData data = baseData as PointerEventData;
            showArrow = true;
            //print(data.pointerEnter.name);
            int index = int.Parse(data.pointerEnter.name.Replace("Button", ""));
            //print(index);
            data.pointerEnter.transform.DOLocalMoveX(oriX + offsetX, 0.3f).SetEase(Ease.OutBack);
            for (int i = 0; i < arrowList.Length; i++) {
                arrowList[i].SetActive(i == index && showArrow);
            }
        }
    }

    private void MouseExit(BaseEventData baseData) {
        if (isPressKey && optionAnimFinish) {
            PointerEventData data = baseData as PointerEventData;
            //print(data.pointerEnter.name);
            showArrow = false;
            data.pointerEnter.transform.DOLocalMoveX(oriX, 0.3f);
            for (int i = 0; i < arrowList.Length; i++) {
                arrowList[i].SetActive(false);
            }
        }
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
        if (Input.anyKey && !isPressKey && backAnimFinish) {
            isPressKey = true;
            backAnimFinish = false;
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

    private bool optionAnimFinish = false;
    private bool backAnimFinish = true;
    IEnumerator ShowOptions() {
        //print("any key");
        TitleFade(GameName1, GameName2);
        // Sequence seq = DOTween.Sequence();//创建队列
        // for (int i = 0; i < btnList.Length; i++) {
        //     seq.Append(btnList[i].transform.DOLocalMoveX(oriX, 0.3f).SetEase(Ease.OutBack)).AppendInterval(0.1f);
        // }

        float timer = 0;
        int index = 0;
        DOTween.To(() => timer, x => timer = x, 1, 0.1f).OnStepComplete(() => {
            btnList[index].transform.DOLocalMoveX(oriX, 0.3f).SetEase(Ease.OutBack);
            index++;
        }).SetLoops(btnList.Length).OnComplete(() => {
            optionAnimFinish = true;
        });
        yield return null;
    }

    void TitleFade(CanvasGroup fadeOut, CanvasGroup fadeIn) {
        fadeOut.DOFade(0, fadeTime).SetEase(Ease.Linear).OnComplete(() => fadeOut.alpha = 0);
        fadeIn.DOFade(1, fadeTime).SetEase(Ease.Linear).OnComplete(() => fadeIn.alpha = 1);
    }
}
