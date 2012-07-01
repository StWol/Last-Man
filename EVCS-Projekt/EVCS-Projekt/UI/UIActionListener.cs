using LastMan.UI;

namespace LastMan.UI
{
    interface UIActionListener: UIListener
    {
        void OnMouseDown(UIElement element);
        void OnMouseUp(UIElement element);
    }
}
