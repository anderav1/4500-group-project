using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using testDB;
using System.Data;
using Mono.Data.Sqlite;
using System;

public class Registration : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;
    public Button returnButton;

    public Text errorText;

    private string dbPath;

    public void RegisterUser()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/game_db.db";

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
                    Debug.Log("User already exists");
                    errorText.gameObject.SetActive(true);
                } 
                else if (count == 0)
                {
                    cmd.CommandText = "INSERT INTO players (username, password) " +
                                      "VALUES (@Username, @Password);";

                    cmd.Parameters.Add(new SqliteParameter {
                        ParameterName = "Username",
                        Value = nameField.text
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Password",
                        Value = passwordField.text
                    });

                    var result = cmd.ExecuteNonQuery();
                    Debug.Log("Users added: " + result);
                    errorText.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("User creation error");
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
