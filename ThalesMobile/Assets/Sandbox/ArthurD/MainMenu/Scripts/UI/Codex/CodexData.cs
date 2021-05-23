using UnityEngine;


[System.Serializable]
public class CodexData 
{
    [Header("Information")]
    public string title;
    public Sprite categoIcon;
    [TextArea()]
    public string description = "Produit de Thalès qui est vachement utile";
    public Sprite image = null;
    public string linkName = "Mon lien http";
    public string link = "http:dsihgfszykhqfg";

}
