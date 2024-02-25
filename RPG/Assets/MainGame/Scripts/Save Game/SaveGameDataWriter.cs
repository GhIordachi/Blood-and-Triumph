using System;
using System.IO;
using UnityEngine;

namespace GI
{
    public class SaveGameDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string dataSaveFileName = "";

        public CharacterSaveData LoadCharacterDataFromJson()
        {
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            CharacterSaveData loadedSaveData = null;

            if(File.Exists(savePath))
            {
                try
                {
                    string saveDataToLoad = "";

                    using(FileStream stream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            saveDataToLoad = reader.ReadToEnd();
                        }
                    }

                    //Deserialize data
                    loadedSaveData = JsonUtility.FromJson<CharacterSaveData>(saveDataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex.Message);
                }
            }
            else
            {
                Debug.Log("Save file does not exist!");
            }

            return loadedSaveData;
        }

        public void WriteCharacterDataToSaveFile(CharacterSaveData characterData)
        {
            //Creates a path to save our file
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("Save path = " + savePath);

                // Serialize the C# game data object to json
                string dataToStore = JsonUtility.ToJson(characterData, true);

                //write the file to our system
                using(FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while trying to save data, game could not be saved!" + ex);
            }
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, dataSaveFileName));
        }

        public bool CheckIfSaveFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath , dataSaveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
