using UnityEngine;
using System.Collections;

public class CAUJump : CharacterActionUnit {

  public override void ActionStart () {
    CharacterMain.obj.ActiveVelocity += new Vector2 (0, 25f);
  }

  public override void ActionContinue () {
    CharacterMain.obj.ActiveVelocity += new Vector2 (0, 25f);
  }

  public override void ActionFinish () {
    
  }
}
