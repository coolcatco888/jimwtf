﻿
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    public class Billboard : Component, IDrawableComponent
    {
        #region Fields

        protected VertexDeclaration vertexDeclaration = new VertexDeclaration(
                GameEngine.Graphics, VertexPositionTexture.VertexElements);

        /// <summary>
        /// Current position of this billboard on the world map.
        /// </summary>
        protected Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Scale of the billboard.
        /// </summary>
        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        protected SpriteInfo spriteInfo;

        protected VertexPositionTexture[] vertices;

        protected BasicEffect basicEffect;

        #endregion  // Fields

        public Billboard(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent)
        {
            visible = true;

            this.spriteInfo = spriteInfo;

            scale = 1.0f;
            position = new Vector3(0.0f, 0.0f, 0.0f);

            // Setup basic effect.
            basicEffect = new BasicEffect(GameEngine.Graphics, null);
            basicEffect.Texture = spriteInfo.SpriteSheet;
            basicEffect.TextureEnabled = true;

            // Four vertices to represet billboard.
            vertices = new VertexPositionTexture[4];

            // Assign position.
            vertices[0].Position = new Vector3(1, 3, 0);
            vertices[1].Position = new Vector3(-1, 3, 0);
            vertices[2].Position = new Vector3(-1, -1, 0);
            vertices[3].Position = new Vector3(1, -1, 0);

            // Assign texture coordinates to vertices.
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].TextureCoordinate = new Vector2(1, 1);
            vertices[3].TextureCoordinate = new Vector2(0, 1);
        }

        public Vector3 GetCenter()
        {
            Vector3 center;

            center.X = position.X;
            center.Y = spriteInfo.CenterHeight;
            center.Z = position.Z;

            return center;
        }

        #region IDrawableComponent Members

        public virtual void Draw(GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            GameEngine.Graphics.RenderState.AlphaTestEnable = true;
            GameEngine.Graphics.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
            GameEngine.Graphics.RenderState.ReferenceAlpha = 200;

            //GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceColor;


            // Assign world, view, & projection matricies to basicEffect.
            basicEffect.World = Matrix.CreateWorld(position, -camera.LookAt, Vector3.Up);
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;

            // Draw billboard.
            basicEffect.Begin();
            basicEffect.CurrentTechnique.Passes[0].Begin();

            GameEngine.Graphics.VertexDeclaration = vertexDeclaration;
            GameEngine.Graphics.DrawUserPrimitives(PrimitiveType.TriangleFan, vertices, 0, 2);

            basicEffect.CurrentTechnique.Passes[0].End();
            basicEffect.End();

            GameEngine.Graphics.RenderState.AlphaTestEnable = false;
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        #endregion

        public override void Dispose()
        {
            //spriteInfo.Dispose();

            base.Dispose();
        }
    }
}
