using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using Android.Views;
using Android.Content;
using Android.Util;

namespace PonderApp
{
    class GLView1 : AndroidGameView
    {
        public GLView1(Context context) : base(context)
        {
            // do not set context on render frame as we will be rendering
            // on separate thread and thus Android will not set GL context
            // behind our back
            AutoSetContextOnRenderFrame = false;

            // render on separate thread. this gains us
            // fluent rendering. be careful to not use GL calls on UI thread.
            // OnRenderFrame is called from rendering thread, so do all
            // the GL calls there
            RenderOnUIThread = false;
        }

        // This gets called when the drawing surface is ready
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Run the render loop
            Run();
        }

        // This method is called everytime the context needs
        // to be recreated. Use it to set any egl-specific settings
        // prior to context creation
        //
        // In this particular case, we demonstrate how to set
        // the graphics mode and fallback in case the device doesn't
        // support the defaults
        protected override void CreateFrameBuffer()
        {
            // the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
            try
            {
                Log.Verbose("GLCube", "Loading with default settings");

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                return;
            }
            catch (Exception ex)
            {
                Log.Verbose("GLCube", ex.ToString());
            }

            // this is a graphics setting that sets everything to the lowest mode possible so
            // the device returns a reliable graphics setting.
            try
            {
                Log.Verbose("GLCube", "Loading with custom Android settings (low mode)");
                GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                return;
            }
            catch (Exception ex)
            {
                Log.Verbose("GLCube", ex.ToString());
            }
            throw new Exception("Can't load egl, aborting");
        }

        // This gets called on each frame render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // you only need to call this if you have delegates
            // registered that you want to have called
            base.OnRenderFrame(e);

            GL.MatrixMode(All.Projection);
            GL.LoadIdentity();

            float aspect = (float)Width / (float)Height;
            GL.Ortho(-aspect, aspect, -1.0f, 1.0f, -1.0f, 1.0f);

            GL.MatrixMode(All.Modelview);
            GL.Rotate(3.0f, 0.0f, 0.0f, 1.0f);

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.VertexPointer(2, All.Float, 0, square_vertices);
            GL.EnableClientState(All.VertexArray);
            GL.ColorPointer(4, All.UnsignedByte, 0, square_colors);
            GL.EnableClientState(All.ColorArray);

            GL.DrawArrays(All.TriangleStrip, 0, 4);

            SwapBuffers();
        }

        float[] square_vertices = {
            -0.5f, -0.5f,
            0.5f, -0.5f,
            -0.5f, 0.5f,
            0.5f, 0.5f,
        };

        byte[] square_colors = {
            255, 255,   0, 255,
            0,   255, 255, 255,
            0,     0,    0,  0,
            255,   0,  255, 255,
        };
    }
}