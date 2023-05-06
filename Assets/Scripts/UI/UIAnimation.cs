using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum AnimationType
{
    AnchoredPosition,
    Position,
    LocalPositon,
    LocalScale,
    LocalRotation,
    Alpha,
}
public enum AnimationMoveType
{ 
    Clamp,
    PingPong,
    Loop,
}

public class UIAnimation : MonoBehaviour
{
    public AnimationCurve animationCurveShow;
    public AnimationCurve animationCurveHide;
    public AnimationType animationType = AnimationType.Position;
    public AnimationMoveType animationMoveType = AnimationMoveType.Clamp;
    public Vector3 initialV3 = Vector3.zero;
    public Vector3 targetV3 = Vector3.zero;
    Vector3 localInitialV3 = Vector3.zero;
    public float initialAlpha = 0;
    public float targetAlpha = 0;
    public bool enable = true;
    public bool delay = false;
    public float delayTime;
    private void Awake()
    {
        localInitialV3 = transform.localPosition;
    }
    void Start()
    {

    }
    private void OnEnable()
    {
        if (!enable) { return; }
        if(animationMoveType == AnimationMoveType.Clamp)
        {
            animationCurveShow.postWrapMode = WrapMode.Clamp;
        }
        if (animationMoveType == AnimationMoveType.PingPong)
        {
            animationCurveShow.postWrapMode = WrapMode.PingPong;
        }
        if (animationMoveType == AnimationMoveType.Loop)
        {
            animationCurveShow.postWrapMode = WrapMode.Loop;
        }

        ShowUIAnimaition();
    }
    /// <summary>
    /// 播放UIAnimation
    /// </summary>
    /// <param name="waitTime">延迟时间</param>
    public void ShowAnimation(float waitTime = 0)
    {
        if (gameObject.activeSelf == false) { return; }
        Invoke("ShowUIAnimaition",waitTime);
    }
    /// <summary>
    /// 倒放UIAnimation
    /// </summary>
    public void InvertedUIAnimation()
    {
        StartCoroutine("InvertedUIAnimationFromCurve");
    }
    void ShowUIAnimaition()
    {
        StartCoroutine("ShowUIAnimationFromCurve");
    }
    public void HideUI()
    {
        StartCoroutine("HideUIAnimationFromCurve");
    }
    public void SetAnimationCurve(AnimationCurve animationCurve,bool play = false)
    {
        this.animationCurveShow = animationCurve;
        if (play) { ShowAnimation(); }
    }
    public void SetAnimationCurve(AnimationCurve animationCurve,AnimationType animationType, Vector3 initialV3,Vector3 targetV3,bool play = false ,float delayTime = 0)
    {
        this.animationCurveShow = animationCurve;
        this.animationType = animationType;
        this.initialV3 = initialV3;
        this.targetV3 = targetV3;
        if (play) { Invoke("ShowUIAnimaition", delayTime); }
    }
    //动画协程
    IEnumerator ShowUIAnimationFromCurve()
    {
        if (animationCurveShow.length <= 1) { yield break; }

        float aimTime = animationCurveShow[animationCurveShow.length - 1].time;

        aimTime = animationMoveType != AnimationMoveType.Clamp ? 999 : aimTime;

        float time = 0,progress;

        Vector3 v3;

        while (time <aimTime)
        {
            time += Time.deltaTime;

            progress = animationCurveShow.Evaluate(time);

            v3 = Vector3.Lerp(initialV3, targetV3, progress);

            if(progress > 1)
            {
                v3 += (targetV3 - initialV3) * (progress - 1);
            }
            else if(progress < 0)
            {
                v3 = -Vector3.Lerp(initialV3, targetV3, -progress);
            }
            if(animationType == AnimationType.AnchoredPosition)
            {
                RectTransform rt = GetComponent<RectTransform>();
                rt.anchoredPosition = v3;
            }
            else if (animationType == AnimationType.Position)
            {
                transform.position = v3;
            }
            if (animationType == AnimationType.LocalPositon)
            {
                transform.localPosition = v3;
            }
            else if(animationType == AnimationType.LocalScale)
            {
                transform.localScale = v3;
            }
            else if(animationType == AnimationType.LocalRotation)
            {
                transform.eulerAngles = v3;
            }
            else if (animationType == AnimationType.Alpha)
            {
                Image image = transform.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(initialAlpha, targetAlpha, progress)) ;
            }
            yield return null;
        }

        yield return null;
    }
    //退场动画协程
    IEnumerator HideUIAnimationFromCurve()
    {

        yield return null;
        gameObject.SetActive(false); 
    }
    //倒放动画协程
    IEnumerator InvertedUIAnimationFromCurve()
    {
        if (animationCurveShow.length <= 1) { Debug.Log("时间过短"); yield break;  }

        if (animationMoveType != AnimationMoveType.Clamp) { Debug.Log("AnimationType类型不对"); yield break; }

        float aimTime = animationCurveShow[animationCurveShow.length - 1].time;
        
        float time = aimTime, progress;

        Vector3 v3;

        while (time > 0)
        {
            time -= Time.deltaTime;

            progress = animationCurveShow.Evaluate(time);

            v3 = Vector3.Lerp(initialV3, targetV3, progress);

            if (progress > 1)
            {
                v3 += (targetV3 - initialV3) * (progress - 1);
            }
            else if (progress < 0)
            {
                v3 = -Vector3.Lerp(initialV3, targetV3, -progress);
            }

            if (animationType == AnimationType.Position)
            {
                transform.position = initialV3 + v3;
            }
            if (animationType == AnimationType.LocalPositon)
            {
                transform.localPosition = v3;
            }
            else if (animationType == AnimationType.LocalScale)
            {
                transform.localScale = v3;
            }
            else if (animationType == AnimationType.LocalRotation)
            {
                transform.eulerAngles = v3;
            }
            else if (animationType == AnimationType.Alpha)
            {
                Image image = transform.GetComponent<Image>();

                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(initialAlpha, targetAlpha, progress));
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }

}

