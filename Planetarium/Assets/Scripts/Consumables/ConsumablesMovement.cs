using UnityEngine;

public class ConsumablesMovement : MonoBehaviour
{
    #region Movement Settings
    [Header("Movement Settings")]

    public Vector3 movementVector = new Vector3(0, 1, 0);

    [SerializeField, Range(0.1f, 1f)]
    public float movementSpeed = 0.15f;

    [SerializeField, Range(1, 5)]
    public int directionChangeTime = 1;
    
    public bool flipDirection;
    #endregion

    #region Rotation Settings
    [Header("Rotation Settings")]

    public Vector3 rotationVector = new Vector3(0,1,0);
    
    [SerializeField, Range(10, 50)]
    public int rotationSpeed = 40;
    #endregion
    
    private int _index = 0; // represents seconds

    private void Awake()
    {
        InvokeRepeating("UpdateIndex", 0, 1);   
    }

    private void Update()
    {
        HandleMovementAndRotation();
    }

    private void HandleMovementAndRotation()
    {
        // Rotate along rotationVector.
        transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);

        // Move along movementVector.
        if (flipDirection)
        {
            transform.position += movementVector * movementSpeed * Time.deltaTime;
        }
        else
        {
            transform.position -= movementVector * movementSpeed * Time.deltaTime;
        }

        // Change movement direction.
        if (_index > directionChangeTime)
        {
            flipDirection = !flipDirection;
            _index = 0;
        }
    }
    
    /* For counting purposes. Invoked once per second. */
    private void UpdateIndex()
    {
        _index++;
    }
}
