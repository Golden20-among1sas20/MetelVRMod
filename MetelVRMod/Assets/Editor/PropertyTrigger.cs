using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (TriggerSpace)), CanEditMultipleObjects]
public class PropertyTrigger : Editor
{

    public SerializedProperty
        state_Prop,
        SingleUse_Prop,
        ForTag_Prop,
        Clip_Prop,
        ActiveObj_Prop,
        NewEvent_Prop,
        DelayBetweenDamage_Prop,
        TrapTarget_Prop,
        TimelineForPlay_Prop,
        ForPowerUse_Prop;

    void OnEnable ()
    {
        // Setup the SerializedProperties
        state_Prop = serializedObject.FindProperty ("MethodTrigger");
        SingleUse_Prop = serializedObject.FindProperty ("SingleUse");
        ForTag_Prop = serializedObject.FindProperty ("ForTag");
        Clip_Prop = serializedObject.FindProperty ("Clip");
        ActiveObj_Prop = serializedObject.FindProperty ("ActiveObj");
        NewEvent_Prop = serializedObject.FindProperty ("NewEvent");
        DelayBetweenDamage_Prop = serializedObject.FindProperty ("DelayBetweenDamage");
        TrapTarget_Prop = serializedObject.FindProperty ("TrapTarget");
        TimelineForPlay_Prop = serializedObject.FindProperty ("TimelineForPlay");
        ForPowerUse_Prop = serializedObject.FindProperty ("ForPowerUse");
    }

    public override void OnInspectorGUI ()
    {
        serializedObject.Update ();

        EditorGUILayout.PropertyField (state_Prop);
        EditorGUILayout.PropertyField (SingleUse_Prop);
        EditorGUILayout.PropertyField (ForTag_Prop);

        TriggerSpace.MethodTriggerEnum st = (TriggerSpace.MethodTriggerEnum)state_Prop.enumValueIndex;

        switch (st) {
            case TriggerSpace.MethodTriggerEnum.PlayerSay:
            case TriggerSpace.MethodTriggerEnum.ManiacSay:
                EditorGUILayout.PropertyField (Clip_Prop);
                break;
            case TriggerSpace.MethodTriggerEnum.ActiveObj:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;
            case TriggerSpace.MethodTriggerEnum.UnityEvent:
                EditorGUILayout.PropertyField (NewEvent_Prop);
                break;
            case TriggerSpace.MethodTriggerEnum.Damage:
                EditorGUILayout.PropertyField (DelayBetweenDamage_Prop);
                break;
            case TriggerSpace.MethodTriggerEnum.Trap:
                EditorGUILayout.PropertyField (TrapTarget_Prop);
                break;
            case TriggerSpace.MethodTriggerEnum.StartTimeline:
                EditorGUILayout.PropertyField (TimelineForPlay_Prop);
                break;
            case TriggerSpace.MethodTriggerEnum.PowerUseIfNotUsedInChasing:
                EditorGUILayout.PropertyField (ForPowerUse_Prop);
                break;

        }


        serializedObject.ApplyModifiedProperties ();
    }
}
