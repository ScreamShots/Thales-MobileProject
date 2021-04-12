using System;
using UnityEngine;

///<summary>

/// Rémi Sécher / 21.30.03 / Attribute to flag value that need to be considered for the Tweek System. 

///</summary>

namespace Tweek.FlagAttributes
{
    //Define in which type of SCO assets they need to be displayed
    public enum FieldUsage { Default, Art, Gameplay, Sound }

    [AttributeUsage(AttributeTargets.Field)]
    public class TweekFlagAttribute : Attribute
    {
        public FieldUsage fieldUsage { get; private set; }
        public TweekFlagAttribute(FieldUsage _fieldUsage = FieldUsage.Default)
        {
            fieldUsage = _fieldUsage;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TweekClassAttribute : Attribute
    {
        public TweekClassAttribute()
        {

        }
    }
}

///<summary>

/// Rémi Sécher / 21.30.03 / Attributes that allow Tweek System to read SCO values correctly and draw custom editor for SCO. 

///</summary>

namespace Tweek.ScoAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IdAttribute : PropertyAttribute
    {
        public string displayName { get; private set; }
        public IdAttribute(string _displayName = null)
        {
            if (_displayName != null) displayName = _displayName;
            else displayName = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CompAttribute : PropertyAttribute
    {
        public string displayName { get; private set; }
        public CompAttribute( string _displayName = null)
        {
            if (_displayName != null) displayName = _displayName;
            else displayName = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PathAttribute : PropertyAttribute
    {
        public string displayName { get; private set; }
        public PathAttribute(string _displayName = null)
        {
            if (_displayName != null) displayName = _displayName;
            else displayName = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class VarAttribute : PropertyAttribute
    {
        public string displayName { get; private set; }
        public VarAttribute(string _displayName = null)
        {
            if (_displayName != null) displayName = _displayName;
            else displayName = string.Empty;
        }
    }
}
