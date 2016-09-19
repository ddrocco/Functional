using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class CharacterActionHandler : MonoBehaviour {

  private ControllerInputManager input_manager;
  private CharacterMain character;
  const float runspeed = 3f;

  public CharacterActionSequence SeqA;
  public CharacterActionSequence SeqB;
  public CharacterActionSequence SeqX;
  public CharacterActionSequence SeqY;

  void Start () {
    input_manager = GameObject.FindObjectOfType<ControllerInputManager> ();
    character = GameObject.FindObjectOfType<CharacterMain> ();
  }

  void Update () {
    JoystickMovements (input_manager.lfeed);
    // print ("Moving with " + input_manager.lfeed);
    if (input_manager.afeed) {
      SeqA.Activate ();
      // print ("Seq A");
    }
    if (input_manager.bfeed) {
      SeqA.Activate ();
      // print ("Seq B");
    }
    if (input_manager.xfeed) {
      SeqA.Activate ();
      // print ("Seq C");
    }
    if (input_manager.yfeed) {
      SeqA.Activate ();
      // print ("Seq D");
    }
  }

  void JoystickMovements (float lrmovement) {
    // If over solid ground (or maybe flying?)
    character.PassiveTargetVelocity = new Vector2 (lrmovement * runspeed, Physics.gravity.y);
  }
}
