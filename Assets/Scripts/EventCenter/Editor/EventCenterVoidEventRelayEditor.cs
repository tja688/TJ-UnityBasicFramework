using System;
using System.Collections.Generic;
using System.Reflection;
using FlyRabbit.EventCenter.EditorSupport;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlyRabbit.EventCenter
{
    [CustomEditor(typeof(EventCenterVoidEventRelay))]
    public sealed class EventCenterVoidEventRelayEditor : Editor
    {
        private SerializedProperty m_EventNameProp;
        private SerializedProperty m_ListenOnEnableProp;
        private SerializedProperty m_OnEventProp;
        private SerializedProperty m_CachedParamCountProp;

        private DropdownField m_EventDropdown;
        private HelpBox m_ObsoleteHelp;
        private HelpBox m_SignatureHelp;
        private Label m_StatusLabel;
        private Label m_SignatureLabel;
        private PropertyField m_ListenField;

        private static readonly string ObsoletePrefix = "(Obsolete) ";
        private static readonly string CurrentObsoletePrefix = "(Current Obsolete) ";

        public override VisualElement CreateInspectorGUI()
        {
            m_EventNameProp = serializedObject.FindProperty("m_EventName");
            m_ListenOnEnableProp = serializedObject.FindProperty("m_ListenOnEnable");
            m_OnEventProp = serializedObject.FindProperty("m_OnEvent");
            m_CachedParamCountProp = serializedObject.FindProperty("m_CachedParamCount");

            var root = new VisualElement();
            root.style.paddingLeft = 8;
            root.style.paddingRight = 8;
            root.style.paddingTop = 6;
            root.style.paddingBottom = 6;

            var title = new Label("EventCenter Void Event Relay");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.marginBottom = 6;
            root.Add(title);

            m_EventDropdown = new DropdownField("Event", BuildChoices(), GetCurrentChoiceLabel());
            m_EventDropdown.RegisterValueChangedCallback(evt => OnDropdownChanged(evt.newValue));
            root.Add(m_EventDropdown);

            m_ObsoleteHelp = new HelpBox(string.Empty, HelpBoxMessageType.Warning);
            m_ObsoleteHelp.style.display = DisplayStyle.None;
            m_ObsoleteHelp.style.marginTop = 6;
            root.Add(m_ObsoleteHelp);

            m_SignatureHelp = new HelpBox(string.Empty, HelpBoxMessageType.Error);
            m_SignatureHelp.style.display = DisplayStyle.None;
            m_SignatureHelp.style.marginTop = 6;
            root.Add(m_SignatureHelp);

            m_SignatureLabel = new Label();
            m_SignatureLabel.style.opacity = 0.85f;
            m_SignatureLabel.style.marginTop = 6;
            root.Add(m_SignatureLabel);

            m_StatusLabel = new Label();
            m_StatusLabel.style.opacity = 0.85f;
            m_StatusLabel.style.marginTop = 2;
            root.Add(m_StatusLabel);

            m_ListenField = new PropertyField(m_ListenOnEnableProp, "Listen On Enable");
            m_ListenField.style.marginTop = 8;
            root.Add(m_ListenField);

            var onEventField = new PropertyField(m_OnEventProp, "On Event");
            onEventField.style.marginTop = 6;
            root.Add(onEventField);

            root.TrackPropertyValue(m_ListenOnEnableProp, _ => RefreshStateUI());
            root.TrackPropertyValue(m_CachedParamCountProp, _ => RefreshStateUI());

            root.Bind(serializedObject);
            RefreshStateUI();

            return root;
        }

        private void OnDropdownChanged(string choice)
        {
            serializedObject.Update();

            var name = choice ?? string.Empty;
            if (name.StartsWith(CurrentObsoletePrefix, StringComparison.Ordinal))
            {
                name = name.Substring(CurrentObsoletePrefix.Length);
            }
            else if (name.StartsWith(ObsoletePrefix, StringComparison.Ordinal))
            {
                name = name.Substring(ObsoletePrefix.Length);
            }

            if (Enum.TryParse<EventName>(name, out var parsed))
            {
                m_EventNameProp.intValue = (int)parsed;
                m_CachedParamCountProp.intValue = EventNameDocCache.GetParamCount(parsed);
            }

            serializedObject.ApplyModifiedProperties();

            m_EventDropdown.choices = BuildChoices();
            m_EventDropdown.SetValueWithoutNotify(GetCurrentChoiceLabel());
            RefreshStateUI();
        }

        private List<string> BuildChoices()
        {
            var choices = new List<string>();

            var current = (EventName)m_EventNameProp.intValue;
            var currentIsObsolete = TryGetObsolete(current, out _);
            if (currentIsObsolete)
            {
                choices.Add(CurrentObsoletePrefix + current);
            }

            foreach (EventName value in Enum.GetValues(typeof(EventName)))
            {
                if (TryGetObsolete(value, out _))
                {
                    continue;
                }
                choices.Add(value.ToString());
            }

            return choices;
        }

        private string GetCurrentChoiceLabel()
        {
            var current = (EventName)m_EventNameProp.intValue;
            if (TryGetObsolete(current, out _))
            {
                return CurrentObsoletePrefix + current;
            }
            return current.ToString();
        }

        private void RefreshStateUI()
        {
            serializedObject.Update();

            var eventName = (EventName)m_EventNameProp.intValue;
            var docParamCount = EventNameDocCache.GetParamCount(eventName);
            if (m_CachedParamCountProp.intValue != docParamCount)
            {
                m_CachedParamCountProp.intValue = docParamCount;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            var paramCount = Mathf.Clamp(m_CachedParamCountProp.intValue, 0, 5);

            var supportsVoid = paramCount == 0;
            m_SignatureLabel.text = $"Signature: {paramCount} params  {(supportsVoid ? "(supported)" : "(NOT supported)")}  |  Mode: {(m_ListenOnEnableProp.boolValue ? "Listen OnEnable" : "Disabled")}";

            if (TryGetObsolete(eventName, out var obsoleteMessage))
            {
                m_ObsoleteHelp.text = string.IsNullOrWhiteSpace(obsoleteMessage)
                    ? "Selected event is obsolete."
                    : $"Selected event is obsolete: {obsoleteMessage}";
                m_ObsoleteHelp.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_ObsoleteHelp.style.display = DisplayStyle.None;
            }

            if (!supportsVoid)
            {
                m_SignatureHelp.text = "This relay supports only 0-parameter events. This selection is not allowed to subscribe.";
                m_SignatureHelp.style.display = DisplayStyle.Flex;

                if (m_ListenOnEnableProp.boolValue)
                {
                    m_ListenOnEnableProp.boolValue = false;
                    serializedObject.ApplyModifiedProperties();
                }
                m_ListenField.SetEnabled(false);
            }
            else
            {
                m_SignatureHelp.style.display = DisplayStyle.None;
                m_ListenField.SetEnabled(true);
            }

            var willListen = supportsVoid && m_ListenOnEnableProp.boolValue && ((EventCenterVoidEventRelay)target).isActiveAndEnabled;
            var play = EditorApplication.isPlaying ? "PlayMode" : "EditMode";
            m_StatusLabel.text = $"Status: {play}  |  Will listen: {(willListen ? "Yes" : "No")}  |  Subscribes exactly one event";
        }

        private static bool TryGetObsolete(EventName eventName, out string message)
        {
            message = null;
            var field = typeof(EventName).GetField(eventName.ToString(), BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                return false;
            }
            var attr = field.GetCustomAttribute<ObsoleteAttribute>();
            if (attr == null)
            {
                return false;
            }
            message = attr.Message;
            return true;
        }
    }
}
