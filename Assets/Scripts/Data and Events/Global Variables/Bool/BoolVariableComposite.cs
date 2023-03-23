using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Bool Variable Composite")]
public class BoolVariableComposite : BoolVariable
{
    public BoolReference bool1 = new BoolReference(false);
    public BoolReference bool2 = new BoolReference(false);
    [SerializeField]
    private BoolComparison comparisonMode;

    public override bool Value
    {
        get
        {
            switch (comparisonMode)
            {
                case BoolComparison.Equal:
                    return bool1.Value == bool2.Value;
                case BoolComparison.NotEqual:
                    return bool1.Value != bool2.Value;
                case BoolComparison.And:
                    return bool1.Value && bool2.Value;
                case BoolComparison.Or:
                    return bool1.Value || bool2.Value;
                default:
                    return false;
            }
        }
        set
        {
            
        }
    }
    protected override void Awake()
    {
        
    }
    protected override void OnEnable()
    {
        if(!bool1.useConstant && bool1.variable != null)
            bool1.variable.OnChanged += RaiseOnChanged;
        if(!bool2.useConstant && bool2.variable != null)
            bool2.variable.OnChanged += RaiseOnChanged;
    }

    protected override void OnDisable()
    {
        if(!bool1.useConstant && bool1.variable != null)
            bool1.variable.OnChanged -= RaiseOnChanged;
        if(!bool2.useConstant && bool2.variable != null)
            bool2.variable.OnChanged -= RaiseOnChanged;
    }
}