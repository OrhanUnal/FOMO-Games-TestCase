using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBase : MonoBehaviour
{
    [SerializeField] 
    private GameObject destroyParticlePrefab;

    private Enums.BlockColor colorId;
    private List<int> directions;
    private int row;
    private int col;
    private bool isMoving;

    protected float blockSize;

    public static event Action OnBlockMoved;
    public static event Action<int> OnBlockCountChanged;

    public void Initialize(int ColorID, List<int> Directions, int Row, int Col, float BlockSize)
    {
        colorId = (Enums.BlockColor)ColorID;
        directions = Directions;
        row = Row;
        col = Col;
        blockSize = BlockSize;
        isMoving = false;
    }

    private void Start()
    {
        OnBlockCountChanged?.Invoke(1);
    }

    public void TryToMove(Enums.Directions direction)
    {
        if (!directions.Contains((int)direction)) return;
        if (isMoving) return;        

        Vector3 directionAsVector;
        switch (direction)
        {
            case Enums.Directions.down:
                directionAsVector = Vector3.down;
                break;    
            case Enums.Directions.up:
                directionAsVector = Vector3.up; 
                break;    
            case Enums.Directions.left:
                directionAsVector = Vector3.left; 
                break;    
            case Enums.Directions.right:
                directionAsVector = Vector3.right;
                break;
            default:
                directionAsVector = Vector3.up;
                break;
        }

        RaycastHit smallestHit = default;
        float minDistance = float.MaxValue;
        foreach(Vector3 pos in GetRayOrigins())
        {
            RaycastHit hit;
            Physics.Raycast(pos, directionAsVector, out hit);
            if (hit.distance < minDistance)
            {
                smallestHit = hit;
                minDistance = hit.distance;
            }
        }
        if (minDistance >= blockSize && smallestHit.collider != null)
        {
            float distanceToTravel = minDistance - blockSize / 2;
            bool canDestroy = false;

            ExitGates closestExitGate = smallestHit.collider.GetComponent<ExitGates>();
            if (closestExitGate && closestExitGate.IsMatchingColors(colorId))
                canDestroy = true;

            StartCoroutine(Move(directionAsVector, distanceToTravel, canDestroy));
        }
        else StartCoroutine(BounceEffect(directionAsVector));
    }

    private IEnumerator Move(Vector3 dir, float amount, bool hitsExit)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + dir * amount;
        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }

        transform.position = targetPosition;        
        isMoving = false;
        
        if (hitsExit)
        {
            if (destroyParticlePrefab != null)
            {
                GameObject particles = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
                ParticleSystem ps = particles.GetComponent<ParticleSystem>();
                var main = ps.main;

                Color blockColor = GetBlockColor();
                main.startColor = blockColor;
            }
            OnBlockCountChanged?.Invoke(-1);
            Destroy(gameObject);
        }
        else yield return StartCoroutine(BounceEffect(dir));
        OnBlockMoved?.Invoke();
    }

    private IEnumerator BounceEffect(Vector3 moveDirection)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 squishScale = originalScale;

        if (Mathf.Abs(moveDirection.x) > 0)
        {
            squishScale.x *= 0.8f;
            squishScale.y *= 1.15f;
        }
        else
        {
            squishScale.y *= 0.8f;
            squishScale.x *= 1.15f;
        }

        float elapsed = 0f;
        float squishDuration = 0.08f;

        while (elapsed < squishDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, squishScale, elapsed / squishDuration);
            yield return null;
        }

        elapsed = 0f;
        float bounceDuration = 0.12f;
        while (elapsed < bounceDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(squishScale, originalScale, elapsed / bounceDuration);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    private Color GetBlockColor()
    {
        switch (colorId)
        {
            case Enums.BlockColor.Blue: return Color.blue;
            case Enums.BlockColor.Red: return Color.red;
            case Enums.BlockColor.Yellow: return Color.yellow;
            case Enums.BlockColor.Green: return Color.green;
            default: return Color.white;
        }
    }

    virtual protected List<Vector3> GetRayOrigins()
    {
        List<Vector3> listOfRayOrigins = new List<Vector3>();
        listOfRayOrigins.Add(transform.position);
        return listOfRayOrigins;
    }
}