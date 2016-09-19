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
using System.Security.Cryptography;

public class ControllerInputManager : MonoBehaviour {
  // A class which communicates with InputDevices and records commands.
  // Should be read by CharacterControllers to determine button presses.

  // Object references.
  private InputDevice device;

  // Whether to use the mouse for controls.
  private bool debugControls = false;

  // Used for left-right movement.
  public float lfeed;
  // Used to trigger ACTION1.
  public bool afeed;
  // Used to trigger ACTION2.
  public bool bfeed;
  // Used to trigger ACTION3.
  public bool xfeed;
  // Used to trigger ACTION4.
  public bool yfeed;

  void Awake () {
    lfeed = 0f;
    afeed = false;
    bfeed = false;
    xfeed = false;
    yfeed = false;

    device = InputManager.ActiveDevice;

    foreach (var Device in InputManager.Devices) {
      if (Device.Name.Contains ("Unknown")) {
        debugControls = true;
      }
    }
    if (InputManager.Devices.Count == 0) {
      debugControls = true;
      Debug.Log ("Debug controls active:\n" +
      "LeftStick: A (left), D (right)\n" +
      "A Button: H\n" +
      "B Button: J\n" +
      "X Button: K\n" +
      "Y Button: L\n" +
      "Keep 3-key-press in mind.");
    }
  }

  void Update () {
    if (debugControls) {
      DebugControlsUpdate ();
    } 
  }

  void DebugControlsUpdate () {
    if (!(Input.GetKey (KeyCode.A) ^ Input.GetKey (KeyCode.D))) {
      lfeed = 0f;
    } else if (Input.GetKey (KeyCode.D)) {
      lfeed = 1.0f;
    } else if (Input.GetKey (KeyCode.A)) {
      lfeed = -1.0f;
    }

    afeed = Input.GetKey (KeyCode.H);
    bfeed = Input.GetKey (KeyCode.J);
    xfeed = Input.GetKey (KeyCode.K);
    yfeed = Input.GetKey (KeyCode.L);
  }

  void XBoxControlsUpdate () {
    lfeed = device.LeftStickX;
    afeed = device.Action1.IsPressed; // A button on Xbox
    bfeed = device.Action2.IsPressed; // B button on Xbox
    xfeed = device.Action3.IsPressed; // X button on Xbox
    yfeed = device.Action4.IsPressed; // Y button on Xbox

  }
}