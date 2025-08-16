using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[ExecuteAlways]
public class CardStack : MonoBehaviour
{
    public enum Orientation { Horizontal, Vertical }
    public enum Identifier { None, Deck, Hand, Cost, Board }

    [SerializeField] RectTransform root;
    [SerializeField] Orientation orientation = Orientation.Horizontal;
    public Identifier identifier = Identifier.None;

    [Header("Spacing")]
    [SerializeField, HideIf(nameof(fitToRect))] float spacing = 40f;
    [SerializeField] bool fitToRect = false;
    [SerializeField] float minSpacing = 10f;
    [SerializeField] float maxSpacing = 200f;

    [Header("Hover")]
    [SerializeField] bool enableHover = true;
    [SerializeField] float hoverLift = 30f;
    [SerializeField] float hoverSeparation = 30f;
    [SerializeField] int hoverSpread = 1;
    [SerializeField] float hoverSmooth = 12f;

    [Header("Padding")]
    [SerializeField] float paddingLeft = 0f;
    [SerializeField] float paddingRight = 0f;
    [SerializeField] float paddingTop = 0f;
    [SerializeField] float paddingBottom = 0f;

    [Header("Layout")]
    [SerializeField] bool centerAlign = true;
    [SerializeField] Vector2 startOffset = Vector2.zero;

    public int Count => cards.Count;
    public int HoveredIndex => hoveredIndex;
    public Orientation Axis => orientation;
    public bool FitToRect => fitToRect;
    public float CurrentSpacing => spacing;
    public bool HoverEnabled => enableHover;

    readonly List<RectTransform> cards = new();
    int hoveredIndex = -1;
    bool hasRoot => root != null;

    void Reset()
    {
        root = GetComponent<RectTransform>();
        RebuildCardList();
    }

    void OnEnable()
    {
        if (!hasRoot) root = GetComponent<RectTransform>();
        RebuildCardList();
        RegisterCards();
    }

    void OnTransformChildrenChanged()
    {
        RebuildCardList();
        RegisterCards();
        SetHoveredIndex(-1);
    }

    public void SetHovered(RectTransform card, bool hovered)
    {
        if (!enableHover) return;
        var idx = cards.IndexOf(card);
        if (idx < 0) return;
        hoveredIndex = hovered ? idx : (hoveredIndex == idx ? -1 : hoveredIndex);
    }

    void RegisterCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var c = cards[i];
            var item = c.GetComponent<Card>();
            if (item == null) item = c.gameObject.AddComponent<Card>();
            item.SetStack(this);
        }
    }

    void RebuildCardList()
    {
        cards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var rt = transform.GetChild(i) as RectTransform;
            if (rt != null && rt.gameObject.activeInHierarchy)
                cards.Add(rt);
        }
    }

    void Update()
    {
        if (!hasRoot) return;
        Layout(Time.deltaTime);
    }

    void OnValidate()
    {
        if (!hasRoot) root = GetComponent<RectTransform>();
        if (hoverSpread < 0) hoverSpread = 0;
        if (minSpacing > maxSpacing) minSpacing = maxSpacing;
    }

    void Layout(float dt)
    {
        int count = cards.Count;
        if (count == 0) return;

        float step = spacing;
        var size = cards[0].rect.size;
        float cardLen = orientation == Orientation.Horizontal ? size.x : size.y;

        if (fitToRect)
        {
            float availableLen;
            if (orientation == Orientation.Horizontal)
                availableLen = root.rect.width - paddingLeft - paddingRight;
            else
                availableLen = root.rect.height - paddingTop - paddingBottom;

            step = count > 1 ? (availableLen - cardLen) / Mathf.Max(1, count - 1) : 0f;
            step = Mathf.Clamp(step, minSpacing, maxSpacing);
        }

        float totalSpan = (count > 1 ? step * (count - 1) : 0f);

        float startPrimary;
        if (orientation == Orientation.Horizontal)
        {
            if (centerAlign)
                startPrimary = -totalSpan * 0.5f + paddingLeft - paddingRight * 0.5f;
            else
                startPrimary = paddingLeft;
        }
        else
        {
            if (centerAlign)
                startPrimary = -totalSpan * 0.5f + paddingBottom - paddingTop * 0.5f;
            else
                startPrimary = paddingBottom;
        }

        for (int i = 0; i < count; i++)
        {
            float primary = startPrimary + i * step;
            float perpendicular = 0f;

            if (enableHover)
            {
                if (i == hoveredIndex && hoveredIndex >= 0)
                    perpendicular += hoverLift;

                if (hoveredIndex >= 0 && hoverSeparation > 0f && hoverSpread > 0)
                {
                    int dist = Mathf.Abs(i - hoveredIndex);
                    if (dist > 0 && dist <= hoverSpread)
                    {
                        float t = 1f - (dist - 1) / Mathf.Max(1f, (float)hoverSpread);
                        float sep = hoverSeparation * t;
                        float side = Mathf.Sign(i - hoveredIndex);
                        primary += sep * side;
                    }
                    else if (i > hoveredIndex && dist > hoverSpread)
                        primary += hoverSeparation;
                    else if (i < hoveredIndex && dist > hoverSpread)
                        primary -= hoverSeparation;
                }
            }

            Vector3 targetLocalPos;
            if (orientation == Orientation.Horizontal)
                targetLocalPos = new Vector3(primary + startOffset.x, perpendicular + startOffset.y, 0f);
            else
                targetLocalPos = new Vector3(perpendicular + startOffset.x, primary + startOffset.y, 0f);

            var rt = cards[i];
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetLocalPos, 1f - Mathf.Exp(-hoverSmooth * dt));
        }
    }

    public void RefreshNow()
    {
        RebuildCardList();
        RegisterCards();
        Layout(Application.isPlaying ? Time.deltaTime : 0f);
    }

    public void SetHoveredIndex(int index)
    {
        if (!enableHover) return;                          // <--- block if disabled
        hoveredIndex = (index >= 0 && index < cards.Count) ? index : -1;
    }

}
