using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Interactive)), CanEditMultipleObjects]
public class PropertyInteractive : Editor
{
    /*
     * ClipLink_Prop.isExpanded = true; EditorGUILayout.PropertyField (ClipLink_Prop, true);
     */

    public SerializedProperty
        state_Prop,
        SingleUse_Prop,
        CanUnUse_Prop,
        HideColliderAfterUse_Prop,
        DisattachParentAfterUse_Prop,
        IsUsedFromStart_Prop,
        Anim_Prop,
        ResetAnimClip_Prop,
        AudioSrc_Prop,
        SuccessAudio_Prop,
        Audio_Prop,
        CloseAudio_Prop,
        FailedAudio_Prop,
        FailedAudioSay_Prop,
        RequierePlace_Prop,
        RequiereItem_Prop,
        ShowObjAfterUseItem_Prop,
        HideObjAfterUseItem_Prop,
        CantTakeBeforeUse_Prop,
        CantUseBeforeUnUse_Prop,
        PlayAfterItemUse_Prop,
        MessageOnWrongUse_Prop,
        InfoMessage_Prop,
        MakeNoise_Prop,
        MakeNoiseOnWrong_Prop,
        Letter_Prop,
        ActiveObj_Prop,
        RequiereHideObj_Prop,
        RequiereUsed_Prop,
        RequiereUnUsed_Prop,
        DestroyParts_Prop,
        VelocityParts_Prop,
        SendUseAfterUse_Prop,
        PlayerReplic_Prop,
        TriggerForPlayerReplic_Prop,
        CheckByManiac_Prop,
        ManiacReplic_Prop,
        ManiacTimeline_Prop,
        CanUseWhileChasing_Prop,
        ChangeToMaterial_Prop,
        TargetForMaterial_Prop,
        CountToReleaseTrap_Prop,
        SpeedOfCloseTrap_Prop,
        SoundForTryReleaseTrap_Prop,
        SoundForSayPlayerTrap_Prop,
        SpeedAnimToReleaseTrap_Prop,
        NewEvent_Prop,
        CanUseWhileManiacCheck_Prop,
        Interraptable_Prop,
        ChangeCheckInOtherRoom_Prop,
        ManiacReplicInOtherRoom_Prop,
        ManiacTimelineInOtherRoom_Prop,
        OneTimeCheck_Prop;

