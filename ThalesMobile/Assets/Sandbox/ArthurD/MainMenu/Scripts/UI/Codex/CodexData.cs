using UnityEngine;


[System.Serializable]
public class CodexData 
{
    [Header("Information")]
    public string title;
    public Sprite categoIcon;
    [TextArea()]
    public string[] description = new string[3]
    {
      "Produit de Thalès qui est vachement utile",
      "En plus il ne coute que quelque % de ton PIB",
      "Il ne faut pas dire crunch mais surtravail volontaire durant les vancances #ProgTeam"
    };
    
}
