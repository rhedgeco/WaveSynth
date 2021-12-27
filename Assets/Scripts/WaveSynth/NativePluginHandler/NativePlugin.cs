using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WaveSynth.NativePluginHandler
{
    public class NativePlugin
    {
        private bool _loaded;
        private IntPtr _pluginHandle;
        public readonly string PluginName;

        public bool Loaded => _loaded;

        public NativePlugin(string pluginName)
        {
            PluginName = pluginName;
        }

        public void LoadPlugin()
        {
            string path = $"{Application.dataPath}/Plugins/{PluginName}.dll";
            _pluginHandle = SystemLibrary.LoadLibrary(path);
            if (_pluginHandle == IntPtr.Zero)
                throw new Exception("Failed to load plugin [" + path + "]");
            _loaded = true;
        }

        public void UnloadPlugin()
        {
            if (!_loaded)
                throw new Exception($"Plugin [{PluginName}] is already unloaded.");
            SystemLibrary.FreeLibrary(_pluginHandle);
            _loaded = false;
        }

        public T ExtractFunction<T>(string functionName) where T : Delegate
        {
            if (!_loaded)
                throw new Exception($"Plugin [{PluginName}] has not been loaded.");
            IntPtr function = SystemLibrary.GetProcAddress(_pluginHandle, functionName);
            if (function == IntPtr.Zero)
                throw new Exception($"Failed to find function [{functionName}] in plugin [{PluginName}]\n" +
                                    $"{SystemLibrary.GetLastError()}");
            return (T) Marshal.GetDelegateForFunctionPointer(function, typeof(T));
        }

        private static class SystemLibrary
        {
            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("kernel32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("kernel32")]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

            [DllImport("kernel32.dll")]
            public static extern uint GetLastError();
        }
    }
}