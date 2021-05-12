using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;
    public Button returnButton;

    public Text errorText;

    private string dbPath;

    public void LogInUser()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/game_db.db"; // C:\Users\<COMPUTER NAME>\AppData\LocalLow\DefaultCompany\CS 4500 Wall Socket Game


        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT COUNT(username) " +
                                  "FROM players " +
                                  "WHERE username=@Username;";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Username",
                    Value = nameField.text
                });

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    cmd.CommandText = "SELECT password " +
                                      "FROM players " +
                                      "WHERE username=@Username;";


                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Username",
                        Value = nameField.text
                    });

                    string storedPW = cmd.ExecuteScalar().ToString();

                    if (passwordField.text == storedPW)
                    {
                        Debug.Log("Log in successful");
                        DBManager.username = nameField.text;
                        errorText.gameObject.SetActive(false);
                        SceneManager.LoadScene(0);
                    }
                    else
                    {
                        Debug.Log("Invalid password");
                        errorText.gameObject.SetActive(true);
                    }

                }
                else if (count == 0)
                {
                    Debug.Log("User doesn't exist");
                    errorText.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("User log in error");
                    errorText.gameObject.SetActive(true);
                }
            }
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 1 && passwordField.text.Length >= 1);
    }

    public void ReturnToMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
