// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/PlatformMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlatformMap : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlatformMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlatformMap"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""cabdcc15-5943-4c4b-8696-aecc32c10a9f"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""9b560bd6-e48a-4c02-b9ec-cc25f8ed5c59"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseCamera"",
                    ""type"": ""Value"",
                    ""id"": ""13e37655-4b8f-4217-9aba-38fcdc869536"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PadCamera"",
                    ""type"": ""Value"",
                    ""id"": ""9d6dcfdd-340a-489a-940a-f299569c58c1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""9a8c6feb-9b75-428e-a70e-48f1e0067798"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Music"",
                    ""type"": ""Value"",
                    ""id"": ""94fe535c-4274-4234-85c7-1ae5cf7e2724"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""7d0757f4-787d-446e-9284-5fc3a44b4629"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Creep"",
                    ""type"": ""Button"",
                    ""id"": ""11428faf-3a01-4b30-9ad7-2d2f50d47d73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""02737394-178c-47f3-8a32-f5198b54bdeb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Binocucom"",
                    ""type"": ""Button"",
                    ""id"": ""1fb2aacd-8a6e-4fae-af58-4c742a373a8c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""3ab9365e-8314-47d7-b923-f273f3f7612f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BinocucomScroll"",
                    ""type"": ""Value"",
                    ""id"": ""40e75d90-1ac2-445b-9be7-76000bfc1fc0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BinocucomPad"",
                    ""type"": ""Value"",
                    ""id"": ""c4bc2e4a-03cd-40c7-94a7-197faa32ad79"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crawl"",
                    ""type"": ""Button"",
                    ""id"": ""6f84782d-645e-44e7-830f-de78fa538046"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""5c4f60e7-2035-4931-b3a7-6cfc9574c1a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""c4538ae2-55d3-4e7e-93ff-9402f8cdb65a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c22201cd-a9b7-49f5-9361-6c53bb8c8a7e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c558461c-7e39-4ea8-952c-11bd51497854"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f42a21c5-f43b-4990-858d-6f843d6db87d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""591ab0ee-8f37-4b72-829b-42bbec74ef95"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c71a7243-a499-44aa-9540-ee03e47b1cd0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79d5fe05-5d6d-454f-afa9-09d1732218f8"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""MouseCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f416d8a-fd59-4369-9623-1c7c1f34d4cc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8604a54-5976-4d4a-b47e-397a930822a7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""0caffec6-5153-414d-aac7-fddd9c57740f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Music"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""488be92d-5268-4cdd-b880-30b4e0569417"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Music"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8866e531-69a6-4e4b-947b-9d3050d8cd6f"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Music"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ce86c5d8-1e94-4b2a-9142-73997de543d6"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d7066a9-8f88-4e84-b07c-9a4f034c22e5"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""Creep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6f34c02-775c-4d73-9327-680d31f139e0"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b5d37d9-cf6a-45ac-b635-87db423b669a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df18c77f-678f-4554-ae33-f1aebe4c7e35"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Binocucom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9266d117-999d-4b32-ac5f-e4dc6091aa4a"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Binocucom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""105dbf89-c0c6-4e10-9d5b-80b3ffa290b8"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Binocucom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a500917-85de-415c-88a7-5a775e7d8082"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""775d9f07-d48c-492c-b1d7-4c51a60379d4"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""506d5e1b-bf99-4421-93aa-cd3622610872"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BinocucomScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a60a368-5c32-4ba3-ad91-b1f415c20e5a"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crawl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ce3fa3e-7270-4460-a3e5-a786a85be021"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f865791-c0b6-4668-a9f3-a9edd8ff0406"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65bf89a0-9a8b-4be1-a476-2d726fd81a3f"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Normal"",
                    ""action"": ""PadCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6bb02eb-9432-4a20-a0cf-45c55defbaec"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BinocucomScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Cutscene"",
            ""id"": ""6b09d1be-30e6-4bfe-ad5d-019a41aa2cc0"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""1e2dc4ae-5eb8-4a32-89f3-e19e56ccfe3d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5603ee06-4055-43b4-814b-244868436658"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19d56f86-0561-4809-b013-fa8ca0ff6a38"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88e6268a-01df-436d-8558-b20948581bed"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""955e2711-09bb-45dd-b668-8821b61176e1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Normal"",
            ""bindingGroup"": ""Normal"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_MouseCamera = m_Character.FindAction("MouseCamera", throwIfNotFound: true);
        m_Character_PadCamera = m_Character.FindAction("PadCamera", throwIfNotFound: true);
        m_Character_Jump = m_Character.FindAction("Jump", throwIfNotFound: true);
        m_Character_Music = m_Character.FindAction("Music", throwIfNotFound: true);
        m_Character_Exit = m_Character.FindAction("Exit", throwIfNotFound: true);
        m_Character_Creep = m_Character.FindAction("Creep", throwIfNotFound: true);
        m_Character_Interact = m_Character.FindAction("Interact", throwIfNotFound: true);
        m_Character_Binocucom = m_Character.FindAction("Binocucom", throwIfNotFound: true);
        m_Character_Attack = m_Character.FindAction("Attack", throwIfNotFound: true);
        m_Character_BinocucomScroll = m_Character.FindAction("BinocucomScroll", throwIfNotFound: true);
        m_Character_BinocucomPad = m_Character.FindAction("BinocucomPad", throwIfNotFound: true);
        m_Character_Crawl = m_Character.FindAction("Crawl", throwIfNotFound: true);
        m_Character_Sprint = m_Character.FindAction("Sprint", throwIfNotFound: true);
        // Cutscene
        m_Cutscene = asset.FindActionMap("Cutscene", throwIfNotFound: true);
        m_Cutscene_Interact = m_Cutscene.FindAction("Interact", throwIfNotFound: true);
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

    // Character
    private readonly InputActionMap m_Character;
    private ICharacterActions m_CharacterActionsCallbackInterface;
    private readonly InputAction m_Character_Movement;
    private readonly InputAction m_Character_MouseCamera;
    private readonly InputAction m_Character_PadCamera;
    private readonly InputAction m_Character_Jump;
    private readonly InputAction m_Character_Music;
    private readonly InputAction m_Character_Exit;
    private readonly InputAction m_Character_Creep;
    private readonly InputAction m_Character_Interact;
    private readonly InputAction m_Character_Binocucom;
    private readonly InputAction m_Character_Attack;
    private readonly InputAction m_Character_BinocucomScroll;
    private readonly InputAction m_Character_BinocucomPad;
    private readonly InputAction m_Character_Crawl;
    private readonly InputAction m_Character_Sprint;
    public struct CharacterActions
    {
        private @PlatformMap m_Wrapper;
        public CharacterActions(@PlatformMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @MouseCamera => m_Wrapper.m_Character_MouseCamera;
        public InputAction @PadCamera => m_Wrapper.m_Character_PadCamera;
        public InputAction @Jump => m_Wrapper.m_Character_Jump;
        public InputAction @Music => m_Wrapper.m_Character_Music;
        public InputAction @Exit => m_Wrapper.m_Character_Exit;
        public InputAction @Creep => m_Wrapper.m_Character_Creep;
        public InputAction @Interact => m_Wrapper.m_Character_Interact;
        public InputAction @Binocucom => m_Wrapper.m_Character_Binocucom;
        public InputAction @Attack => m_Wrapper.m_Character_Attack;
        public InputAction @BinocucomScroll => m_Wrapper.m_Character_BinocucomScroll;
        public InputAction @BinocucomPad => m_Wrapper.m_Character_BinocucomPad;
        public InputAction @Crawl => m_Wrapper.m_Character_Crawl;
        public InputAction @Sprint => m_Wrapper.m_Character_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @MouseCamera.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMouseCamera;
                @MouseCamera.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMouseCamera;
                @MouseCamera.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMouseCamera;
                @PadCamera.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnPadCamera;
                @PadCamera.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnPadCamera;
                @PadCamera.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnPadCamera;
                @Jump.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnJump;
                @Music.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMusic;
                @Music.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMusic;
                @Music.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMusic;
                @Exit.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnExit;
                @Creep.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCreep;
                @Creep.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCreep;
                @Creep.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCreep;
                @Interact.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnInteract;
                @Binocucom.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucom;
                @Binocucom.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucom;
                @Binocucom.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucom;
                @Attack.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAttack;
                @BinocucomScroll.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucomScroll;
                @BinocucomScroll.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucomScroll;
                @BinocucomScroll.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucomScroll;
                @BinocucomPad.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucomPad;
                @BinocucomPad.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucomPad;
                @BinocucomPad.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnBinocucomPad;
                @Crawl.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCrawl;
                @Crawl.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCrawl;
                @Crawl.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCrawl;
                @Sprint.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_CharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @MouseCamera.started += instance.OnMouseCamera;
                @MouseCamera.performed += instance.OnMouseCamera;
                @MouseCamera.canceled += instance.OnMouseCamera;
                @PadCamera.started += instance.OnPadCamera;
                @PadCamera.performed += instance.OnPadCamera;
                @PadCamera.canceled += instance.OnPadCamera;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Music.started += instance.OnMusic;
                @Music.performed += instance.OnMusic;
                @Music.canceled += instance.OnMusic;
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
                @Creep.started += instance.OnCreep;
                @Creep.performed += instance.OnCreep;
                @Creep.canceled += instance.OnCreep;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Binocucom.started += instance.OnBinocucom;
                @Binocucom.performed += instance.OnBinocucom;
                @Binocucom.canceled += instance.OnBinocucom;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @BinocucomScroll.started += instance.OnBinocucomScroll;
                @BinocucomScroll.performed += instance.OnBinocucomScroll;
                @BinocucomScroll.canceled += instance.OnBinocucomScroll;
                @BinocucomPad.started += instance.OnBinocucomPad;
                @BinocucomPad.performed += instance.OnBinocucomPad;
                @BinocucomPad.canceled += instance.OnBinocucomPad;
                @Crawl.started += instance.OnCrawl;
                @Crawl.performed += instance.OnCrawl;
                @Crawl.canceled += instance.OnCrawl;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public CharacterActions @Character => new CharacterActions(this);

    // Cutscene
    private readonly InputActionMap m_Cutscene;
    private ICutsceneActions m_CutsceneActionsCallbackInterface;
    private readonly InputAction m_Cutscene_Interact;
    public struct CutsceneActions
    {
        private @PlatformMap m_Wrapper;
        public CutsceneActions(@PlatformMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact => m_Wrapper.m_Cutscene_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Cutscene; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CutsceneActions set) { return set.Get(); }
        public void SetCallbacks(ICutsceneActions instance)
        {
            if (m_Wrapper.m_CutsceneActionsCallbackInterface != null)
            {
                @Interact.started -= m_Wrapper.m_CutsceneActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_CutsceneActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_CutsceneActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_CutsceneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public CutsceneActions @Cutscene => new CutsceneActions(this);
    private int m_NormalSchemeIndex = -1;
    public InputControlScheme NormalScheme
    {
        get
        {
            if (m_NormalSchemeIndex == -1) m_NormalSchemeIndex = asset.FindControlSchemeIndex("Normal");
            return asset.controlSchemes[m_NormalSchemeIndex];
        }
    }
    public interface ICharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnMouseCamera(InputAction.CallbackContext context);
        void OnPadCamera(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnMusic(InputAction.CallbackContext context);
        void OnExit(InputAction.CallbackContext context);
        void OnCreep(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnBinocucom(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnBinocucomScroll(InputAction.CallbackContext context);
        void OnBinocucomPad(InputAction.CallbackContext context);
        void OnCrawl(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface ICutsceneActions
    {
        void OnInteract(InputAction.CallbackContext context);
    }
}
