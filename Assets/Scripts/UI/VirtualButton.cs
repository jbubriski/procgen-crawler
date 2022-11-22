﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	private Image _backgroundImage;
	private Image _buttonImage;
	private Vector3 _inputVector;

	void Start ()
	{
		_backgroundImage = GetComponent<Image>();
		_buttonImage = transform.GetChild(0).GetComponent<Image>();

        if (!IsMobile())
        {
            _backgroundImage.enabled = false;
            _buttonImage.enabled = false;
        }
    }
	
	void Update ()
	{

    }

    public bool IsMobile()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer
            || Application.platform == RuntimePlatform.Android;
    }

    public void OnDrag(PointerEventData pointerEventData)
	{
		Vector2 position;

		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform, 
			pointerEventData.position,
			pointerEventData.pressEventCamera,
			out position))
		{
			position.x = (position.x / _backgroundImage.rectTransform.sizeDelta.x);
			position.y = (position.y / _backgroundImage.rectTransform.sizeDelta.y);

			_inputVector = new Vector3(position.x * 2 - 1, 0, position.y * 2 - 1);
			_inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

			_buttonImage.rectTransform.anchoredPosition = new Vector3(_inputVector.x * (_backgroundImage.rectTransform.sizeDelta.x / 3),
				_inputVector.z * (_backgroundImage.rectTransform.sizeDelta.y / 3));
		}
	}

	public float Horizontal()
	{
		if (_inputVector.x != 0)
			return _inputVector.x;
		else
			return Input.GetAxis("Horizontal");
	}

	public float Vertical()
	{
		if (_inputVector.z != 0)
			return _inputVector.z;
		else
			return Input.GetAxis("Vertical");
	}

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		OnDrag(pointerEventData);
	}

	public void OnPointerUp(PointerEventData pointerEventData)
	{
		_inputVector = Vector3.zero;
		_buttonImage.rectTransform.anchoredPosition = Vector3.zero;
	}
}
