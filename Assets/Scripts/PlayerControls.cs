//------------------------------------------------------------------------------
// Generated-style Input System wrapper for Player.inputactions
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }

    public PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""6a946150-d160-4cf2-b518-045389fa083c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""1012a8fd-490e-4b7b-a72c-2210775be994"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AttackMode"",
                    ""type"": ""Button"",
                    ""id"": ""b796fefa-a9b7-4055-971f-dc0a494a7120"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PlatformMode"",
                    ""type"": ""Button"",
                    ""id"": ""c1a8e2f0-3d4b-4f6a-9e1c-2b7d8a4f5e60"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""JumpMode"",
                    ""type"": ""Button"",
                    ""id"": ""d2b9f3e1-4e5c-5a7b-0f2d-3c8e9b5a6f71"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a1b2c3d4-e5f6-7890-abcd-ef1234567890"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a33327f-dd01-4550-bbf1-fdb8e1785199"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AttackMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a223614b-30ff-44eb-b713-c3fd3f1dae73"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PlatformMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1345966c-5db4-46f5-9a5e-35fd912397d9"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JumpMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_AttackMode = m_Gameplay.FindAction("AttackMode", throwIfNotFound: true);
        m_Gameplay_PlatformMode = m_Gameplay.FindAction("PlatformMode", throwIfNotFound: true);
        m_Gameplay_JumpMode = m_Gameplay.FindAction("JumpMode", throwIfNotFound: true);
    }

    ~PlayerControls()
    {
        UnityEngine.Debug.Assert(!m_Gameplay.enabled, "This will cause a leak and performance issues, PlayerControls.Gameplay.Disable() has not been called.");
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_AttackMode;
    private readonly InputAction m_Gameplay_PlatformMode;
    private readonly InputAction m_Gameplay_JumpMode;

    public struct GameplayActions
    {
        private PlayerControls m_Wrapper;

        public GameplayActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction Move => m_Wrapper.m_Gameplay_Move;
        public InputAction AttackMode => m_Wrapper.m_Gameplay_AttackMode;
        public InputAction PlatformMode => m_Wrapper.m_Gameplay_PlatformMode;
        public InputAction JumpMode => m_Wrapper.m_Gameplay_JumpMode;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }

        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            Move.started += instance.OnMove;
            Move.performed += instance.OnMove;
            Move.canceled += instance.OnMove;
            AttackMode.started += instance.OnAttackMode;
            AttackMode.performed += instance.OnAttackMode;
            AttackMode.canceled += instance.OnAttackMode;
            PlatformMode.started += instance.OnPlatformMode;
            PlatformMode.performed += instance.OnPlatformMode;
            PlatformMode.canceled += instance.OnPlatformMode;
            JumpMode.started += instance.OnJumpMode;
            JumpMode.performed += instance.OnJumpMode;
            JumpMode.canceled += instance.OnJumpMode;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            Move.started -= instance.OnMove;
            Move.performed -= instance.OnMove;
            Move.canceled -= instance.OnMove;
            AttackMode.started -= instance.OnAttackMode;
            AttackMode.performed -= instance.OnAttackMode;
            AttackMode.canceled -= instance.OnAttackMode;
            PlatformMode.started -= instance.OnPlatformMode;
            PlatformMode.performed -= instance.OnPlatformMode;
            PlatformMode.canceled -= instance.OnPlatformMode;
            JumpMode.started -= instance.OnJumpMode;
            JumpMode.performed -= instance.OnJumpMode;
            JumpMode.canceled -= instance.OnJumpMode;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }

    public GameplayActions Gameplay => new GameplayActions(this);

    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAttackMode(InputAction.CallbackContext context);
        void OnPlatformMode(InputAction.CallbackContext context);
        void OnJumpMode(InputAction.CallbackContext context);
    }
}
