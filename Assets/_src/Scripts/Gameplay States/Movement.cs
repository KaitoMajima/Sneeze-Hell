using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KaitoCo
{
    public static class Movement
    {

        public static void Move(ref MovementState state, in MovementSettings settings, in MovementInput input, float deltaTime)
        {
            state.Velocity = input.MoveVector * (settings.MovementSpeed * deltaTime);
        }
        
        public static void Move(ref MovementState state, in MovementSettings settings, in Vector2 input, float deltaTime)
        {
            state.Velocity = input * (settings.MovementSpeed * deltaTime);
        }
        public static void FlipHorizontally(ref MovementState state, float movementValue)
        {
            state.isXFlipped = state.LocalScale.x < 0;
            bool willFlip = !state.isXFlipped && movementValue < 0 || state.isXFlipped && movementValue > 0;
            if (willFlip)
            {
                state.LocalScale = new Vector2(state.LocalScale.x * -1, state.LocalScale.y); 
            }

        }

        public static void FlipVertically(ref MovementState state, float movementValue)
        {
            state.isYFlipped = state.LocalScale.y < 0;
            bool willFlip = !state.isYFlipped && movementValue < 0 || state.isYFlipped && movementValue > 0;
            if (willFlip)
            {
                state.LocalScale = new Vector2(state.LocalScale.x, state.LocalScale.y * -1); 
            }

        }
        public static void SetScale(ref MovementState state, Vector3 scale)
        {
            state.LocalScale = scale;
        }
        public static void SetPosition(ref MovementState state, Vector3 position)
        {
            state.Position = position;
        }
    }
    [Serializable]
    public struct MovementState
    {
        public Vector3 Position;
        public Vector3 LocalScale;
        public bool isXFlipped;

        public bool isYFlipped;
        public Vector2 Velocity;
        
    }
    [Serializable]
    public struct MovementSettings
    {
        public float MovementSpeed;
        public static MovementSettings Default = new MovementSettings()
        {
            MovementSpeed = 8
        };
    }
    [Serializable]
    public struct MovementInput 
    {
        public Vector2 MoveVector;
    }

    public struct MovementFlip
    {
        public bool isXFlipped;

    }
}
