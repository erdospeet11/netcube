using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace MySilkProgram;

public class Program
{
    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode) { 
        Console.WriteLine(key);

        if (key == Key.Escape){
            _window.Close();
        }
    }

    private static void OnLoad() { 
        //Console.WriteLine("Load!");

        IInputContext input = _window.CreateInput();

        for (int i = 0; i < input.Keyboards.Count; i++){
            input.Keyboards[i].KeyDown += KeyDown;
        }
    }

    private static void OnUpdate(double deltaTime) { 
        //Console.WriteLine("Update!");
    }

    private static void OnRender(double deltaTime) { 
        //Console.WriteLine("Render!");
    }

    private static IWindow _window;
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