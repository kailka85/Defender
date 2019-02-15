using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInstructions : MonoBehaviour
{
    private PlayerController _player;

    private void Awake()
    {
        _player = PlayerController.Instance;
    }
    private void OnEnable()
    {
        _player.ControlInputAssigned += OnControlInputAssigned;
    }

    private void OnDisable()
    {
        
    }

    private void OnControlInputAssigned(ControlInputs inputs)
    {
        if (inputs == ControlInputs.Mobile)
            gameObject.SetActive(false);
    }
}
