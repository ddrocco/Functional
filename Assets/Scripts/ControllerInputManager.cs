/* DEBUGGUING KEYBOARD CONTROLS
 * 
 * IJKL - movement
 * Mouse - look around
 * Tab - switch between Stan's mouse controls and Q's mouse controls
 * Left click - interact
 * O - sprint
 * 
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using IronPython.Runtime;

public class ControllerInputManager : MonoBehaviour
{
	// Object references.
	private InputDevice device;

	// TODO(ddrocco): Delete this and handle creation more elegantly.
	public GameObject ih_visualizer_prefab;
	public GameObject ih_tankbot_prefab;
	enum IHType {
		visualizer,
		tankbot,
	}

	// Current ControllerInputHandler.
	private ControllerInputHandler handler;
	private List<ControllerInputHandler> old_handlers = new List<ControllerInputHandler>();

	// Whether to use the mouse for controls.
	private bool debugControls = false;

	public float lfeed;
	public float rfeed;
	public bool afeed;
	public bool sfeed;

	void Awake() {
		lfeed = 0f;
		rfeed = 0f;
		afeed = false;
		sfeed = false;

		device = InputManager.ActiveDevice;

		foreach (var Device in InputManager.Devices) {
			if (Device.Name.Contains("Unknown")) {
				debugControls = true;
			}
		}
		if (InputManager.Devices.Count == 0) {
			debugControls = true;
			Debug.Log("Debug controls active:\n" +
					"LeftStick: Q (up), A (down)\n" +
					"RightStick: O (up), L (down)\n" +
					"A Button: Spacebar\n" +
					"X Button: Enter\n" + 
					"Keep 3-key-press in mind.");
		}

		handler = this.NewHandler(Vector3.zero);
	}

	void Update() {
		if (debugControls){
			DebugControlsUpdate();
		} 
	}

	void DebugControlsUpdate() {
		if (!(Input.GetKey(KeyCode.Q) ^ Input.GetKey(KeyCode.A))) {
			lfeed = 0f;
		} else if (Input.GetKey(KeyCode.Q)) {
			lfeed = 1.0f;
		} else if (Input.GetKey(KeyCode.A)) {
			lfeed = -1.0f;
		}

		if (!(Input.GetKey(KeyCode.O) ^ Input.GetKey(KeyCode.L))) {
			rfeed = 0f;
		} else if (Input.GetKey(KeyCode.O)) {
			rfeed = 1.0f;
		} else if (Input.GetKey(KeyCode.L)) {
			rfeed = -1.0f;
		}

		afeed = Input.GetKey(KeyCode.Return);
		sfeed = Input.GetKey(KeyCode.Space);
	}

	void XBoxControlsUpdate() {
		lfeed = device.LeftStickY;
		rfeed = device.RightStickY;
		afeed = device.Action3.IsPressed; // X button on Xbox
		sfeed = device.Action1.IsPressed; // A button on Xbox
	}

	// Called by MasterTimer on the first step during FixedUpdate action.
	public void _TimerStartOfStream() {
		handler.StartOfStream();
	}

	// Called by MasterTimer on the last step during FixedUpdate action.
	public void _TimerEndOfStream(int step) {
		handler.EndOfStream(step);
		old_handlers.Add(handler);
		// TODO(ddrocco): Make this controllable and dynamic.
		handler = this.NewHandler(new Vector3(2 * old_handlers.Count, 0, 0));
	}

	void CheckDebugStates()
	{
	}

	// Sets the player's movement direction based on controller left stick input
	// Called in update, so controller input is registered as quickly as possible
	void SetMoveDirection()
	{

	}

	ControllerInputHandler NewHandler(Vector3 position, IHType ih_type = IHType.tankbot) {
		GameObject new_handler_object = null;
		switch (ih_type) {
		case IHType.visualizer:
			new_handler_object = Instantiate(ih_visualizer_prefab) as GameObject;
			Debug.Log("Intantiating Visualizer");
			break;
		case IHType.tankbot:
			new_handler_object = Instantiate(ih_tankbot_prefab) as GameObject;
			Debug.Log("Intantiating Tankbot");
			break;
		}
		new_handler_object.transform.position = position;
		ControllerInputHandler new_handler = new_handler_object.GetComponent<ControllerInputHandler>();
		new_handler.SetControllerInputManager(this);
		return new_handler;
	}
}