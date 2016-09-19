using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;

public class CharacterActionSequence : MonoBehaviour {

  public struct CharacterActionCommand {
    public float start_time;
    public float end_time;
    public CharacterActionUnit action;

    public CharacterActionCommand (float start, float end, CharacterActionUnit action) {
      this.start_time = start;
      this.end_time = end;
      this.action = action;
    }
  }

  // Must be sorted by start_time.
  public List<CharacterActionCommand> action_commands = new List<CharacterActionCommand> ();

  List<CharacterActionCommand> active_commands = new List<CharacterActionCommand> ();
  bool is_active;
  float current_time;
  int next_action;
  const float duration = 5f;

  void Start () {
    is_active = false;

    // Dev:
    action_commands.Add (
      new CharacterActionCommand (0.1f, 0.2f, new CAUJump ()));
  }

  public void Activate () {
    if (!is_active) {
      is_active = true;
      current_time = 0f;
    }
  }

  void Update () {
    if (!is_active) {
      return;
    }
    current_time += Time.deltaTime;
    // End old action_commands and continue existing action_commands:
    var removed_commands = new List<CharacterActionCommand> ();
    foreach (CharacterActionCommand command in active_commands) {
      if (current_time > command.end_time) {
        command.action.ActionFinish ();
        removed_commands.Add (command);
      } else {
        command.action.ActionContinue ();
      }
    }
    foreach (CharacterActionCommand command in removed_commands) {
      active_commands.Remove (command);
    }

    // Start new action_commands:
    while (next_action < action_commands.Count) {
      var next_command = action_commands [next_action];
      if (current_time > next_command.start_time) {
        ++next_action;
        next_command.action.ActionStart ();
        active_commands.Add (next_command);
      } else {
        break;
      }
    }

    if (next_action == action_commands.Count) {
      is_active = false;
    }
  }
}
