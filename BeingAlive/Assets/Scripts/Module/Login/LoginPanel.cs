using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private bool isFreeTextGrow = false;
    private CanvasGroup GameName1;
    private CanvasGroup GameName2;
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
        yield return null;
    }
}
