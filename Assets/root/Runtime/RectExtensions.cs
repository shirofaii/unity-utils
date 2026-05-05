using UnityEngine;

namespace SharedUtils
{
    public static class RectExtensions
    {
        public static Rect Padding(this Rect rect, float padding)
        {
            rect.x += padding;
            rect.y += padding;
            float num = padding * 2f;
            rect.width -= num;
            rect.height -= num;
            return rect;
        }
    }
}