using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ControllerInputHandler : MonoBehaviour {

	protected int id;

	// Streams input to the handler.
	protected ControllerInputManager input;

	// Records the number of clock-steps in this system.
	// Should be aligned with the clock_length.
	protected int total_steps;

	// A stream of left-joystick actions.
	protected Queue<float> lstream;
	// A stream of right-joystick actions.
	protected Queue<float> rstream;
	// A stream of A-button presses.
	protected Queue<bool> astream;
	// A history of left-joystick actions.
	protected float[] lhistory;
	// A history of right-joystick actions.
	protected float[] rhistory;
	// A history of A-button presses.
	protected bool[] ahistory;

	protected enum State {
		streaming,
		waiting,
		running,
	};
	protected State state;

	// Use this for initialization
	protected void Start () {
		lstream = new Queue<float>();
		rstream = new Queue<float>();
		astream = new Queue<bool>();

		state = State.waiting;
		MasterTicker.main.handlers.Add(this);
		id = MasterTicker.main.handlers.Count - 1;

		LAct(0);
		RAct(0);
		AAct(false);
	}

	public void SetId(int i) {
		id = i;
	}

	public void SetControllerInputManager(ControllerInputManager c) {
		input = c;
	}

	// Called by MasterTimer as a FixedUpdate action.
	public void _TimerUpdateStep(int step) {
		switch (state) {
			case State.running:
				if (step < total_steps) {
					LAct(lhistory[step]);
					RAct(rhistory[step]);
					AAct(ahistory[step]);
					ResolveActions();
				} else {
					Pause();
				}
				break;
			case State.streaming:
				LAct(input.lfeed);
				RAct(input.rfeed);
				AAct(input.afeed);
				ResolveActions();
				lstream.Enqueue(input.lfeed);
				rstream.Enqueue(input.rfeed);
				astream.Enqueue(input.afeed);
				break;
			case State.waiting:
				Debug.Log(step + " zzz.");
				return;
		}
	}

	void Pause() {
		LAct(0);
		RAct(0);
		AAct(false);
		state = State.waiting;
	}

	public void StartOfStream() {
		state = State.streaming;
	}

	public void Playback() {
		if (state != State.streaming) {
			state = State.running;
		}
	}

	public void EndOfStream(int step) {
		lhistory = lstream.ToArray();
		rhistory = rstream.ToArray();
		ahistory = astream.ToArray();
		total_steps = step ;
		CleanupPositions();
		Pause();
	}

	virtual protected void LAct(float y) {
		throw new NotImplementedException();
	}

	virtual protected void RAct(float y) {
		throw new NotImplementedException();
	}

	virtual protected void AAct(bool y) {
		throw new NotImplementedException();
	}

	virtual protected void ResolveActions() {
		throw new NotImplementedException();
	}

	virtual public void CleanupPositions() {
		throw new NotImplementedException();
	}
}
