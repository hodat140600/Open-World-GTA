using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using _GAME._Scripts._EventSystems;
using _GAME._Scripts;
using _GAME._Scripts._Utils;

public partial class MenuComponent
{
    public const string path = "DatHQ/Utils/";

    [MenuItem(path + "SimpleTrigger")]
    public static void AddSimpleTrigger()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<SimpleTrigger>();
    }

    [MenuItem(path + "AnimatorEventReceiver")]
    public static void AddAnimatorEventReceiver()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<AnimatorEventReceiver>();
    }

    [MenuItem(path + "MessageReceiver")]
    public static void AddMessageReceiver()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<MessageReceiver>();
    }

    [MenuItem(path + "MessageSender")]
    public static void AddMessageSender()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<MessageSender>();
    }

    [MenuItem(path + "EventWithDelay")]
    public static void AddEventWithDelay()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<EventWithDelay>();
    }

    [MenuItem(path + "DestroyGameObject")]
    public static void AddDestroyGameObject()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<ExtendDestroyGameObject>();
    }

    [MenuItem(path + "DestroyOnTrigger")]
    public static void AddDestroyOnTrigger()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<DestroyOnTrigger>();
    }

    [MenuItem(path + "PlayRandomClip")]
    public static void AddPlayRandomClip()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<PlayRandomClip>();
    }

    [MenuItem(path + "RotateObject")]
    public static void AddRotateObject()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<RotateObject>();
    }

    [MenuItem(path + "LookAtCamera")]
    public static void AddLookAtCamera()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<LookAtCamera>();
    }

    [MenuItem(path + "Instantiate")]
    public static void AddInstantiate()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<Instantiate>();
    }

    [MenuItem(path + "SetParent")]
    public static void AddSetParent()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<ExtendSetParent>();
    }

    [MenuItem(path + "ResetTransform")]
    public static void AddResetTransform()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<ResetTransform>();
    }

    [MenuItem(path + "DestroyChildrens")]
    public static void AddDestroyChildrens()
    {
        var currentObject = Selection.activeGameObject;
        if (currentObject)
            currentObject.AddComponent<ExtendDestroyChildrens>();
    }
}