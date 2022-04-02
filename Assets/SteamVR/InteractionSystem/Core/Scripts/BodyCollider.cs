//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Collider dangling from the player's head
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( CapsuleCollider ) )]
	public class BodyCollider : MonoBehaviour
	{
		public Transform head;
		public static bool isGrounded = false;

		private CapsuleCollider capsuleCollider;
		public LayerMask terrainLayerMask;

		//-------------------------------------------------
		void Awake()
		{
			capsuleCollider = GetComponent<CapsuleCollider>();
			Debug.Log("isgrounded: " + isGrounded);
		}


		//-------------------------------------------------
		void FixedUpdate()
		{
		}

		private void OnTriggerEnter(Collider other) {
			Debug.Log("trigger tag: " + other.tag );
			isGrounded = other != null && (((1 << other.gameObject.layer) & terrainLayerMask) != 0);
			Debug.Log("isgrounded: " + isGrounded);
		}

		private void OnTriggerExit(Collider other) {
			isGrounded = false;
			Debug.Log("isgrounded: " + isGrounded);
		}
	}
}
