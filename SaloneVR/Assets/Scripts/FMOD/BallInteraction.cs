using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    public InputAction interactControl;
	public GameObject ParticleBlast;
	public float ForceY = 100;
	private Rigidbody rbody;
	private bool InTrigger = false;
	public GameObject UiX;

	private void OnEnable()
    {
        interactControl.Enable();
    }

    private void OnDisable()
    {
        interactControl.Disable();
    }

    void Start()
    {
		rbody = GetComponent<Rigidbody>();
		interactControl.performed += context =>
		{
			
			if (InTrigger)
			{
				ParticleBlast.SetActive(true);
				rbody.AddForce(new Vector3(ForceY/2, ForceY, 0));
			}
		};
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter(Collider Trig)
	{
		if (Trig.gameObject.tag == "Player")
		{
			InTrigger = true;
			UiX.SetActive(true);
		}


	}

	void OnTriggerExit(Collider Trig)
	{
		if (Trig.gameObject.tag == "Player")
		{
			InTrigger = false;
			UiX.SetActive(false);
		}


	}
}
