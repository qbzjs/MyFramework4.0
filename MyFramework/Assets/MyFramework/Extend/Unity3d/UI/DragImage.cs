using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyFramework
{
    /// <summary>
    /// 拖拽图片
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class DragImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            var rt = gameObject.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos;
            }
        }
    }
}
