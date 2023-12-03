using System;
using System.IO;
using System.Xml.Serialization;

namespace JohnCricketFishingGame.Source
{
        
    [Serializable]
    public class SaveData
    {

        private XmlSerializer serializer;
        private string _filePath;
        private string _containerName = "/JohnCricketGame/SaveData/";
        private string _fileName = "saves.sav";
            
        public int score;

        public SaveData()
        {
            _filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{_containerName}";
            serializer = new XmlSerializer(typeof(SaveData));
            
            
            if (!File.Exists(_filePath + _fileName))
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + _fileName).Close();

                score = 0;

                using (Stream stream = File.Open(_filePath + _fileName, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(stream, this);
                }
            }
        }


        public void SaveToDevice()
        {
            score = GameStats.HighScore;

            if (!File.Exists(_filePath + _fileName))
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + _fileName).Close();
            }
            using (Stream stream = File.Open(_filePath + _fileName, FileMode.OpenOrCreate))
            {
                serializer.Serialize(stream, this);
            }
        }

        public int LoadFromDevice()
        {
            SaveData save;

            int retVal = 0;

            if (!File.Exists(_filePath + _fileName))
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + _fileName).Close();
            }
            using (Stream stream = File.Open(_filePath + _fileName, FileMode.OpenOrCreate))
            {
                save = (SaveData) serializer.Deserialize(stream);
                retVal = save.score;
            }

            return retVal;
        } 
    }
}