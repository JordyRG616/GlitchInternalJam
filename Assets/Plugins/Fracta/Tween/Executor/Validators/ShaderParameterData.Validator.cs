using UnityEngine;

public partial class ShaderParameterData
{
    private bool IsFloat => parameterType == ParameterType.Float;
    private bool IsVector2 => parameterType == ParameterType.Vector2;
    private bool IsVector3 => parameterType == ParameterType.Vector3;
    private bool IsVector4 => parameterType == ParameterType.Vector4;
    private bool IsColor => parameterType == ParameterType.Color;
}