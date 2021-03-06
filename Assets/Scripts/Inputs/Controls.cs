// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""1c448a8b-8bba-4496-9b1a-d15c4ec0074b"",
            ""actions"": [
                {
                    ""name"": ""Move Camera"",
                    ""type"": ""Value"",
                    ""id"": ""04f78990-85f6-430f-bac9-f77138787f79"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset Camera"",
                    ""type"": ""Button"",
                    ""id"": ""3951c6f2-abc8-43e5-a433-f6f562181ffc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom Camera"",
                    ""type"": ""Value"",
                    ""id"": ""3ba64fe1-150b-4847-bd35-0c835514b268"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select All Units"",
                    ""type"": ""Button"",
                    ""id"": ""ad5c89b7-0d33-4627-acf3-549da73038c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Set Control Group"",
                    ""type"": ""Button"",
                    ""id"": ""7fbfe763-92ba-4d8e-8b48-80a48dd5e9fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""1ed7c4d6-50f2-4704-8871-eae34b97c025"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""33aa94ad-0db8-4542-b762-b70a9a332673"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""38e47ca5-7866-4d53-93c3-5b8e48248d4a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""69b1dae5-bdba-4294-8a3e-83d9b8ece712"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7a46d649-27bb-405d-9762-9d5726bbf0ee"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""97bb7bd6-adf7-41d6-b6f9-87246ced719c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d07285ea-b762-4016-a82f-bad968dfa267"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b6228c70-013c-431b-96a6-ba01f4af7f15"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8cca8c3c-10d2-476d-aa08-a397d9155091"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""707097c8-0edd-4138-b761-30a136f8609b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0e9cc58f-f358-45e7-9454-1e3da8d80500"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Reset Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f651dfa-c04f-4c13-8f32-4ab2a4c05b6e"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Zoom Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5df910d3-0c26-421e-b069-b23615a250a5"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Select All Units"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Ctrl + ..."",
                    ""id"": ""966274d3-2482-4797-a260-c8328aa43259"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Set Control Group"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""0df405c7-e460-47b8-a0a3-433c4d0dcef1"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Set Control Group"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""3080065d-5167-4320-b5c8-c4cdf9d7feea"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Set Control Group"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveCamera = m_Player.FindAction("Move Camera", throwIfNotFound: true);
        m_Player_ResetCamera = m_Player.FindAction("Reset Camera", throwIfNotFound: true);
        m_Player_ZoomCamera = m_Player.FindAction("Zoom Camera", throwIfNotFound: true);
        m_Player_SelectAllUnits = m_Player.FindAction("Select All Units", throwIfNotFound: true);
        m_Player_SetControlGroup = m_Player.FindAction("Set Control Group", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MoveCamera;
    private readonly InputAction m_Player_ResetCamera;
    private readonly InputAction m_Player_ZoomCamera;
    private readonly InputAction m_Player_SelectAllUnits;
    private readonly InputAction m_Player_SetControlGroup;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveCamera => m_Wrapper.m_Player_MoveCamera;
        public InputAction @ResetCamera => m_Wrapper.m_Player_ResetCamera;
        public InputAction @ZoomCamera => m_Wrapper.m_Player_ZoomCamera;
        public InputAction @SelectAllUnits => m_Wrapper.m_Player_SelectAllUnits;
        public InputAction @SetControlGroup => m_Wrapper.m_Player_SetControlGroup;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MoveCamera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @ResetCamera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetCamera;
                @ResetCamera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetCamera;
                @ResetCamera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetCamera;
                @ZoomCamera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoomCamera;
                @ZoomCamera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoomCamera;
                @ZoomCamera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoomCamera;
                @SelectAllUnits.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectAllUnits;
                @SelectAllUnits.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectAllUnits;
                @SelectAllUnits.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectAllUnits;
                @SetControlGroup.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetControlGroup;
                @SetControlGroup.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetControlGroup;
                @SetControlGroup.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetControlGroup;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveCamera.started += instance.OnMoveCamera;
                @MoveCamera.performed += instance.OnMoveCamera;
                @MoveCamera.canceled += instance.OnMoveCamera;
                @ResetCamera.started += instance.OnResetCamera;
                @ResetCamera.performed += instance.OnResetCamera;
                @ResetCamera.canceled += instance.OnResetCamera;
                @ZoomCamera.started += instance.OnZoomCamera;
                @ZoomCamera.performed += instance.OnZoomCamera;
                @ZoomCamera.canceled += instance.OnZoomCamera;
                @SelectAllUnits.started += instance.OnSelectAllUnits;
                @SelectAllUnits.performed += instance.OnSelectAllUnits;
                @SelectAllUnits.canceled += instance.OnSelectAllUnits;
                @SetControlGroup.started += instance.OnSetControlGroup;
                @SetControlGroup.performed += instance.OnSetControlGroup;
                @SetControlGroup.canceled += instance.OnSetControlGroup;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMoveCamera(InputAction.CallbackContext context);
        void OnResetCamera(InputAction.CallbackContext context);
        void OnZoomCamera(InputAction.CallbackContext context);
        void OnSelectAllUnits(InputAction.CallbackContext context);
        void OnSetControlGroup(InputAction.CallbackContext context);
    }
}
