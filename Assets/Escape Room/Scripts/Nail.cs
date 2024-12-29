using UnityEngine;

public class Nail : MonoBehaviour
{
    public float maxDepth = 0.1f; // Maximum depth the nail can go
    public float hammerStep = 0.02f; // Depth to move on each hammer collision
    public Transform nailHead; // The part of the nail that moves (nail head)

    private float currentDepth = 0f; // Track how far the nail has been hammered in
    private bool isFullyHammered = false; // Track if the nail is fully hammered in

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision object is tagged as "Hammer"
        if (collision.gameObject.CompareTag("Hammer") && !isFullyHammered)
        {
            print("Nail collided");
            HammerNail();
        }
        print("Nail collided");
        HammerNail();
    }

    private void HammerNail()
    {
        // Move the nail deeper by hammerStep
        currentDepth += hammerStep;

        // Clamp depth to maxDepth
        if (currentDepth >= maxDepth)
        {
            currentDepth = maxDepth;
            isFullyHammered = true;
            OnFullyHammered();
        }

        // Update nail's position
        nailHead.localPosition = new Vector3(
            nailHead.localPosition.x,
            nailHead.localPosition.y - hammerStep,
            nailHead.localPosition.z
        );
    }

    private void OnFullyHammered()
    {
        // Logic for when the nail is fully hammered in
        Debug.Log("Nail is fully hammered in!");

        // Optionally, trigger an event or animation
    }
}