using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Managers
{
    /// <summary>
    /// The InputManager handle input from keyboard and mouse, as well can handle input from four gamepads.
    /// Thread safe singleton without using locks
    /// See link: "http://csharpindepth.com/Articles/General/Singleton.aspx#nested-cctor"
    /// </summary>
    class InputManager
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static InputManager() { }
        private InputManager() { }


        readonly Dictionary<string, List<Keys>> _keyBoardActions = new Dictionary<string, List<Keys>>();
        readonly Dictionary<PlayerIndex, Dictionary<string, List<Buttons>>> gamePadActions =  new Dictionary<PlayerIndex, Dictionary<string, List<Buttons>>>();

        MouseState currentMouse = Mouse.GetState();
        MouseState previousMouse = Mouse.GetState();

        KeyboardState currentKeyBoard = Keyboard.GetState();
        KeyboardState previousKeyBoard = Keyboard.GetState();

        GamePadState[] currentGamePad = new GamePadState[4];
        GamePadState[] previousGamePad = new GamePadState[4];

        /// <summary>
        /// Returns the instance of the InputManager.
        /// </summary>
        public static InputManager Instance { get; } = new InputManager();

        /// <summary>
        /// Updates the keyboardStates, gamePadStates and mouseStates.
        /// </summary>
        public void update()
        {
            previousKeyBoard = currentKeyBoard;
            currentKeyBoard = Keyboard.GetState();

            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            

            for (int i = 0; i < currentGamePad.Length; ++i)
            {
                previousGamePad[i] = currentGamePad[i];
                currentGamePad[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        //
        //  MOUSE INPUT
        //

        /// <summary>
        /// Gets the mouse current position.
        /// </summary>
        /// <returns>Returns the current position of the mouse</returns>
        public Point GetMouseCurrentPosition()
        {
            return currentMouse.Position;
        }

        /// <summary>
        /// Gets the mouse previous position.
        /// </summary>
        /// <returns>Returns the previous position of the mouse</returns>
        public Point GetMousePreviousPosition()
        {
            return previousMouse.Position;
        }

        /// <summary>
        /// Returns the cumulative scroll wheel value since the game start
        /// </summary>
        /// <returns>Returns the cumulative scroll wheel value since the game start</returns>
        public int GetMouseWheelValue()
        {
            return currentMouse.ScrollWheelValue;
        }

        /// <summary>
        /// Checks if the left mouse button was pressed.
        /// </summary>
        /// <returns>Returns true if the left mouse button was clicked, otherwise false.</returns>
        public bool LeftMousePressed()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the left mouse button is held.
        /// </summary>
        /// <returns>Returns true if the left mouse button is held, otherwise false.</returns>
        public bool LeftMouseHeld()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the left mouse button was released.
        /// </summary>
        /// <returns>Returns true if the left mouse button was released, otherwise false.</returns>
        public bool LeftMouseReleased()
        {
            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the right mouse button was pressed.
        /// </summary>
        /// <returns>Returns true if the right mouse button was pressed, otherwise false.</returns>
        public bool RightMousePressed()
        {
            if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton != ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Checks if the right mouse button is held.
        /// </summary>
        /// <returns>Returns true if right mouse button is held, otherwise false.</returns>
        public bool RightMouseHeld()
        {
            if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Checks if the right mouse button was released.
        /// </summary>
        /// <returns>Returns true if right mouse button was released, otherwise false.</returns>
        public bool RightMouseReleased()
        {
            if (currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Checks if the middle mouse button was pressed.
        /// </summary>
        /// <returns>Returns true if middle mouse button was pressed.</returns>
        public bool MiddleMousePressed()
        {
            if (currentMouse.MiddleButton == ButtonState.Pressed && previousMouse.MiddleButton != ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Checks if middle mouse button is held.
        /// </summary>
        /// <returns>Returns true if middle mouse button is held.</returns>
        public bool MiddleMouseHeld()
        {
            if (currentMouse.MiddleButton == ButtonState.Pressed && previousMouse.MiddleButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Checks if the middle mouse button was released.
        /// </summary>
        /// <returns>Returns true if the middle mouse button was released.</returns>
        public bool MiddleMouseReleased()
        {            
            if (currentMouse.MiddleButton == ButtonState.Released && previousMouse.MiddleButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }


        //
        //  KEYBOARD INPUT
        //

        /// <summary>
        /// Registers a actions to a keyboard button.
        /// </summary>
        /// <param name="action">The name of the action.</param>
        /// <param name="key">The key that you want to bind to the action.</param>
        public void AddKeyBoardAction(string action, Keys key)
        {
            if (!_keyBoardActions.ContainsKey(action))
            {
                _keyBoardActions[action] = new List<Keys>();
            }
            _keyBoardActions[action].Add(key);
        }

        /// <summary>
        /// Removes a key bound to a action.
        /// </summary>
        /// <param name="action">The action you want to remove a key from.</param>
        /// <param name="key">They key you want to remove.</param>
        public void RemoveKeyBoardAction(string action, Keys key)
        {
            if(_keyBoardActions.ContainsKey(action))
            {
                _keyBoardActions[action].Remove(key);
            }
        }

        /// <summary>
        /// Keyboard
        /// Checks if any key bound to the action was pressed.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>Returns true if any key bound to the action was pressed, otherwise false.</returns>
        public bool KeyPressed(string action)
        {
            if (_keyBoardActions.ContainsKey(action))
            {
                foreach (Keys key in _keyBoardActions[action])
                {
                    if (currentKeyBoard.IsKeyDown(key) && !previousKeyBoard.IsKeyDown(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Keyboard
        /// Checks if any key bound to the action is held.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>Returns true if any key bound to the action is held, otherwise false.</returns>
        public bool KeyHeld(string action)
        {
            if(_keyBoardActions.ContainsKey(action))
            {
                foreach (Keys key in _keyBoardActions[action])
                {
                    if (currentKeyBoard.IsKeyDown(key) && previousKeyBoard.IsKeyDown(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Keyboard
        /// Checks if any key bound to the action was released.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>Returns true if any key bound to the actions was released, otherwise false.</returns>
        public bool KeyReleased(string action)
        {
            if (_keyBoardActions.ContainsKey(action))
            {
                foreach (Keys key in _keyBoardActions[action])
                {
                    if (previousKeyBoard.IsKeyDown(key) && currentKeyBoard.IsKeyUp(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //
        //  GAMEPAD INPUT
        //

        /// <summary>
        /// Adds an action to a gamepad button for the selected controller.
        /// </summary>
        /// <param name="playerIndex">The player index.</param>
        /// <param name="action">The name of the action.</param>
        /// <param name="button">The gamepad button you want to bind to the action.</param>
        public void AddGamePadAction(PlayerIndex playerIndex, string action, Buttons button)
        {
            if (!gamePadActions.ContainsKey(playerIndex))
            {
                gamePadActions[playerIndex] = new Dictionary<string, List<Buttons>>();
            }

            if (!gamePadActions[playerIndex].ContainsKey(action))
            {
                gamePadActions[playerIndex][action] = new List<Buttons>();
            }

            gamePadActions[playerIndex][action].Add(button);
        }

        /// <summary>
        /// Removes a button bound to an action for the selected controller. 
        /// </summary>
        /// <param name="playerIndex">The player index</param>
        /// <param name="action">The action you want to remove a button from</param>
        /// <param name="button">The button you want to remove from the action</param>
        public void RemoveGamePadAction(PlayerIndex playerIndex, string action, Buttons button)
        {
            if (gamePadActions.ContainsKey(playerIndex))
            {
                if(gamePadActions[playerIndex].ContainsKey(action))
                {
                    gamePadActions[playerIndex][action].Remove(button);
                }
            }
        }

        /// <summary>
        /// Gamepad
        /// Check if the buttons bound to the action and playerIndex was pressed.
        /// 
        /// This functions throws an exception if the playerIndex don't have a controller
        /// connected to it.
        /// </summary>
        /// <param name="playerIndex">The index that selects what gamepad is being check</param>
        /// <param name="action">The action you want to check if it was pressed</param>
        /// <returns>True if button bound to action is pressed, otherwise false.</returns>
        public bool ButtonPressed(PlayerIndex playerIndex, string action)
        {
            if (currentGamePad[(int)playerIndex].IsConnected)
            {
                if (gamePadActions.ContainsKey(playerIndex))
                {
                    if (gamePadActions[playerIndex].ContainsKey(action))
                    {
                        foreach (Buttons button in gamePadActions[playerIndex][action])
                        {
                            if (currentGamePad[(int)playerIndex].IsButtonDown(button) && !previousGamePad[(int)playerIndex].IsButtonDown(button))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("GamePad for playerIndex " + playerIndex + ", is not connected");
            }
            return false;
        }

        /// <summary>
        /// Gamepad
        /// Checks if any button bound to the action and player index is held.
        /// 
        /// This functions throws an exception if the playerIndex don't have a controller
        /// connected to it.
        /// </summary>
        /// <param name="playerIndex">The index that selects what gamepad is being check</param>
        /// <param name="action">The action to check</param>
        /// <returns>Returns true if any button bound to the action is held, otherwise false.</returns>
        public bool ButtonHeld(PlayerIndex playerIndex, string action)
        {
            if (currentGamePad[(int)playerIndex].IsConnected)
            {
                if (gamePadActions.ContainsKey(playerIndex))
                {
                    if (gamePadActions[playerIndex].ContainsKey(action))
                    {
                        foreach (Buttons button in gamePadActions[playerIndex][action])
                        {
                            if (currentGamePad[(int)playerIndex].IsButtonDown(button) && previousGamePad[(int)playerIndex].IsButtonDown(button))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("GamePad for playerIndex " + playerIndex + ", is not connected");
            }
            return false;
        }


        /// <summary>
        /// Gamepad
        /// Checks if the action bound to the button and playerIndex was released.
        /// 
        /// This functions throws an exception if the playerIndex don't have a controller
        /// connected to it.
        /// </summary>
        /// <param name="playerIndex">The index that selects what gamepad is being check</param>
        /// <param name="action">The action to check</param>
        /// <returns>Returns true if any button bound to the action was released, otherwise false.</returns>
        public bool ButtonReleased(PlayerIndex playerIndex, string action)
        {
            if (currentGamePad[(int)playerIndex].IsConnected)
            {
                if (gamePadActions.ContainsKey(playerIndex))
                {
                    if (gamePadActions[playerIndex].ContainsKey(action))
                    {
                        foreach (Buttons button in gamePadActions[playerIndex][action])
                        {
                            if (previousGamePad[(int)playerIndex].IsButtonDown(button) && currentGamePad[(int)playerIndex].IsButtonUp(button))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("GamePad for playerIndex " + playerIndex + ", is not connected.");
            }
            return false;
        }

    }
}
