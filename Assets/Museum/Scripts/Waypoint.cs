using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Waypoint : MonoBehaviour
{
	public bool	occupied = false;
	public bool	focused = false;
	public bool	triggered = false;

	public Color active_color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
	public Color hilight_color = new Color(0.8f, 0.8f, 1.0f, 0.125f);
	public Color disabled_color = new Color(0.125f, 0.125f, .125f, 0.0f);

	public float animation_scale = 1.5f;
	public float animation_speed = 3.0f;

	private	Vector3 _origional_scale = Vector3.one;

	private float _hilight = 0.0f;
	private float _hilight_fade_speed = 0.25f;

	public Rigidbody rigid_body;
	private Material _material;
    private Collider _collider;

	public Vector3 position = Vector3.zero;

	void Awake() {
		rigid_body = gameObject.GetComponent<Rigidbody>();
		_material = gameObject.GetComponent<MeshRenderer>().material;
        _collider = (gameObject.GetComponent(typeof(BoxCollider)) as Collider);
		_origional_scale = transform.localScale;

		if (position == Vector3.zero) {
			position = gameObject.transform.position;
		}
	}

	void LateUpdate() {
		if(!occupied) {
			Animate();
		} else {
            Deactivate();
		}
	}

	public void Occupy() {
		occupied = true;
        toggleChildren();
        GetComponent<AudioSource>().Play();
	}

	public void Depart() {
		occupied = false;
        Activate();
        toggleChildren();
	}

	public void Activate() {
		_material.color = active_color;
		transform.localScale = _origional_scale;
		
        showModels(true);
    }

	public void Deactivate() {
		_material.color = disabled_color;
		transform.localScale = _origional_scale * 0.5f;
		
		triggered = false;

        showModels(false);
	}

	public void Trigger() {
		if(focused && !occupied) {
			triggered = true;
			occupied = false;
			_hilight = 1.0f;
		}
	}

	public void Enter() {
		if(!focused) {
			focused = true;
			_hilight = .5f;
		}
	}

	public void Exit() {
		focused = false;
		_hilight = 1.0f;
	}

	private void Animate() {
		float pulse_animation = Mathf.Abs(Mathf.Cos(Time.time * animation_speed));
		
		_material.color = Color.Lerp(active_color, hilight_color, _hilight);
			
		_hilight = Mathf.Max(_hilight - _hilight_fade_speed, 0.0f);

		Vector3 hilight_scale = Vector3.one * (_hilight + (focused ? 0.5f : 0.0f));

		transform.localScale = Vector3.Lerp(_origional_scale + hilight_scale, _origional_scale * animation_scale + hilight_scale, pulse_animation);
	}

    private void showModels(bool show) {        
        foreach (MeshRenderer mesh in GetComponents<MeshRenderer>()) {
            mesh.enabled = show;
        }

        if (_collider != null) {
            _collider.enabled = show;
        }        
    }

    private void toggleChildren() {
        int children = gameObject.transform.childCount;
		for (int i = 0; i < children; i++) {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            bool isActive = child.activeSelf;
			child.SetActive(!isActive);
		}
    }
}
