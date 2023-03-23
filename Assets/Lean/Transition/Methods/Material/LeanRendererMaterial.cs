using UnityEngine;
using System.Collections.Generic;
using TARGET = UnityEngine.MeshRenderer;

namespace Lean.Transition.Method
{
    /// <summary>This component allows you to transition the specified <b>Material</b>'s <b>color</b> to the target value.</summary>
    [HelpURL(LeanTransition.HelpUrlPrefix + "LeanRendererMaterial")]
    [AddComponentMenu(LeanTransition.MethodsMenuPrefix + "Material/Renderer Material" + LeanTransition.MethodsMenuSuffix + "(LeanRendererMaterial)")]
    public class LeanRendererMaterial : LeanMethodWithStateAndTarget
    {
        public override System.Type GetTargetType()
        {
            return typeof(TARGET);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Value, Data.index, Data.Duration, Data.Ease);
        }

        public static LeanState Register(TARGET target, Material value, int index, float duration, LeanEase ease = LeanEase.Smooth)
        {
            var state = LeanTransition.SpawnWithTarget(State.Pool, target);

            state.Value = value;
            state.index = index;

            state.Ease = ease;

            return LeanTransition.Register(state, duration);
        }

        [System.Serializable]
        public class State : LeanStateWithTarget<TARGET>
        {
            [UnityEngine.Tooltip("The color value will transition to this.")]
             public Material Value = null;
             public int index = 0;

            [UnityEngine.Tooltip("This allows you to control how the transition will look.")]
            public LeanEase Ease = LeanEase.Smooth;

            [System.NonSerialized] private Material oldValue;

            public override int CanFill
            {
                get
                {
                    return Target != null && Target.sharedMaterials[index] != Value? 1 : 0;
                }
            }

            public override void FillWithTarget()
            {
                Value = Target.sharedMaterials[index];
            }

            public override void BeginWithTarget()
            {
                oldValue = Target.sharedMaterials[index];
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.sharedMaterials[index] = progress > 0.5f ? Value : oldValue;
            }

            public static System.Collections.Generic.Stack<State> Pool = new System.Collections.Generic.Stack<State>(); public override void Despawn() { Pool.Push(this); }
        }

        public State Data;
    }
}

namespace Lean.Transition
{
    public static partial class LeanExtensions
    {
        public static TARGET colorTransition(this TARGET target, Material value, int index, float duration, LeanEase ease = LeanEase.Smooth)
        {
            Method.LeanRendererMaterial.Register(target, value, index, duration, ease); return target;
        }
    }
}