using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class LeftButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler// required interface when using the OnPointerDown method.
{
    //Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        if(this.gameObject.name == "L")
        {
            Title.move = 500 * Time.deltaTime;
        }
        else
        {
            Title.move = -500 * Time.deltaTime;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Title.move = 0f;
    }
}