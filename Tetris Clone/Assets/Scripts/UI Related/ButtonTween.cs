using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform leftIndicator;
    [SerializeField] private RectTransform rightIndicator;

    [Tooltip("Use this to Set and Open a Panel")]
    [SerializeField] private RectTransform openMenuPanel = null;
    [Tooltip("Use this to Set and Close a Panel (Must be same one above)")]
    [SerializeField] private RectTransform closeMenuPanel = null;

    [SerializeField] private float animationMoveSpeed = 0.5f;
    [SerializeField] private float animationMoveDistance = 50f;

    [SerializeField] private Vector2 leftOriginalPos;
    [SerializeField] private Vector2 rightOriginalPos;

    private Vector2 originalScale;

    private void Start()
    {
        if (openMenuPanel != null)
        {
            originalScale = openMenuPanel.localScale;
            openMenuPanel.localScale = Vector2.zero;
        }

        if (closeMenuPanel != null)
        {
            originalScale = closeMenuPanel.localScale;
            closeMenuPanel.localScale = Vector2.zero;
        }

        leftOriginalPos = leftIndicator.anchoredPosition;
        rightOriginalPos = rightIndicator.anchoredPosition;

        leftIndicator.gameObject.SetActive(false);
        rightIndicator.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverTween();
    }

    private void HoverTween()
    {
        leftIndicator.gameObject.SetActive(true);
        rightIndicator.gameObject.SetActive(true);

        LeanTween.moveX(leftIndicator, leftOriginalPos.x - animationMoveDistance, animationMoveSpeed)
                 .setEase(LeanTweenType.easeInOutSine)
                 .setLoopPingPong();

        LeanTween.moveX(rightIndicator, rightOriginalPos.x + animationMoveDistance, animationMoveSpeed)
                 .setEase(LeanTweenType.easeInOutSine)
                 .setLoopPingPong();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonTweenEnder();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (openMenuPanel != null)
        {
            ShowMenuTween();
        }

        if (closeMenuPanel != null)
        {
            CloseMenuTween();
        }

        ButtonTweenEnder();
    }

    private void ShowMenuTween()
    {
        if (openMenuPanel != null)
        {
            openMenuPanel.gameObject.SetActive(true);   // Activate the panel
            LeanTween.scale(openMenuPanel, originalScale, 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
            {
                LeanTween.cancel(openMenuPanel);
            }); // Pop-in effect
        }
    }

    private void CloseMenuTween()
    {
        if (closeMenuPanel != null)
        {
            LeanTween.scale(closeMenuPanel, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                LeanTween.cancel(closeMenuPanel);
                closeMenuPanel.gameObject.SetActive(false);  // Deactivate after shrinking
                closeMenuPanel.localScale = new Vector2(1, 1);
            });
        }
    }

    private void ButtonTweenEnder()
    {
        LeanTween.cancel(leftIndicator);
        LeanTween.cancel(rightIndicator);

        leftIndicator.anchoredPosition = leftOriginalPos;
        rightIndicator.anchoredPosition = rightOriginalPos;

        leftIndicator.gameObject.SetActive(false);
        rightIndicator.gameObject.SetActive(false);
    }
}
