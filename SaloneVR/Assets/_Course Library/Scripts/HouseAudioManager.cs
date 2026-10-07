using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HouseAudioManager : MonoBehaviour
{
    [SerializeField] private Transform houseRoot;
    [SerializeField] private Collider[] outsideRugs;
    [SerializeField] private Collider[] insideRugs;
    [SerializeField] private Transform head;
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float heightTolerance = 3f;

    private readonly List<StudioEventEmitter> emitters = new();
    private float volume = 1f, target = 1f;

    private void Awake()
    {
        emitters.AddRange(houseRoot.GetComponentsInChildren<StudioEventEmitter>(true));
    }

    private void Start()
    {
        if (head == null && Camera.main != null) head = Camera.main.transform;
    }

    private void Update()
    {
        if (head == null) return;

        if (IsOnAny(outsideRugs)) target = 0f;
        else if (IsOnAny(insideRugs)) target = 1f;
        // altrimenti mantiene l'ultimo stato

        volume = Mathf.MoveTowards(volume, target, fadeSpeed * Time.deltaTime);
        foreach (var e in emitters)
            if (e != null && e.IsActive) e.EventInstance.setVolume(volume);
    }

    private bool IsOnAny(Collider[] rugs)
    {
        Vector3 p = head.position;
        foreach (var r in rugs)
        {
            Bounds b = r.bounds;
            if (p.x >= b.min.x && p.x <= b.max.x &&
                p.z >= b.min.z && p.z <= b.max.z &&
                Mathf.Abs(p.y - b.max.y) <= heightTolerance)
                return true;
        }
        return false;
    }
}
