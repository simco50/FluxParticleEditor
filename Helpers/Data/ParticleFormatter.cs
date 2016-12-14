using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParticleEditor.Data.ParticleSystem;

namespace ParticleEditor.Helpers.Data
{
    public static class ParticleFormatter
    {
        private static string _currentLocation = "";

        public static bool SaveAs(ParticleSystem particleSystem)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "Json (.json) | *.json";
            bool? success = dialog.ShowDialog();
            if (success == false)
                return false;
            try
            {
                DebugLog.Log($"Export to '{dialog.FileName}'...", "Json export");
                string data = JsonConvert.SerializeObject(particleSystem, Formatting.Indented);
                File.WriteAllText(dialog.FileName, data);
                _currentLocation = dialog.FileName;
                DebugLog.Log("Export successful", "Json export");
                return true;
            }
            catch (Exception exception)
            {
                DebugLog.Log(exception.Message, "Json export failed!", LogSeverity.Warning);
                return false;
            }
        }

        public static bool Save(ParticleSystem particleSystem)
        {
            if(_currentLocation == "")
                return SaveAs(particleSystem);
            try
            {
                DebugLog.Log($"Export to '{_currentLocation}'...", "Json export");
                string data = JsonConvert.SerializeObject(particleSystem, Formatting.Indented);
                File.WriteAllText(_currentLocation, data);
                DebugLog.Log("Export successful", "Json export");
                return true;
            }
            catch (Exception exception)
            {
                DebugLog.Log(exception.Message, "Json export failed!", LogSeverity.Warning);
                return false;
            }
        }

        public static ParticleSystem Open()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "Json (.json) | *.json";
            bool? success = dialog.ShowDialog();
            if (success == false)
                return null;
            try
            {
                DebugLog.Log($"Import from {dialog.FileName}...", "Json import");
                string data = File.ReadAllText(dialog.FileName);
                ParticleSystem particleSystem = JsonConvert.DeserializeObject<ParticleSystem>(data);
                DebugLog.Log("Import successful");
                _currentLocation = dialog.FileName;
                return particleSystem;
            }
            catch (Exception exception)
            {
                DebugLog.Log(exception.Message, "Json import failed!", LogSeverity.Warning);
                return null;
            }
        }

        public static ParticleSystem MakeNewSystem()
        {
            _currentLocation = "";
            return new ParticleSystem();
        }
    }
}
