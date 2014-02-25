// Called when the cursor is actually being locked
	function DidLockCursor () {
		Debug.Log("Locking cursor");
		// Disable the button
		guiTexture.enabled = false;
	}
	// Called when the cursor is being unlocked
	// or by a script calling Screen.lockCursor = false;
	function DidUnlockCursor () {
		Debug.Log("Unlocking cursor");
		// Show the button again
		guiTexture.enabled = true;
	}
	function OnMouseDown () {
		// Lock the cursor
		Screen.lockCursor = true;
	}
	private var wasLocked = false;
	function Update () {
		// In standalone player we have to provide our own key
		// input for unlocking the cursor
		if (Input.GetKeyDown ("escape"))
			Screen.lockCursor = false;
		// Did we lose cursor locking?
		// eg. because the user pressed escape
		// or because he switched to another application
		// or because some script set Screen.lockCursor = false;
		if (!Screen.lockCursor && wasLocked) {
			wasLocked = false;
			DidUnlockCursor();
		}
		// Did we gain cursor locking?
		else if (Screen.lockCursor && !wasLocked) {
			wasLocked = true;
			DidLockCursor ();
		}
	}