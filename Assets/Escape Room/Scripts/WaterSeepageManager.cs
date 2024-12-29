using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaterSeepageManager : MonoBehaviour
{
    public ParticleSystem waterParticles; // Reference to the particle system
    public XRSocketInteractor socket; // Reference to the XRSocketInteractor
    public int totalNails = 4; // Total number of nails
    private int nailsHammered = 0; // Counter for nailed nails
    private ParticleSystem.EmissionModule emission;
    public float maxEmission = 50f; // Max emission rate
    public float plankCoverEmission = 30f; // Emission rate when plank is covering the hole

    private bool plankPlaced = false;

    private void Start()
    {
        emission = waterParticles.emission;
        emission.rateOverTime = maxEmission; // Set the emission to max initially
        socket.selectEntered.AddListener(OnPlankPlaced);
        socket.selectExited.AddListener(OnPlankRemoved);
    }

    private void OnDestroy()
    {
        socket.selectEntered.RemoveListener(OnPlankPlaced);
        socket.selectExited.RemoveListener(OnPlankRemoved);
    }

    private void OnPlankPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("Plank"))
        {
            plankPlaced = true;
            if (nailsHammered == 0)
            {
                emission.rateOverTime = plankCoverEmission;
            }
        }
    }

    private void OnPlankRemoved(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("Plank"))
        {
            plankPlaced = false;
            emission.rateOverTime = maxEmission; // Reset to max if plank is removed
        }
    }

    public void HammerNail()
    {
        if (plankPlaced && nailsHammered < totalNails)
        {
            nailsHammered++;
            AdjustEmissionRate();
        }
    }

    private void AdjustEmissionRate()
    {
        float emissionStep = plankCoverEmission / totalNails; // Decrease per nail
        emission.rateOverTime = plankCoverEmission - (emissionStep * nailsHammered);

        if (nailsHammered == totalNails)
        {
            emission.rateOverTime = 0f; // Fully sealed
        }
    }
}
