using UnityEngine;
using TMPro;
using DG.Tweening;

public class MainMenuAnimator : MonoBehaviour
{
    [SerializeField] private TMP_Text gameTitle;
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform settingsButton;
    [SerializeField] private RectTransform storeButton;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text coins;

    void Awake()
    {
        DOTween.defaultEaseType = Ease.OutQuad;
        DOTween.useSmoothDeltaTime = true;
    }
    
    void Start()
    {
        AnimateUI();
    }

    void AnimateUI()
    {
        gameTitle.alpha = 0;
        coins.alpha = 0;
        score.alpha = 0;

        Vector3 playStart = playButton.anchoredPosition - new Vector2(0, 1400);
        Vector3 settingsStart = settingsButton.anchoredPosition - new Vector2(0, 1400);
        Vector3 storeStart = storeButton.anchoredPosition - new Vector2(0, 1400);

        playButton.anchoredPosition = playStart;
        settingsButton.anchoredPosition = settingsStart;
        storeButton.anchoredPosition = storeStart;

        gameTitle.DOFade(1, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            playButton.DOAnchorPosY(playButton.anchoredPosition.y + 1400, 0.8f)
                    .SetEase(Ease.OutBack).SetDelay(0.1f);

            settingsButton.DOAnchorPosY(settingsButton.anchoredPosition.y + 1400, 0.8f)
                        .SetEase(Ease.OutBack).SetDelay(0.3f);

            storeButton.DOAnchorPosY(storeButton.anchoredPosition.y + 1400, 0.8f)
                    .SetEase(Ease.OutBack).SetDelay(0.5f).OnComplete(()=>
                    {
                        score.DOFade(1, 1f).SetEase(Ease.OutQuad);
                        coins.DOFade(1, 1f).SetEase(Ease.OutQuad);
                    });
        });
    }
}