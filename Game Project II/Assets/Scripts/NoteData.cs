using UnityEngine;

[CreateAssetMenu(fileName = "NewNote", menuName = "Notes/New Note")]
public class NoteData : ScriptableObject
{
    [Header("Основная информация")]
    public int noteID;
    public string noteTitle;
    [TextArea(5, 15)] public string noteText;

    [Header("Визуал (необязательно)")]
    public Sprite noteImage;
}