using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Engine.InputDefs;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using GameEngine.Interfaces;
using GameEngine.Managers;

namespace GameEngine.Systems
{
    public class MouseSystem : IUpdateSystem
    {
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            List<MouseComponent> MouseComps = ComponentManager.Instance.GetComponentsFromEntities<MouseComponent>(entities);
            if (MouseComps.Count > 0)
            {
                for (int i = 0; i < MouseComps.Count; ++i)
                {
                    UpdateState(MouseComps[i]);
                    updateActionStates(MouseComps[i]);
                }
            }
        }

        public void UpdateState(MouseComponent mouseComp)
        {
            mouseComp.OldState = mouseComp.NewState;
            mouseComp.NewState = Mouse.GetState();
        }

        public void updateActionStates(MouseComponent mouseComp)
        {
            MouseState newState = mouseComp.NewState;
            MouseState oldState = mouseComp.OldState;
            // LeftButton
            UpdateMouseButton(mouseComp, newState.LeftButton, oldState.LeftButton, "LeftButton");
            // MiddleButton
            UpdateMouseButton(mouseComp, newState.MiddleButton, oldState.MiddleButton, "MiddleButton");
            // RightButton
            UpdateMouseButton(mouseComp, newState.RightButton, oldState.RightButton, "RightButton");
        }

        private void UpdateMouseButton(MouseComponent mouseComp, ButtonState newState, ButtonState oldState, string button)
        {
            if (newState == ButtonState.Pressed && oldState != ButtonState.Pressed)
            {
                mouseComp.actionStates[button] = BUTTON_STATE.PRESSED;
            }
            else if (newState == ButtonState.Pressed && oldState == ButtonState.Pressed)
            {
                mouseComp.actionStates[button] = BUTTON_STATE.HELD;
            }
            else if (newState != ButtonState.Pressed && oldState == ButtonState.Pressed)
            {
                mouseComp.actionStates[button] = BUTTON_STATE.RELEASED;
            }
            else
            {
                mouseComp.actionStates[button] = BUTTON_STATE.NOT_PRESSED;
            }
        }
    }

}
