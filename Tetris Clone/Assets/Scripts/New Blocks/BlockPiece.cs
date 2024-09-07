using UnityEngine;

public class BlockPiece : MonoBehaviour
{
    [SerializeField] private TetrisGrid tetrisGrid;

    [SerializeField] private Vector3 rotationPoint;
    [SerializeField] private float rotationAngle = 90f;

    [SerializeField] private float moveAmount = 0.64f;
    [SerializeField] private float moveDelay = 0.2f;

    private float nextMoveTime = 0f;

    [SerializeField] private float fallDelay = 0.4f;

    private float stepFall = 0f;

    [Header("Check Limits")]
    [SerializeField] private Transform[] blocks;

    void Update()
    {
        if (transform.position.y > tetrisGrid.bottomLimit)
        {
            BlockPieceFall();
            BlockPieceMovement();

            if (Input.GetKeyDown(KeyCode.W))
            {
                PieceRotation();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                HardDrop();
            }
        }
    }

    void BlockPieceFall()
    {
        stepFall += Time.deltaTime;

        if (stepFall >= fallDelay)
        {
            Vector2 newPosition = transform.position;
            newPosition.y -= moveAmount;

            // Round x coordinate to 2 decimal places
            newPosition.y = Mathf.Round(newPosition.y * 100f) / 100f;

            transform.position = newPosition;
            stepFall = 0f;
        }
    }    

    void CheckLimitsForMovement()
    {
        foreach (Transform block in blocks)
        {

        }
    }

    void BlockPieceMovement()
    {
        nextMoveTime += Time.deltaTime;

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && nextMoveTime >= moveDelay)
        {
            if (transform.position.x > tetrisGrid.leftLimit)
            {
                Vector2 newPosition = transform.position;
                newPosition.x -= moveAmount;

                // Round x coordinate to 2 decimal places
                newPosition.x = Mathf.Round(newPosition.x * 100f) / 100f;

                transform.position = newPosition;
                nextMoveTime = 0f;
            }
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && nextMoveTime >= moveDelay)
        {
            if (transform.position.x < tetrisGrid.rightLimit)
            {
                Vector2 newPosition = transform.position;
                newPosition.x += moveAmount;

                // Round x coordinate to 2 decimal places
                newPosition.x = Mathf.Round(newPosition.x * 100f) / 100f;

                transform.position = newPosition;
                nextMoveTime = 0f;
            }
        }
    }    

    void HardDrop()
    {
        while (transform.position.y > tetrisGrid.bottomLimit)
        {
            Vector2 newPosition = transform.position;
            newPosition.y -= moveAmount;

            // Round x coordinate to 2 decimal places
            newPosition.y = Mathf.Round(newPosition.y * 100f) / 100f;

            transform.position = newPosition;
        }
    }

    void PieceRotation()
    {
        // Rotate around the specified point
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), rotationAngle);

        // Round the position to 2 decimal places
        Vector3 roundedPosition = transform.position;
        roundedPosition.x = Mathf.Round(roundedPosition.x * 100f) / 100f;
        roundedPosition.y = Mathf.Round(roundedPosition.y * 100f) / 100f;
        roundedPosition.z = Mathf.Round(roundedPosition.z * 100f) / 100f;

        transform.position = roundedPosition;
    }    
}
