using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float swipeThreshold = 0.5f;

    private BlockBase selectedBlock;
    private Vector3 dragStartPosition;
    private bool isDragging;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            TrySelectBlock();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            CheckSwipe();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            selectedBlock = null;
        }
    }

    private void TrySelectBlock()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BlockBase block = hit.collider.GetComponent<BlockBase>();
            if (block != null)
            {
                selectedBlock = block;
                dragStartPosition = Input.mousePosition;
                isDragging = true;
            }
        }
    }

    private void CheckSwipe()
    {
        Vector3 dragDelta = Input.mousePosition - dragStartPosition;

        if (dragDelta.magnitude < swipeThreshold) return;

        Enums.Directions direction;
        if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
        {
            direction = dragDelta.x > 0 ? Enums.Directions.right : Enums.Directions.left;
        }
        else
        {
            direction = dragDelta.y > 0 ? Enums.Directions.up : Enums.Directions.down;
        }

        selectedBlock.TryToMove(direction);
        isDragging = false;
        selectedBlock = null;
    }
}