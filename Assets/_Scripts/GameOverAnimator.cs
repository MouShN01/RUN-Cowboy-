using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class GameOverAnimator : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private TMP_Text gameOver;
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private RectTransform replayButton;
    [SerializeField] private RectTransform homeButton;

    public void AnimateUI()
    {
        // Скрываем всё
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
        gameOver.alpha = 0;
        scoreLabel.alpha = 0;

        var replayButtonImage = replayButton.GetComponent<Image>();
        var homeButtonImage = homeButton.GetComponent<Image>();

        // Скрываем кнопки
        replayButtonImage.color = new Color(1, 1, 1, 0);
        homeButtonImage.color = new Color(1, 1, 1, 0);

        Debug.Log("animate panel");
        panel.DOFade(0.972f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("animate title");
            gameOver.gameObject.SetActive(true);
            gameOver.DOFade(1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Debug.Log("animate score");
                scoreLabel.gameObject.SetActive(true);
                scoreLabel.DOFade(1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    Debug.Log("animate buttons");
                    replayButtonImage.gameObject.SetActive(true);
                    homeButtonImage.gameObject.SetActive(true);

                    replayButtonImage.DOFade(1f, 0.8f);
                    homeButtonImage.DOFade(1f, 0.8f);
                });
            });
        });
    }
}
