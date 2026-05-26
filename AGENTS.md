# FlowMotion Agent Instructions

## Communication
- Default language: Simplified Chinese.
- Keep responses concise, direct, and implementation-oriented.
- Do not generate or edit gameplay C# code unless the user explicitly overrides this rule.
- Prefer giving design review, implementation hints, checklists, debugging guidance, and small refactor suggestions for the user to implement.

## Project Goal
- Build a single-player third-person action movement demo inspired by Genshin Impact and Wuthering Waves.
- First priority is a polished local prototype: movement feel, state transitions, camera-relative control, animation events, and root-motion integration.
- Defer LAN/network multiplayer until after the v1.0 single-player slice is stable.

## Unity Constraints
- Unity version: 2022.3.62f3c1.
- Render pipeline: URP.
- Existing packages include Cinemachine, Timeline, TextMeshPro, UGUI, and Unity Test Framework.
- Prefer edits under `Assets/`, `Packages/`, and `ProjectSettings/` only when necessary.
- Do not modify Unity cache or generated folders: `Library/`, `Temp/`, `Logs/`, `Obj/`, `Build/`, `Builds/`, `UserSettings/`.
- Preserve `.meta` files and GUID relationships.
- Do not delete or overwrite scenes, imported assets, or third-party files without explicit confirmation.

## Architecture Direction
- Use a hybrid movement model: `CharacterController` controls normal movement, while key actions can consume root motion.
- Keep systems modular:
  - `PlayerInputReader`: input, buffering, press/hold detection.
  - `PlayerMotor`: movement, gravity, slope handling, collision.
  - `PlayerAnimatorBridge`: Animator parameters, animation events, root-motion handoff.
  - `PlayerStateMachine`: gameplay state transitions outside Animator logic.
  - `PlayerContext`: shared references to input, motor, animator, config, and environment checks.
  - `ScriptableObject` configs: movement speed, dash cooldowns, landing thresholds, combat windows, climb/swim parameters.
- Prefer top-level state groups: Ground, Air, Climb, Swim, CombatAction.
- Treat climb-to-ground as a transition state, not both climb and grounded at once.
- Keep attacks as action states or an action layer so movement can be locked, slowed, or cancelled by windows.

## Version Roadmap
- v0.0: Project baseline. Keep `Assets/Scripts/总览.md` as the design draft, maintain this `AGENTS.md`, and prepare a simple test scene concept with player, slope, wall, and water markers.
- v0.1: Basic third-person control. Camera follow, WASD camera-relative movement, Idle/Walk/Run placeholders.
- v0.2: Ground movement loop. Sprint, Ctrl walk/run toggle, Stop state, foot-phase stop windows, random idle variants.
- v0.3: Dash system. Directional dash, default back dash, right-click hold to enter sprint, double-dash cooldown, root-motion dash window.
- v0.4: Air system. Jump, Fall, Glide, Air Dash, Light Land, Heavy Land, jump buffering, coyote time, fall-height thresholds.
- v0.5: Climb system. Wall detection, climb idle, eight-direction climb BlendTree concept, climb dash, climb-to-ground transition.
- v0.6: Water system. Water fall, swim idle, swim, fast swim, shallow-water return to ground movement.
- v0.7: Basic combat. Normal attack combo, combo input window, timeout reset, plunge attack, movement lock/slow, attack root-motion window.
- v0.8: Perfect dash and enemy prototype. Simple enemy AI, hitboxes/hurtboxes, perfect dash timing, brief slow motion, feedback effects.
- v0.9: Data and feedback. Move tunable parameters into ScriptableObjects; add audio, VFX, camera shake, and debug UI.
- v1.0: Single-player playable slice showing movement, dash, jump, glide, landing, climb, swim, attacks, perfect dash, and enemy interaction.

## Verification Guidance
- For each version, maintain a short manual acceptance checklist.
- Prioritize testing state deadlocks, missed animation events, input buffering mistakes, root-motion double movement, slope behavior, and cancel windows.
- From v0.3 onward, validate with a 30-second manual flow: move, stop, dash, jump, land, attack, and recover.

## Collaboration Rules
- When asked to review code, lead with bugs, risks, regressions, and missing tests.
- When asked for implementation help, give steps and reasoning first; only write code if explicitly requested.
- Keep changes minimal and targeted.
- Call out any change that affects Animator parameters, animation events, ScriptableObject fields, scene references, or serialized names.
