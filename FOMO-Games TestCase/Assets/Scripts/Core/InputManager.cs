using UnityEngine;

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

        // Determine direction based on which axis has more movement
        Enums.Directions direction;
        if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
        {
            direction = dragDelta.x > 0 ? Enums.Directions.right : Enums.Directions.left;
        }
        else
        {
            direction = dragDelta.y > 0 ? Enums.Directions.up : Enums.Directions.down;
        }

        // Try to move the block
        selectedBlock.TryToMove(direction);

        // Reset so we don't trigger multiple moves per swipe
        isDragging = false;
        selectedBlock = null;
    }
}