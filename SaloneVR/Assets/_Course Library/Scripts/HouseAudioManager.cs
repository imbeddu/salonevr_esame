using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HouseAudioManager : MonoBehaviour
{
    [Header("Gruppi audio")]
    [SerializeField] private Transform insideRoot;   // padre suoni interni
    [SerializeField] private Transform outsideRoot;  // padre suoni esterni

    [Header("Tappeti")]
    [SerializeField] private Collider[] insideRugs;
    [SerializeField] private Collider[] outsideRugs;

    [Header("Player")]
    [SerializeField] private Transform head;         // vuoto = Camera.main

    [Header("Comportamento")]
    [SerializeField] private float fadeSpeed = 2f;   // 2 = fade di mezzo secondo
    [SerializeField] private float heightTolerance = 3f;
    [SerializeField] private bool startInside = true;
    [SerializeField] private bool pauseWhenSilent = false; // risparmia CPU, ma congela l'audio

    private readonly List<StudioEventEmitter> inside = new();
    private readonly List<StudioEventEmitter> outside = new();

    private float insideVol, outsideVol;
    private bool isInside;

    private void Awake()
    {
        if (insideRoot)  inside.AddRange(insideRoot.GetComponentsInChildren<StudioEventEmitter>(true));
        if (outsideRoot) outside.AddRange(outsideRoot.GetComponentsInChildren<StudioEventEmitter>(true));

        isInside = startInside;
        insideVol  = isInside ? 1f : 0f;
        outsideVol = isInside ? 0f : 1f;
    }

    private void Start()
    {
        if (head == null && Camera.main != null) head = Camera.main.transform;
        Apply(inside, insideVol);
        Apply(outside, outsideVol);
    }

    private void Update()
    {
        if (head == null) return;

        if (IsOnAny(outsideRugs))      isInside = false;
        else if (IsOnAny(insideRugs))  isInside = true;
        // altrimenti resta lo stato precedente

        float step = fadeSpeed * Time.deltaTime;
        insideVol  = Mathf.MoveTowards(insideVol,  isInside ? 1f : 0f, step);
        outsideVol = Mathf.MoveTowards(outsideVol, isInside ? 0f : 1f, step);

        Apply(inside, insideVol);
        Apply(outside, outsideVol);
    }

    private void Apply(List<StudioEventEmitter> list, float vol)
    {
        foreach (var e in list)
        {
            if (e == null || !e.IsActive) continue;
            var inst = e.EventInstance;
            if (!inst.isValid()) continue;

            inst.setVolume(vol);
            if (pauseWhenSilent) inst.setPaused(vol <= 0.001f);
        }
    }

    private bool IsOnAny(Collider[] rugs)
    {
        Vector3 p = head.position;
        foreach (var r in rugs)
        {
            if (r == null) continue;
            Bounds b = r.bounds;
            if (p.x >= b.min.x && p.x <= b.max.x &&
                p.z >= b.min.z && p.z <= b.max.z &&
                Mathf.Abs(p.y - b.max.y) <= heightTolerance)
                return true;
        }
        return false;
    }
}