
using UnityEngine;

[CreateAssetMenu(fileName = "NewNote", menuName = "Notes/New Note")]
public class NoteData : ScriptableObject
{
    [Header("Основная информация")]
    [SerializeField] private int noteID;
    [SerializeField] private string noteTitle;
    [TextArea(5, 15)] [SerializeField] private string noteText;

    [Header("Визуал (необязательно)")]
    [SerializeField] private Sprite noteImage;

    // Публичные геттеры
    public int NoteID => noteID;
    public string NoteTitle => noteTitle;
    public string NoteText => noteText;
    public Sprite NoteImage => noteImage;
}
