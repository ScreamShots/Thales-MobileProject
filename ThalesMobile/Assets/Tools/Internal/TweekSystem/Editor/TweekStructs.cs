using System;
using System.Collections.Generic;
using Tweek.FlagAttributes;

namespace Tweek.System
{
    public struct TweekObj
    {
        public string objName;
        public byte[] serializedGuid;
        public List<TweekComponent> attachedComponents;

        public bool gameplayFields { private set; get; }
        public bool artFields { private set; get; }
        public bool soundFields { private set; get; }

        public TweekObj(string _objName, byte[] _serializedGuid)
        {
            objName = _objName;
            serializedGuid = _serializedGuid;
            attachedComponents = new List<TweekComponent>();

            gameplayFields = false;
            artFields = false;
            soundFields = false;
        }

        public void AddComp(TweekComponent tweekComp)
        {
            attachedComponents.Add(tweekComp);

            if (tweekComp.artFields) artFields = true;
            if (tweekComp.gameplayFields) gameplayFields = true;
            if (tweekComp.soundFields) soundFields = true;
        }
    }

    public struct TweekComponent
    {
        public string componentName;
        public byte[] serializedGuid;
        public List<TweekField> fields;
        public bool gameplayFields { private set; get; }
        public bool artFields { private set; get; }
        public bool soundFields { private set; get; }

        public TweekComponent(string _componentName, byte[] _serializedGuid)
        {
            serializedGuid = _serializedGuid;
            componentName = _componentName;
            fields = new List<TweekField>();

            gameplayFields = false;
            artFields = false;
            soundFields = false;
        }

        public void AddField(TweekField tweekField)
        {
            fields.Add(tweekField);

            switch (tweekField.fieldUsage)
            {
                case FieldUsage.Default:
                    gameplayFields = true;
                    artFields = true;
                    soundFields = true;
                    break;
                case FieldUsage.Art:
                    artFields = true;
                    break;
                case FieldUsage.Gameplay:
                    gameplayFields = true;
                    break;
                case FieldUsage.Sound:
                    soundFields = true;
                    break;
                default:
                    break;
            }
        }
    }

    public struct TweekField
    {
        public Type fieldType;
        public string fieldName;
        public dynamic fieldValue;
        public FieldUsage fieldUsage;

        public TweekField(Type _fieldType, string _fieldName, FieldUsage _fieldUsage, dynamic _fieldValue = null)
        {
            fieldType = _fieldType;
            fieldName = _fieldName;
            fieldValue = _fieldValue;
            fieldUsage = _fieldUsage;
        }

        public TweekField(Type _fieldType, string _fieldName, dynamic _fieldValue = null, FieldUsage _fieldUsage = FieldUsage.Default)
        {
            fieldType = _fieldType;
            fieldName = _fieldName;
            fieldValue = _fieldValue;
            fieldUsage = _fieldUsage;
        }
    }

    public struct RequieredDictionaries
    {
        public Dictionary<string, List<TweekObj>> sceneObjs;
        public Dictionary<string, List<TweekObj>> prefabObjs;

        public RequieredDictionaries(Dictionary<string, List<TweekObj>> _sceneObjs, Dictionary<string, List<TweekObj>> _prefabObjs)
        {
            sceneObjs = _sceneObjs;
            prefabObjs = _prefabObjs;
        }
    }

    /*public struct AttributeConstructor
    {
        public TweekCoreUtilities.SupportedAttributes attribute;
        public dynamic arg_1;
        public dynamic arg_2;
        public dynamic arg_3;

        public AttributeConstructor(TweekCoreUtilities.SupportedAttributes _attribute, dynamic _arg_1 = null, dynamic _arg_2 = null, dynamic _arg_3 = null)
        {
            attribute = _attribute;
            arg_1 = _arg_1;
            arg_2 = _arg_2;
            arg_3 = _arg_3;
        }
    }*/

    public struct AttributeBuilder
    {
        public TweekCore.SupportedAttributes attribute;
        public dynamic arg_1;
        public dynamic arg_2;

        public AttributeBuilder(TweekCore.SupportedAttributes _attribute, dynamic _arg_1 = null, dynamic _arg_2 = null)
        {
            attribute = _attribute;
            arg_1 = _arg_1;
            arg_2 = _arg_2;
        }
    }
}
