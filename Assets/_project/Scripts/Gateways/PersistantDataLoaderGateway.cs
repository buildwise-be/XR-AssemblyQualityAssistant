using System.IO;
using _project.Scripts.Entities;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace _project.Scripts.Gateways
{
    public class PersistantDataLoaderGateway : IFeedbackDataLoaderGateway
    {
        private string _path;
        private readonly string _directoryPath = Application.persistentDataPath+"/AssemblyData/";

        public AssemblyProcessDataEntity GetAssemblyData(string projectId)
        {
            _path = _directoryPath + projectId+".json";
            if (File.Exists(_path))
            {
                var json = File.ReadAllText(_path);
                return JsonConvert.DeserializeObject<AssemblyProcessDataEntity>(json);
            }
            else
            {
                return null;
            }
        }

        public void SaveData(AssemblyProcessDataEntity assemblyStepFeedbacks)
        {
            var json = JsonUtility.ToJson(assemblyStepFeedbacks,true);
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
            File.WriteAllText(_path, json);
            Debug.Log(json);
        }
    }
}
