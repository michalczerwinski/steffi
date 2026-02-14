using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public class GenerateModelBuilderAttribute : Attribute
{
}