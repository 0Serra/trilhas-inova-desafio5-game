using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoDoPlayer : MonoBehaviour
{
    [SerializeField] private float moveTime = 0.2f; private bool isMoving = false;
    private Vector2 input;
    private Vector3 startPos;
    private Vector3 endPos;
    private float t;

    [SerializeField] private Vector2 gridSize;
    [SerializeField] private Vector2 gridMin;
    [SerializeField] private Vector2 gridMax;

    private void Start()
    {
        gridSize /= 100f;
        gridMin /= 100f;
        gridMax /= 100f;
    }

    private void Update()
    {
        if (!isMoving)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                Vector3 targetPos = transform.position + new Vector3(input.x * gridSize.x, input.y * gridSize.y, 0f);

                if (IsInsideGrid(targetPos))
                {
                    StartCoroutine(MoveToPosition(targetPos));
                }
            }
        }
    }

    bool IsInsideGrid(Vector3 targetPos)
    {
        return targetPos.x >= gridMin.x && targetPos.x <= gridMax.x &&
               targetPos.y >= gridMin.y && targetPos.y <= gridMax.y;
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;

        startPos = transform.position;
        endPos = target;
        t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }
}
