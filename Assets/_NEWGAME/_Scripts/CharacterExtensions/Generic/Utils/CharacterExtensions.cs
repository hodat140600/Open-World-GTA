using _GAME._Scripts._CharacterController;
using _GAME._Scripts._CharacterController._Actions;
using UnityEngine;

namespace _GAME._Scripts
{
    public static class CharacterExtensions
    {

        /// <summary>
        /// Load all <see cref="_CharacterController._Actions.IActionController"/> and derivatives  in character gameObject to register to events <see cref="_CharacterController.Character.onActionEnter"/>,<see cref="_CharacterController.Character.onActionStay"/> and <see cref="_CharacterController.Character.onActionExit"/>.
        /// </summary>
        /// <param name="character">Target <seealso cref="_CharacterController.Character>"/></param>
        public static void LoadActionControllers(this Character character, bool debug = false)
        {
            var actionControllers = character.GetComponents<IActionController>();
            for (int i = 0; i < actionControllers.Length; i++)
            {
                if (actionControllers[i].enabled)
                {
                    if (actionControllers[i] is IActionListener)
                    {
                        var actionListener = actionControllers[i] as IActionListener;

                        {
                            if (actionListener.actionEnter)
                            {
                                character.onActionEnter.RemoveListener(actionListener.OnActionEnter);
                                character.onActionEnter.AddListener(actionListener.OnActionEnter);
                                if (debug) Debug.Log("Register Action Enter event to the " + actionListener.GetType().Name);
                            }

                            if (actionListener.actionStay)
                            {
                                character.onActionStay.RemoveListener(actionListener.OnActionStay);
                                character.onActionStay.AddListener(actionListener.OnActionStay);
                                if (debug) Debug.Log("Register Action Stay event to the " + actionListener.GetType().Name);
                            }

                            if (actionListener.actionExit)
                            {
                                character.onActionExit.RemoveListener(actionListener.OnActionExit);
                                character.onActionExit.AddListener(actionListener.OnActionExit);
                                if (debug) Debug.Log("Register action Exit event to the " + actionListener.GetType().Name);
                            }

                        }
                    }
                    else
                    {
                        if (actionControllers[i] is IActionEnterListener)
                        {
                            character.onActionEnter.RemoveListener((actionControllers[i] as IActionEnterListener).OnActionEnter);
                            character.onActionEnter.AddListener((actionControllers[i] as IActionEnterListener).OnActionEnter);
                            if (debug) Debug.Log("Register Action Enter event to the " + actionControllers[i].GetType().Name);
                        }

                        if (actionControllers[i] is IActionStayListener)
                        {
                            character.onActionStay.RemoveListener((actionControllers[i] as IActionStayListener).OnActionStay);
                            character.onActionStay.AddListener((actionControllers[i] as IActionStayListener).OnActionStay);
                            if (debug) Debug.Log("Register Action Stay event to the " + actionControllers[i].GetType().Name);
                        }

                        if (actionControllers[i] is IActionExitListener)
                        {
                            character.onActionExit.RemoveListener((actionControllers[i] as IActionExitListener).OnActionExit);
                            character.onActionExit.AddListener((actionControllers[i] as IActionExitListener).OnActionExit);
                            if (debug) Debug.Log("Register Action Exit event to the " + actionControllers[i].GetType().Name);
                        }
                    }
                }
            }
        }
    }
}