    void OnEnable ()
    {
        // Setup the SerializedProperties
        state_Prop = serializedObject.FindProperty ("MethodUse");
        SingleUse_Prop = serializedObject.FindProperty ("SingleUse");
        CanUnUse_Prop = serializedObject.FindProperty ("CanUnUse");
        HideColliderAfterUse_Prop = serializedObject.FindProperty ("HideColliderAfterUse");
        DisattachParentAfterUse_Prop = serializedObject.FindProperty ("DisattachParentAfterUse");
        IsUsedFromStart_Prop = serializedObject.FindProperty ("IsUsedFromStart");
        Anim_Prop = serializedObject.FindProperty ("Anim");
        ResetAnimClip_Prop = serializedObject.FindProperty ("ResetAnimClip");
        AudioSrc_Prop = serializedObject.FindProperty ("AudioSrc");
        SuccessAudio_Prop = serializedObject.FindProperty ("SuccessAudio");
        Audio_Prop = serializedObject.FindProperty ("Audio");
        CloseAudio_Prop = serializedObject.FindProperty ("CloseAudio");
        FailedAudio_Prop = serializedObject.FindProperty ("FailedAudio");
        FailedAudioSay_Prop = serializedObject.FindProperty ("FailedAudioSay");
        RequierePlace_Prop = serializedObject.FindProperty ("RequierePlace");
        RequiereItem_Prop = serializedObject.FindProperty ("RequiereItem");
        ShowObjAfterUseItem_Prop = serializedObject.FindProperty ("ShowObjAfterUseItem");
        HideObjAfterUseItem_Prop = serializedObject.FindProperty ("HideObjAfterUseItem");
        CantTakeBeforeUse_Prop = serializedObject.FindProperty ("CantUseBeforeUse");
        CantUseBeforeUnUse_Prop = serializedObject.FindProperty ("CantUseBeforeUnUse");
        PlayAfterItemUse_Prop = serializedObject.FindProperty ("PlayAfterItemUse");
        MessageOnWrongUse_Prop = serializedObject.FindProperty ("MessageOnWrongUse");
        InfoMessage_Prop = serializedObject.FindProperty ("InfoMessage");
        MakeNoise_Prop = serializedObject.FindProperty ("MakeNoise");
        MakeNoiseOnWrong_Prop = serializedObject.FindProperty ("MakeNoiseOnWrong");
        Letter_Prop = serializedObject.FindProperty ("Letter");
        ActiveObj_Prop = serializedObject.FindProperty ("ActiveObj");
        RequiereHideObj_Prop = serializedObject.FindProperty ("RequiereHideObj");
        RequiereUsed_Prop = serializedObject.FindProperty ("RequiereUsed");
        RequiereUnUsed_Prop = serializedObject.FindProperty ("RequiereUnUsed");
        DestroyParts_Prop = serializedObject.FindProperty ("DestroyParts");
        VelocityParts_Prop = serializedObject.FindProperty ("VelocityParts");
        SendUseAfterUse_Prop = serializedObject.FindProperty ("SendUseAfterUse");
        PlayerReplic_Prop = serializedObject.FindProperty ("PlayerReplic");
        TriggerForPlayerReplic_Prop = serializedObject.FindProperty ("TriggerForPlayerReplic");
        CheckByManiac_Prop = serializedObject.FindProperty ("CheckByManiac");
        ManiacReplic_Prop = serializedObject.FindProperty ("ManiacReplic");
        ManiacTimeline_Prop = serializedObject.FindProperty ("ManiacTimeline");
        CanUseWhileChasing_Prop = serializedObject.FindProperty ("CanUseWhileChasing");
        ChangeToMaterial_Prop = serializedObject.FindProperty ("ChangeToMaterial");
        TargetForMaterial_Prop = serializedObject.FindProperty ("TargetForMaterial");
        CountToReleaseTrap_Prop = serializedObject.FindProperty ("CountToReleaseTrap");
        SpeedOfCloseTrap_Prop = serializedObject.FindProperty ("SpeedOfCloseTrap");
        SoundForTryReleaseTrap_Prop = serializedObject.FindProperty ("SoundForTryReleaseTrap");
        SoundForSayPlayerTrap_Prop = serializedObject.FindProperty ("SoundForSayPlayerTrap");
        SpeedAnimToReleaseTrap_Prop = serializedObject.FindProperty ("SpeedAnimToReleaseTrap");
        NewEvent_Prop = serializedObject.FindProperty ("NewEvent");
        CanUseWhileManiacCheck_Prop = serializedObject.FindProperty ("CanUseWhileManiacCheck");
        Interraptable_Prop = serializedObject.FindProperty ("Interraptable");
        ChangeCheckInOtherRoom_Prop = serializedObject.FindProperty ("ChangeCheckInOtherRoom");
        ManiacReplicInOtherRoom_Prop = serializedObject.FindProperty ("ManiacReplicInOtherRoom");
        ManiacTimelineInOtherRoom_Prop = serializedObject.FindProperty ("ManiacTimelineInOtherRoom");
        OneTimeCheck_Prop = serializedObject.FindProperty ("OneTimeCheck");
    }

