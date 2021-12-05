// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Ground"",
            ""id"": ""584b6bda-1c9c-46c7-aec1-7bb9a42cf360"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""758dceb6-c8bf-4a0f-bb50-34283cc39021"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a57c0f82-7c3b-4f26-b082-4670befd6ac1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraPan"",
                    ""type"": ""Value"",
                    ""id"": ""df577260-1683-4f4d-9297-117d278003e3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""4f7e98e8-b462-4166-8bab-abb4aafc982c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""451f51aa-5e0d-49f6-9347-2cd56280eaa6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""254034c3-2593-49d3-93c2-aae127d64dcb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ff1c1342-b908-4a96-8d88-2823a0b31ff3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7f2c480a-f784-4a51-b08f-1ced2cb91875"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""26ab6a24-23e9-43eb-8d72-d161b317010d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1109f7bb-bb25-48b0-981b-c062e2a98579"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ee5e1e1-dd6d-43fd-8af1-1adb184c1d3f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ba93ea6-8968-4536-b20b-f56ffd4e2d63"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc5bce53-5f14-4784-a5c3-bcd7d2afc1d5"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d367778-0e80-4408-a133-e8e32dd14a83"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""CameraPan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc56b4c5-2bc8-4d81-bd52-69f7b875b1f7"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraPan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""762aae49-702b-4cc0-9644-cb2499fc4628"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77a558f2-55a2-4ff8-b4a9-94980fa4524e"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fa5adc4-9e17-4b5b-b3fb-a732790193fe"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Battle"",
            ""id"": ""e8cc955b-93e2-4d82-a328-2089ad9db79c"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Value"",
                    ""id"": ""7575b27f-3ac3-452e-bb75-375f79818b2c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackLight"",
                    ""type"": ""Button"",
                    ""id"": ""7b92e4fc-dd45-453a-8eb7-81aa4b6dd1fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackHard"",
                    ""type"": ""Button"",
                    ""id"": ""16cf2a1b-9ec0-4f4b-aeea-670ec63e134a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackSpecial"",
                    ""type"": ""Button"",
                    ""id"": ""ab9275f7-b9f0-45fa-b0aa-f49232a34afd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0ffdbdeb-621e-4638-9107-4f437ea22ffa"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd69615a-7e4a-430c-8de4-163d48147256"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""21d957aa-123a-473a-84f4-67b6d55ea2ee"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""507df8a2-01bc-4643-9ac2-ad73d9c0562e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e1b7296a-1234-45de-aeeb-211cab55e12b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""86cb832f-a1f7-44d2-aeec-4e6126195a12"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9c957183-eb8d-40c1-b2ca-9bb9346287b6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0c58a486-ad38-4e6b-b188-7c0e0f282fc6"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""AttackLight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdd40b0f-94b3-4675-be93-ace125e2ccdd"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackLight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a6010ec-1fb7-4e07-918e-32143cf064a9"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackLight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6962890d-4b6c-420c-a79b-3dd0ee223a8c"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""AttackHard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""747d18a4-22df-495d-b62f-750866b091c6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackHard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e19bc7fc-1dd3-48a0-95c0-548f8ac0ec4f"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackHard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b704b03a-ec24-4e23-83c6-791410cfc2a7"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""AttackSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5223c176-5b5a-4435-b3f7-3e29dfa11de0"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17a0e45f-851c-41d3-ba5b-80e4579cca68"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Pc"",
                    ""action"": ""AttackSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Pc"",
            ""bindingGroup"": ""Pc"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Ground
        m_Ground = asset.FindActionMap("Ground", throwIfNotFound: true);
        m_Ground_Move = m_Ground.FindAction("Move", throwIfNotFound: true);
        m_Ground_Jump = m_Ground.FindAction("Jump", throwIfNotFound: true);
        m_Ground_CameraPan = m_Ground.FindAction("CameraPan", throwIfNotFound: true);
        m_Ground_Menu = m_Ground.FindAction("Menu", throwIfNotFound: true);
        // Battle
        m_Battle = asset.FindActionMap("Battle", throwIfNotFound: true);
        m_Battle_Select = m_Battle.FindAction("Select", throwIfNotFound: true);
        m_Battle_AttackLight = m_Battle.FindAction("AttackLight", throwIfNotFound: true);
        m_Battle_AttackHard = m_Battle.FindAction("AttackHard", throwIfNotFound: true);
        m_Battle_AttackSpecial = m_Battle.FindAction("AttackSpecial", throwIfNotFound: true);
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

    // Ground
    private readonly InputActionMap m_Ground;
    private IGroundActions m_GroundActionsCallbackInterface;
    private readonly InputAction m_Ground_Move;
    private readonly InputAction m_Ground_Jump;
    private readonly InputAction m_Ground_CameraPan;
    private readonly InputAction m_Ground_Menu;
    public struct GroundActions
    {
        private @PlayerControls m_Wrapper;
        public GroundActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Ground_Move;
        public InputAction @Jump => m_Wrapper.m_Ground_Jump;
        public InputAction @CameraPan => m_Wrapper.m_Ground_CameraPan;
        public InputAction @Menu => m_Wrapper.m_Ground_Menu;
        public InputActionMap Get() { return m_Wrapper.m_Ground; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GroundActions set) { return set.Get(); }
        public void SetCallbacks(IGroundActions instance)
        {
            if (m_Wrapper.m_GroundActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnJump;
                @CameraPan.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnCameraPan;
                @CameraPan.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnCameraPan;
                @CameraPan.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnCameraPan;
                @Menu.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnMenu;
            }
            m_Wrapper.m_GroundActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @CameraPan.started += instance.OnCameraPan;
                @CameraPan.performed += instance.OnCameraPan;
                @CameraPan.canceled += instance.OnCameraPan;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
            }
        }
    }
    public GroundActions @Ground => new GroundActions(this);

    // Battle
    private readonly InputActionMap m_Battle;
    private IBattleActions m_BattleActionsCallbackInterface;
    private readonly InputAction m_Battle_Select;
    private readonly InputAction m_Battle_AttackLight;
    private readonly InputAction m_Battle_AttackHard;
    private readonly InputAction m_Battle_AttackSpecial;
    public struct BattleActions
    {
        private @PlayerControls m_Wrapper;
        public BattleActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Battle_Select;
        public InputAction @AttackLight => m_Wrapper.m_Battle_AttackLight;
        public InputAction @AttackHard => m_Wrapper.m_Battle_AttackHard;
        public InputAction @AttackSpecial => m_Wrapper.m_Battle_AttackSpecial;
        public InputActionMap Get() { return m_Wrapper.m_Battle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleActions set) { return set.Get(); }
        public void SetCallbacks(IBattleActions instance)
        {
            if (m_Wrapper.m_BattleActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnSelect;
                @AttackLight.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackLight;
                @AttackLight.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackLight;
                @AttackLight.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackLight;
                @AttackHard.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackHard;
                @AttackHard.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackHard;
                @AttackHard.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackHard;
                @AttackSpecial.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackSpecial;
                @AttackSpecial.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackSpecial;
                @AttackSpecial.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnAttackSpecial;
            }
            m_Wrapper.m_BattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @AttackLight.started += instance.OnAttackLight;
                @AttackLight.performed += instance.OnAttackLight;
                @AttackLight.canceled += instance.OnAttackLight;
                @AttackHard.started += instance.OnAttackHard;
                @AttackHard.performed += instance.OnAttackHard;
                @AttackHard.canceled += instance.OnAttackHard;
                @AttackSpecial.started += instance.OnAttackSpecial;
                @AttackSpecial.performed += instance.OnAttackSpecial;
                @AttackSpecial.canceled += instance.OnAttackSpecial;
            }
        }
    }
    public BattleActions @Battle => new BattleActions(this);
    private int m_PcSchemeIndex = -1;
    public InputControlScheme PcScheme
    {
        get
        {
            if (m_PcSchemeIndex == -1) m_PcSchemeIndex = asset.FindControlSchemeIndex("Pc");
            return asset.controlSchemes[m_PcSchemeIndex];
        }
    }
    public interface IGroundActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCameraPan(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
    public interface IBattleActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnAttackLight(InputAction.CallbackContext context);
        void OnAttackHard(InputAction.CallbackContext context);
        void OnAttackSpecial(InputAction.CallbackContext context);
    }
}
