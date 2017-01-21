using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPosition { LEFT, RIGHT};
public class Player : MonoBehaviour {
    public PlayerPosition position;
    AttackManager atkManager;

	// Use this for initialization
	void Start () {
        atkManager = GetComponent<AttackManager>();
        GameManagerScript.Instance.RegisterPlayer(gameObject, position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack(ControllerKeys button)
    {
        if (this.atkManager.Attack(button))
            GameManagerScript.Instance.Attack(position);
    }
}
