namespace TestProject
{
    using HexaEngine.Core;
    using HexaEngine.Core.Editor.Attributes;
    using HexaEngine.Core.Scenes;
    using HexaEngine.Core.Scripts;
    using HexaEngine.Mathematics;
    using HexaEngine.Rendering;
    using HexaEngine.Scenes.Components.Renderer;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    
    public class SpriteAnimationTest : IScriptBehaviour
    {
        private Sprite sprite;
        private const int stateCount = 7;
        private int state = 0;
        private int animation = 0;
        private const int atlasWidth = 50;
        private const int atlasHeight = 37;
        private const int animationIndex = 0;
        private const int animationCount = 1;
        private float accumulation;

        public GameObject GameObject { get; set; }

        [EditorProperty]
        public float Delay { get; set; } = 0.3f;

        public void Awake()
        {
            sprite = GameObject.GetComponent<SpriteRendererComponent>().SpriteBatch[0];
        }

        public void Update()
        {
            accumulation += Time.Delta;
            while (accumulation > Delay)
            {
                accumulation -= Delay;

                sprite.AltasPos.X = atlasWidth * state;
                sprite.AltasPos.Y = atlasHeight * (animation + animationIndex);

                state++;
                if (state == stateCount)
                {
                    state = 0;
                    animation++;
                    if (animation == animationCount)
                        animation = 0;
                }
            }
        }

        public void Destory()
        {
            sprite.AltasPos.X = 0;
            sprite.AltasPos.Y = 0;
        }
    }
}