    public override void OnInspectorGUI ()
    {
        serializedObject.Update ();

        EditorGUILayout.PropertyField (state_Prop);
        EditorGUILayout.PropertyField (SingleUse_Prop);
        EditorGUILayout.PropertyField (DisattachParentAfterUse_Prop);
        EditorGUILayout.PropertyField (IsUsedFromStart_Prop);
        EditorGUILayout.PropertyField (Anim_Prop);
        EditorGUILayout.PropertyField (ResetAnimClip_Prop);
        EditorGUILayout.PropertyField (AudioSrc_Prop);
        EditorGUILayout.PropertyField (SuccessAudio_Prop);
        EditorGUILayout.PropertyField (Audio_Prop);
        EditorGUILayout.PropertyField (CloseAudio_Prop);
        EditorGUILayout.PropertyField (FailedAudio_Prop);
        EditorGUILayout.PropertyField (FailedAudioSay_Prop);
        EditorGUILayout.PropertyField (RequierePlace_Prop);
        EditorGUILayout.PropertyField (RequiereItem_Prop);
        EditorGUILayout.PropertyField (ShowObjAfterUseItem_Prop);
        EditorGUILayout.PropertyField (HideObjAfterUseItem_Prop);
        EditorGUILayout.PropertyField (PlayAfterItemUse_Prop);
        EditorGUILayout.PropertyField (MakeNoise_Prop);
        EditorGUILayout.PropertyField (MakeNoiseOnWrong_Prop);
        EditorGUILayout.PropertyField (MessageOnWrongUse_Prop);
        EditorGUILayout.PropertyField (RequiereHideObj_Prop);
        EditorGUILayout.PropertyField (RequiereUsed_Prop);
        EditorGUILayout.PropertyField (RequiereUnUsed_Prop);
        EditorGUILayout.PropertyField (CantTakeBeforeUse_Prop);
        EditorGUILayout.PropertyField (CantUseBeforeUnUse_Prop);
        EditorGUILayout.PropertyField (SendUseAfterUse_Prop);
        EditorGUILayout.PropertyField (PlayerReplic_Prop);
        EditorGUILayout.PropertyField (TriggerForPlayerReplic_Prop);
        EditorGUILayout.PropertyField (CheckByManiac_Prop);
        EditorGUILayout.PropertyField (CanUseWhileChasing_Prop);
        EditorGUILayout.PropertyField (CanUseWhileManiacCheck_Prop);
        EditorGUILayout.PropertyField (Interraptable_Prop);
        EditorGUILayout.PropertyField (OneTimeCheck_Prop);

        Interactive.MethodUseEnum st = (Interactive.MethodUseEnum)state_Prop.enumValueIndex;

        switch (st) {
            case Interactive.MethodUseEnum.PlayAnim:
                break;

            case Interactive.MethodUseEnum.Trap:
                EditorGUILayout.PropertyField (CountToReleaseTrap_Prop);
                EditorGUILayout.PropertyField (SpeedOfCloseTrap_Prop);
                EditorGUILayout.PropertyField (SoundForTryReleaseTrap_Prop);
                EditorGUILayout.PropertyField (SoundForSayPlayerTrap_Prop);
                EditorGUILayout.PropertyField (SpeedAnimToReleaseTrap_Prop);
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.ShowLetter:
                EditorGUILayout.PropertyField (Letter_Prop);
                break;

            case Interactive.MethodUseEnum.PlayAnimAndActiveObj:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.PlayAnimAndDisactiveObj:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.ShowMessage:
                EditorGUILayout.PropertyField (InfoMessage_Prop);
                break;

            case Interactive.MethodUseEnum.DropItem:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.Item:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.DisactiveObj:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.ShowMessageAndSwitchObj:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                EditorGUILayout.PropertyField (InfoMessage_Prop);
                break;

            case Interactive.MethodUseEnum.ActiveObj:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.SwitchObj:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.PlayAnimAndActivePhisics:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.ActivePhysics:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.PlayAnimAndActiveEmission:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.PlayAnimAndDisactiveParticle:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                break;

            case Interactive.MethodUseEnum.Destruction:
                DestroyParts_Prop.isExpanded = true; 
                EditorGUILayout.PropertyField (DestroyParts_Prop, true);
                EditorGUILayout.PropertyField (VelocityParts_Prop);
                break;

            case Interactive.MethodUseEnum.ActiveObjAndChangeMaterial:
                EditorGUILayout.PropertyField (ActiveObj_Prop);
                EditorGUILayout.PropertyField (ChangeToMaterial_Prop);
                EditorGUILayout.PropertyField (TargetForMaterial_Prop);
                break;

            case Interactive.MethodUseEnum.UnityEvent:
                EditorGUILayout.PropertyField (NewEvent_Prop);
                break;
        }

        if (SingleUse_Prop.boolValue) {
            EditorGUILayout.PropertyField (HideColliderAfterUse_Prop);
        }
        else {
            EditorGUILayout.PropertyField (CanUnUse_Prop);
        }
        if (CheckByManiac_Prop.boolValue) {
            EditorGUILayout.PropertyField (ManiacReplic_Prop);
            EditorGUILayout.PropertyField (ManiacTimeline_Prop);
            EditorGUILayout.PropertyField (ChangeCheckInOtherRoom_Prop);
            if (ChangeCheckInOtherRoom_Prop.boolValue) {
                EditorGUILayout.PropertyField (ManiacReplicInOtherRoom_Prop);
                EditorGUILayout.PropertyField (ManiacTimelineInOtherRoom_Prop);
            }
        }

        serializedObject.ApplyModifiedProperties ();
    }
}
