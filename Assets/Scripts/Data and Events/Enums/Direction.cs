using System;
using UnityEngine;

[Serializable]
public enum Direction
{
    center = 0,
    up = 1,
    left = 2,
    right = 3,
    down = 4
}

public static class DirectionUtility
{
    public static Vector2 ToVector2(Direction direction)
    {
        switch (direction)
        {
            case Direction.center:
                return Vector2.zero;
            case Direction.up:
                return Vector2.up;
            case Direction.left:
                return Vector2.right * -1;
            case Direction.right:
                return Vector2.right;
            case Direction.down:
                return Vector2.up * -1; 
            default:
                return Vector2.zero;
        }
    }
}

