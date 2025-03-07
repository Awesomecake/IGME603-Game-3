using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class UISway : MonoBehaviour
{
	[SerializeField]private float amount;
	[SerializeField] private float speed;
	[SerializeField] private Vector2 offset;

	private RectTransform rect; 

    private void Start()
    {
		rect = GetComponent<RectTransform>();
	}

    private void Update()
	{
		if (Mouse.current == null) return;

		Vector2 targetPosition = (-Mouse.current.position.ReadValue() + new Vector2(Screen.width / 2, Screen.height / 2)) * amount / 100 + offset;

		rect.localPosition = Vector3.Lerp(rect.localPosition, targetPosition, Time.unscaledDeltaTime * speed);
    }
}
