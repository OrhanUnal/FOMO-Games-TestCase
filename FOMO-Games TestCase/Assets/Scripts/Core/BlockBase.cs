using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    private int colorId;
    private List<int> directions;
    private int row;
    private int col;

    public void Initialize(int ColorID, List<int> Directions, int Row, int Col)
    {
        colorId = ColorID;
        directions = Directions;
        row = Row;
        col = Col;
    }

    public void TryToMove(Enums.Directions direction)
    {
        Vector3 dir;
        switch (direction)
        {
            case Enums.Directions.down:
                dir = Vector3.down;
                break;    
            case Enums.Directions.up:
                dir = Vector3.up; 
                break;    
            case Enums.Directions.left:
                dir = Vector3.left; 
                break;    
            case Enums.Directions.right:
                dir = Vector3.right;
                break;
            default:
                dir = Vector3.up;
                break;
        }

        RaycastHit smallestHit = default;
        float minDistance = float.MaxValue;
        foreach(Vector3 pos in GetRayOrigins())
        {
            RaycastHit hit;
            Physics.Raycast(pos, dir, out hit);
            if (hit.distance < minDistance)
            {
                smallestHit = hit;
                minDistance = hit.distance;
            }
        }
        if (minDistance >= GameManager.instance.BlockSize)
        {
            StartCoroutine(Move(dir, minDistance));
            if (smallestHit.collider.GetComponent<ExitGates>())
                Destroy(gameObject);
        }
    }

    public IEnumerator Move(Vector3 directionAsVector, float amount)
    {
        GameManager.instance.DecrementMoveLimit();
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + directionAsVector * amount;
        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }

        transform.position = targetPosition;
    }

    virtual public List<Vector3> GetRayOrigins()
    {
        List<Vector3> listOfRayOrigins = new List<Vector3>();
        listOfRayOrigins.Add(transform.position);
        return listOfRayOrigins;
    }
}