using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControllerInputVisualizer : ControllerInputHandler {

	public GameObject visualizer_prefab;

	Transform LVisualizerTransform;
	Transform RVisualizerTransform;
	Material LVisualizerMaterial;
	Material RVisualizerMaterial;
	Renderer AVisualizer;

	int x_displacement {
		get {
			return id;
		}
	}


	// Use this for initialization
	new protected void Start () {
		GameObject LObject = Instantiate(visualizer_prefab) as GameObject;
		LVisualizerTransform = LObject.transform;
		LVisualizerTransform.SetParent(transform);
		LVisualizerMaterial = LObject.GetComponent<Renderer>().material;
		GameObject RObject = Instantiate(visualizer_prefab) as GameObject;
		RVisualizerTransform = RObject.transform;
		RVisualizerTransform.SetParent(transform);
		RVisualizerMaterial = RObject.GetComponent<Renderer>().material;
		GameObject AObject = Instantiate(visualizer_prefab) as GameObject;
		AVisualizer = AObject.GetComponent<Renderer>();
		base.Start();
	}

	override protected void LAct(float y) {
		LVisualizerTransform.transform.localPosition = new Vector3(-1 - x_displacement, y, 0);
		LVisualizerMaterial.color = new Color(x_displacement + 0.5f + y/2f, 0.5f, 0.5f + y/4f);
	}

	override protected void RAct(float y) {
		RVisualizerTransform.transform.localPosition = new Vector3(1 + x_displacement, y, 0);
		RVisualizerMaterial.color = new Color(0.5f - y/4f, 0.5f - y/2f, 0.5f);
	}

	override protected void AAct(bool y) {
		AVisualizer.enabled = y;
	}

	override protected void ResolveActions() {
		return;
	}

	override public void CleanupPositions() {
		return;
	}
}
