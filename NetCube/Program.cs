using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

//3D
using Silk.NET.OpenGL;
using System.Drawing;

namespace Game;

public class Program
{
    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode) { 
        Console.WriteLine(key);

        if (key == Key.Escape){
            _window.Close();
        }
    }

    private static unsafe void OnLoad() { 
        //Console.WriteLine("Load!");

        IInputContext input = _window.CreateInput();

        for (int i = 0; i < input.Keyboards.Count; i++){
            input.Keyboards[i].KeyDown += KeyDown;
        }

        _gl = _window.CreateOpenGL();

        _gl.ClearColor(Color.CornflowerBlue);

        _vao = _gl.GenVertexArray();

        _gl.BindVertexArray(_vao);

        float[] vertices = {
            0.5f,  0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
            -0.5f,  0.5f, 0.0f
        };

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        fixed (float* buf = vertices){
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        }

        uint[] indices = {
            0u, 1u, 3u,
            1u, 2u, 3u
        };

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

        fixed (uint* buf = indices)
        {
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);
        }

        const string vertexCode = @"
        #version 330 core

        layout (location = 0) in vec3 aPosition;

        void main()
        {
            gl_Position = vec4(aPosition, 1.0);
        }";

        const string fragmentCode = @"
        #version 330 core

        out vec4 out_color;

        void main()
        {
            out_color = vec4(1.0, 0.5, 0.2, 1.0);
        }";

        uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        
        _gl.ShaderSource(vertexShader, vertexCode);

        _gl.CompileShader(vertexShader);

        _gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);

        if (vStatus != (int) GLEnum.True)
            throw new Exception("Vertex shader failed to compile: " + _gl.GetShaderInfoLog(vertexShader));


    private static void OnUpdate(double deltaTime) { 
        //Console.WriteLine("Update!");
    }

    private static void OnRender(double deltaTime) { 
        //Console.WriteLine("Render!");
        _gl.Clear(ClearBufferMask.ColorBufferBit);
    }

    private static IWindow _window;
    private static GL _gl;
    private static uint _vao;
    private static uint _vbo;
    private static uint _ebo;

    public static void Main(string[] args) { 
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "My first Silk.NET application!"
        };  

        _window = Window.Create(options);

        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;

        _window.Run();
    }
}