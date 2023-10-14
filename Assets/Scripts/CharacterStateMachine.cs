using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public CharacterState CurrentState { get; private set; }

    private void Awake()
    {
        CurrentState = CharacterState.Idle;
    }

    public void ChangeState(CharacterState newState)
    {
        CurrentState = newState;
        // Handle state change logic here
        // Stop movement, reset animations, etc.
    }
}

