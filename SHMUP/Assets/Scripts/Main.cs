using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

	public WeaponDefinition[] weaponDefinitions;

	WeaponType[] activeWeaponTypes;

	static public WeaponDefinition GetWeaponDefinition( WeaponType wt ) {
		// Check to make sure that the key exists in the Dictionary
		if (W_DEFS.ContainsKey(wt)) {
			// Attempting to retrieve a key that doesn't exist, throws an error
			return( W_DEFS[wt]);
		}
		// This will return a definition for WeaponType.none,
		//   which means it has failed to find the WeaponDefinition
		return( new WeaponDefinition() );
	}

	void Awake(){
		// A generic Dictionary with WeaponType as the key
		W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
		foreach( WeaponDefinition def in weaponDefinitions ) {
			W_DEFS[def.type] = def;
		}

	}

	// Use this for initialization
	void Start () {
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for ( int i=0; i<weaponDefinitions.Length; i++ )  {
			activeWeaponTypes[i] = weaponDefinitions[i].type;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
