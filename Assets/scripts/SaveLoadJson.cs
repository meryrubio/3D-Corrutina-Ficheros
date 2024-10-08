using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

//guardar: ajustes(volumen..)
//un objeto que es capaz de convertirse a iun archivo en formato json. etc.. y viceversa (del objeto al archivo y del archivo al objeto) -> es un ARCHIVO SERIALIZABLE (como una herencia)
[System.Serializable] // indicar que este objeto es serializable
struct PlayerData //struct = es una clase pero mas peque�a -> para guardar atributos
{
    public Vector3 position;
    public int puntuation;
    public string stopHour;


}
public class SaveLoadJson : MonoBehaviour
{
    //json es un formato de archivo especial
    public string fileName = "test.json";
    

    public List<string> StopHours = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        fileName = Application.persistentDataPath + "\\" + fileName;
        
        Load();
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void OnDestroy()
    {
        StopHours.Add(System.DateTime.Now.ToString("HH:mm"));
        Save();
    }
    private void Save()
    {
        StreamWriter streamWriter = new StreamWriter(fileName);

        PlayerData playerData = new PlayerData(); // instancio el objeto que vamos a guardar
        playerData.position = transform.position;// lo rellenamos de informacion, escribimos la posici�n
        playerData.puntuation = GameManager.instance.GetPoints(); //escribimos la puntuacion

        playerData.stopHour = System.DateTime.Now.ToString("HH:mm");// escribimos la hora en la que deja de jugar

        string json = JsonUtility.ToJson(playerData); //Tojson = recibe un objeto serializable y nos genera el string del formato json 
        streamWriter.WriteLine(json);//escribimos lo  que queremos guardar

        streamWriter.Close();
    }

    private void Load()
    {
        if (File.Exists(fileName)) // si el archivo existe nos creamos el reader
        {
            StreamReader streamReader = new StreamReader(fileName);

            string json = streamReader.ReadToEnd();
            streamReader.Close();

            try
            {
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);//FromJson= string de formato json a objeto serializable -> playerdata es el objeto serializable/ y como queremos leer todo - readtoen
                transform.position = playerData.position; //establecemos la posicion del objeto serializable
                GameManager.instance.SetPoints(playerData.puntuation); //guardamos la puntuacion
              

                StopHours.Add(playerData.stopHour);//guardamos l ahora en la que deja de jugar
            }
            catch (System.Exception e)
            {
                //sacar al topo
                Debug.Log(e.Message);
            }

            
        }
    }
   
}

