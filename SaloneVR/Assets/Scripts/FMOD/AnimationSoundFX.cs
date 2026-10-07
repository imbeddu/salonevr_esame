using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundFX : MonoBehaviour
{
    
    public FMODUnity.EventReference[] PlaySoundEvent;
    public FMODUnity.EventReference[] FootstepSoundEvent;

    
    public GameObject RaycastFoot;

    public string SpeedParameter = "Speed";
    public string TagParameterName="TagMaterial";
    

    private string FeetTag;
    private Vector3 collision;
    private FMOD.Studio.EventInstance FootSound;
    public Animator Controller;
    

    public bool DebugFeetTag;
    public bool DebugSpeed;
    public bool DebugGizmo;

    [TagSelector] public string[] FootstepsTag;


    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlaySoundFX(int SFX)
    {
        FMODUnity.RuntimeManager.PlayOneShot(PlaySoundEvent[SFX], transform.position);
    }

    private void PlayFootstep(int SoundEvent)
    {
        FootSound = FMODUnity.RuntimeManager.CreateInstance(FootstepSoundEvent[SoundEvent]);
        FootSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        var ray = new Ray(RaycastFoot.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            collision = hit.point;
            FeetTag = hit.transform.gameObject.tag;
            if (DebugFeetTag) Debug.Log("Tag Name="+FeetTag);
        }

        for (int i = 0; i < FootstepsTag.Length; i++)
        {
            if (FeetTag == FootstepsTag[i]) /*& Controller.GetFloat("Speed")>1f)*/
            {
                if (DebugFeetTag) Debug.Log(TagParameterName +"="+ i);
                FootSound.setParameterByName(TagParameterName, (float)i);
                

            }
        }
        if (DebugSpeed) Debug.Log(Controller.GetFloat("Speed"));
        if (SpeedParameter!="") FootSound.setParameterByName(SpeedParameter, Controller.GetFloat("Speed"));
        FootSound.start();
        FootSound.release();
        
    }

    private void OnDrawGizmos()
    {
        if (DebugGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(collision, 0.2f);
        }
    }
}
