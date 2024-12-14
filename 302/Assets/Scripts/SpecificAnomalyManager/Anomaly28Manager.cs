using System.Collections;
using UnityEngine;

public class Anomaly28Manager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;

    private float tiltMaxAngle = 15f; // Maximum tilt angle for heavy swaying
    private float tiltDuration = 5f; // Time for each tilt
    private float swayDelay = 0.5f; // Time between each sway
    private float totalDuration = 15f; // Total duration of the anomaly

    private string floatableTag = "floatable"; // Tag to identify floatable objects
    private string floorEmptyTag = "FloorEmpty"; // Tag for the floor objects triggering player pop-up

    private GameObject classroomParent; // Parent object for the entire classroom
    private Rigidbody playerRb; // Rigidbody of the player

    // Clock transform variables
    private Vector3 initialClockLocalPosition;
    private Quaternion initialClockLocalRotation;
    private Vector3 originalClockPosition;
    private Quaternion originalClockRotation;

    void Start()
    {
        // Create a parent GameObject for the classroom
        CreateClassroomParent();

        // Add rigidbodies to all floatable objects
        AddRigidbodiesToFloatables();

        // Start the gradual swaying with delay
        StartCoroutine(DelayedStartSwaying());

        // Stage always clears after experiencing the anomaly
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SetStageClear();

        // Find the player object and its Rigidbody
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
            if (playerRb == null)
            {
                playerRb = player.AddComponent<Rigidbody>(); // Ensure Rigidbody exists
            }

            // Apply a slight upward force to the player
            playerRb.AddForce(Vector3.up * 50f); // Adjust the force as needed
        }

        // Handle clock separately
        GameObject clock = GameObject.Find("clock");
        if (clock != null && classroomParent != null)
        {
            // Save the clock's original global position and rotation
            originalClockPosition = clock.transform.position;
            originalClockRotation = clock.transform.rotation;

            // Calculate and store the initial local position and rotation relative to the classroomParent
            initialClockLocalPosition = clock.transform.position - classroomParent.transform.position;
            initialClockLocalRotation = Quaternion.Inverse(classroomParent.transform.rotation) * clock.transform.rotation;
        }
    }

    private void CreateClassroomParent()
    {
        // Create the parent object
        classroomParent = new GameObject("ClassroomParent");

        // Find all relevant objects in the scene
        GameObject[] objects = {
            GameObject.Find("Classroom"),
            GameObject.Find("Furniture"),
            GameObject.Find("professor_normal"),
            GameObject.Find("Laptop") // 플레이어가 상호작용해야 하는 노트북
        };

        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.transform.parent = classroomParent.transform;
            }
        }
    }

    void Update()
    {
        GameObject clock = GameObject.Find("clock");
        if (clock != null && classroomParent != null)
        {
            // Calculate the new global position and rotation using the stored initial local values
            Vector3 newClockPosition = classroomParent.transform.TransformPoint(initialClockLocalPosition);
            Quaternion newClockRotation = classroomParent.transform.rotation * initialClockLocalRotation;

            // Update clock's transform
            clock.transform.position = newClockPosition;
            clock.transform.rotation = newClockRotation;
        }
    }

    private void AddRigidbodiesToFloatables()
    {
        // Find all objects with the "floatable" tag
        GameObject[] floatableObjects = GameObject.FindGameObjectsWithTag(floatableTag);

        foreach (GameObject obj in floatableObjects)
        {
            if (obj.name == "desk (10)") continue; // Skip desk (10) to prevent affecting the laptop

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = obj.AddComponent<Rigidbody>();
            }

            // Enable gravity and set mass
            rb.useGravity = true;
            rb.mass = 1f; // Adjust mass as needed

            // Apply a slight upward force to prevent them from sinking into the floor
            rb.AddForce(Vector3.up * 2f, ForceMode.Impulse); // Adjust the force as needed
        }

        Debug.Log("Rigidbodies added and upward forces applied to all floatable objects.");
    }

    private IEnumerator DelayedStartSwaying()
    {
        // Wait for 2 seconds before starting the sway
        yield return new WaitForSeconds(2f);
        StartCoroutine(SwayClassroom());
    }

    private IEnumerator SwayClassroom()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            // Scale tilt magnitude gradually based on elapsed time
            float currentMagnitude = Mathf.Lerp(0, tiltMaxAngle, elapsedTime / totalDuration);

            // Dynamically tilt along the X-axis
            float xTilt = Mathf.Sin(Time.time * (2 * Mathf.PI / tiltDuration)) * currentMagnitude;

            // Dynamically tilt along the Z-axis
            float zTilt = Mathf.Cos(Time.time * (2 * Mathf.PI / tiltDuration)) * currentMagnitude;

            // Apply the rotation to the classroom
            classroomParent.transform.rotation = Quaternion.Euler(xTilt, 0, zTilt);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Gradually reset to normal
        StartCoroutine(GradualReset());
    }

    private IEnumerator GradualReset()
    {
        if (classroomParent == null) yield break;

        float elapsedTime = 0f;
        Quaternion initialRotation = classroomParent.transform.rotation;

        while (elapsedTime < tiltDuration)
        {
            // Gradually reduce the angle back to zero
            classroomParent.transform.rotation = Quaternion.Slerp(initialRotation, Quaternion.identity, elapsedTime / tiltDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        classroomParent.transform.rotation = Quaternion.identity;
        Debug.Log("Classroom reset to normal.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player collides with a "FloorEmpty" tagged object
        if (collision.collider.CompareTag(floorEmptyTag) && playerRb != null)
        {
            // Apply a slight upward force to the player
            playerRb.AddForce(Vector3.up * 200f); // Adjust the force as needed
            Debug.Log("Player popped up after colliding with FloorEmpty!");
        }
    }

    void OnDestroy()
    {
        GameObject clock = GameObject.Find("clock");
        if (clock != null)
        {
            // Restore the clock's original global position and rotation
            clock.transform.position = originalClockPosition;
            clock.transform.rotation = originalClockRotation;
        }

        Debug.Log("Anomaly28Manager destroyed. Clock restored to its original phase.");
    }
}
