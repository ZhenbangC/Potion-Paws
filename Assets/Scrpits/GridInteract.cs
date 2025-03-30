using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridPosition))]
public class GridInteract : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    investeroyController InvesteroyController;
    GridPosition gridPosition;

    // Start is called before the first frame update
    private void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
        InvesteroyController = FindObjectOfType(typeof(investeroyController)) as investeroyController;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        InvesteroyController.GridPosition = gridPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        InvesteroyController.GridPosition = null;
    }


}
