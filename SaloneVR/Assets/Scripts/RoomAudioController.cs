using UnityEngine;
using FMODUnity;

public class RoomAudioController : MonoBehaviour
{
    [Header("Emettitori presenti nella stanza")]
    [Tooltip("Trascina qui i GameObject dell'Orologio e della Stufa")]
    [SerializeField] private StudioEventEmitter[] roomEmitters;

    private void OnTriggerEnter(Collider other)
    {
        // Quando il Player entra nella stanza, attiva gli audio
        if (other.CompareTag("MainCamera"))
        {
            foreach (var emitter in roomEmitters)
            {
                if (emitter != null && !emitter.IsPlaying())
                {
                    emitter.Play();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Quando il Player esce dalla stanza, spegni gli audio
        if (other.CompareTag("MainCamera"))
        {
            foreach (var emitter in roomEmitters)
            {
                if (emitter != null && emitter.IsPlaying())
                {
                    // Nota: FMOD eseguirà automaticamente il fade-out 
                    // configurato su FMOD Studio se la traccia lo prevede.
                    emitter.Stop(); 
                }
            }
        }
    }
}