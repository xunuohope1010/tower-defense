using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private Animator myAnimator;

	// Use this for initialization
	void Start () {
        myAnimator = GetComponent<Animator>();
	}
	
	public void Open()
    {
        myAnimator.SetTrigger("Open");
    }
}
