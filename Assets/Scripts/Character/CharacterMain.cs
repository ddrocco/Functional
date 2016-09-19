using UnityEngine;
using System.Collections;

public class CharacterMain : MonoBehaviour {
  public static CharacterMain obj;

  public Rigidbody2D rigid;
  public Vector2 ActiveVelocity = Vector2.zero;
  public Vector2 PassiveTargetVelocity = Vector2.zero;
  float MaxPassiveVelocityDelta = 1f;

  void Awake () {
    obj = this;
  }

  // Use this for initialization
  void Start () {
    rigid = GetComponent<Rigidbody2D> ();
  }
	
  // Update is called once per frame
  void Update () {
    ResolveVelocity ();
	  
  }

  void ResolveVelocity () {
    if (ActiveVelocity.magnitude > 0) {
      rigid.velocity = ActiveVelocity;
      ActiveVelocity = Vector2.zero;
    } else {
      float new_x, new_y = 0f;

      // Set Passive X Velocity.
      if (PassiveTargetVelocity.x < rigid.velocity.x) {
        new_x = Mathf.Max (rigid.velocity.x - MaxPassiveVelocityDelta, PassiveTargetVelocity.x);
      } else {
        new_x = Mathf.Min (rigid.velocity.x + MaxPassiveVelocityDelta, PassiveTargetVelocity.x);
      }
      // Set Passive Y Velocity.
      if (PassiveTargetVelocity.y < rigid.velocity.y) {
        new_y = Mathf.Max (rigid.velocity.y - MaxPassiveVelocityDelta, PassiveTargetVelocity.y);
      } else {
        new_y = Mathf.Min (rigid.velocity.y + MaxPassiveVelocityDelta, PassiveTargetVelocity.y);
      }
      rigid.velocity = new Vector2 (new_x, new_y);
    }
  }
}
