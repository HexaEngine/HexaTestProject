#nullable disable

namespace TestProject
{
    using HexaEngine.Core.Editor.Attributes;
    using HexaEngine.Core.IO.Animations;
    using HexaEngine.Core.Scenes;
    using HexaEngine.Core.Scripts;
    using HexaEngine.Scenes.Components;
    using System.Diagnostics;

    public class AnimationTest : IScriptBehaviour
    {
        public GameObject GameObject { get; set; }

        [EditorProperty]
        public string Animation { get; set; } = "assets/animations/CesiumMan.anim";

        public void Awake()
        {
            var scene = GameObject.GetScene();
            var animator = GameObject.GetComponent<Animator>();
            if (animator != null)
            {
                Animation animation = scene.AnimationManager.Load(Animation);

                if (animation == null)
                {
                    for (int i = 0; i < scene.AnimationManager.Count; i++)
                    {
                        Trace.WriteLine(scene.AnimationManager.Animations[i].Name);
                    }
                    return;
                }

                animator.Play(animation);
            }
        }
    }
}