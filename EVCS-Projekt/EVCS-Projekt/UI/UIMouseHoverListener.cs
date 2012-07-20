namespace LastMan.UI
{
    interface UIMouseHoverListener: UIListener
    {
        void OnMouseIn(UIElement element);
        void OnMouseOut(UIElement element);
    }
}
