using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Sachet.Attributes
{
    #region Attribute

    #region DrawIf

    public enum ComparisonType
    {
        Equals = 1,
        NotEqual = 2,
        GreaterThan = 3,
        SmallerThan = 4,
        SmallerOrEqual = 5,
        GreaterOrEqual = 6
    }

    public enum DisablingType
    {
        ReadOnly = 2,
        DontDraw = 3
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DrawIfAttribute : PropertyAttribute
    {
        public string comparedPropertyName { get; private set; }
        public object comparedValue { get; private set; }
        public ComparisonType comparisonType { get; private set; }
        public DisablingType disablingType { get; private set; }

        /// <summary>
        /// Only draws the field only if a condition is met.
        /// </summary>
        /// <param name="comparedPropertyName">The name of the property that is being compared (case sensitive).</param>
        /// <param name="comparedValue">The value the property is being compared to.</param>
        /// <param name="comparisonType">The type of comperison the values will be compared by.</param>
        /// <param name="disablingType">The type of disabling that should happen if the condition is NOT met. Defaulted to DisablingType.DontDraw.</param>
        public DrawIfAttribute(string comparedPropertyName, object comparedValue, ComparisonType comparisonType, DisablingType disablingType = DisablingType.DontDraw)
        {
            this.comparedPropertyName = comparedPropertyName;
            this.comparedValue = comparedValue;
            this.comparisonType = comparisonType;
            this.disablingType = disablingType;
        }
    }

    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {
        // Reference to the attribute on the property.
        DrawIfAttribute drawIf;

        // Field that is being compared.
        SerializedProperty comparedField;
        Type comparedType;
        FieldInfo fieldInfo;

        // Height of the property.
        private float propertyHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Set the global variables.
            drawIf = attribute as DrawIfAttribute;
            comparedField = property.serializedObject.FindProperty(drawIf.comparedPropertyName);
            comparedType = comparedField.GetType();

            // Get the value of the compared field.
            //object comparedFieldValue = comparedField.GetValue<object>();

            object comparedFieldValue = null;

            fieldInfo = comparedType.GetField(property.propertyPath);
            if (fieldInfo != null)
            {
                comparedFieldValue = fieldInfo.GetValue(comparedField);
            }
            Debug.Log(fieldInfo);
            //Debug.Log(comparedFieldValue);

            // References to the values as numeric types.
            float numericComparedFieldValue = 0f;
            float numericComparedValue = 0f;

            try
            {
                // Try to set the numeric types.
                numericComparedFieldValue = (float)comparedFieldValue;
                numericComparedValue = (float)drawIf.comparedValue;
            }
            catch (Exception e)
            {
                // This place will only be reached if the type is not a numeric one. If the comparison type is not valid for the compared field type, log an error.
                if (drawIf.comparisonType != ComparisonType.Equals && drawIf.comparisonType != ComparisonType.NotEqual)
                {
                    Debug.LogError("The only comparsion types available to type '" + comparedFieldValue.GetType() + "' are Equals and NotEqual. (On object '" + property.serializedObject.targetObject.name + "')");
                    return;
                }
            }

            // Is the condition met? Should the field be drawn?
            bool conditionMet = false;

            // Compare the values to see if the condition is met.
            switch (drawIf.comparisonType)
            {
                case ComparisonType.Equals:
                    if (comparedFieldValue.Equals(drawIf.comparedValue))
                        conditionMet = true;
                    break;

                case ComparisonType.NotEqual:
                    if (!comparedFieldValue.Equals(drawIf.comparedValue))
                        conditionMet = true;
                    break;

                case ComparisonType.GreaterThan:
                    if ((float)numericComparedFieldValue > (float)numericComparedValue)
                        conditionMet = true;
                    break;

                case ComparisonType.SmallerThan:
                    if ((float)numericComparedFieldValue < (float)numericComparedValue)
                        conditionMet = true;
                    break;

                case ComparisonType.SmallerOrEqual:
                    if ((float)numericComparedFieldValue <= (float)numericComparedValue)
                        conditionMet = true;
                    break;

                case ComparisonType.GreaterOrEqual:
                    if ((float)numericComparedFieldValue >= (float)numericComparedValue)
                        conditionMet = true;
                    break;
            }

            // The height of the property should be defaulted to the default height.
            propertyHeight = base.GetPropertyHeight(property, label);

            // If the condition is met, simply draw the field. Else...
            if (conditionMet)
            {
                EditorGUI.PropertyField(position, property);
            }
            else
            {
                //...check if the disabling type is read only. If it is, draw it disabled, else, set the height to zero.
                if (drawIf.disablingType == DisablingType.ReadOnly)
                {
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property);
                    GUI.enabled = true;
                }
                else
                {
                    propertyHeight = 0f;
                }
            }
        }
    }
    #endregion

    #region ReadOnly
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    #endregion

    #endregion
}